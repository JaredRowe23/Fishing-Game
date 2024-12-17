using Fishing.Fishables;
using Fishing.Fishables.FishGrid;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.Gear {
    public class Radar : MonoBehaviour {
        [SerializeField, Tooltip("Prefab for the camera object that renders the radar object against the minimap.")] private GameObject _radarCameraPrefab;
        [SerializeField, Tooltip("Dropdown that holds the fishable options for radar scanning.")] private Dropdown _fishDropdown;

        [Header("Map Zoom")]
        [SerializeField, Min(0), Tooltip("Minimum orthographic size for the radar camera. This translates to maximum zoom.")] private float _minimumOrthographicSize = 25f;
        [SerializeField, Min(0), Tooltip("Maximum orthographic size for the radar camera. This translates to minimum zoom.")] private float _maximumOrthographicSize = 75f;
        [SerializeField, Min(0), Tooltip("Amount of zoom to add or subtract when the player attempts to change the zoom.")] private float _zoomStep = 5f;

        [Header("Scan Range")]
        [SerializeField, Min(0), Tooltip("Minimum distance in meters the radar is able to scan down to.")] private float _minScanRange = 10f;
        [SerializeField, Min(0), Tooltip("Maximum distance in meters the radar is able to scan up to.")] private float _maxScanRange = 50f;
        [SerializeField, Min(0), Tooltip("Distance to add or subtract when the player attempts to change the scan range.")] private float _scanRangeStep = 5f;

        [Header("Scan Frequency")]
        [SerializeField, Min(0), Tooltip("Minimum frequency in revolutions per second the radar can be set down to.")] private float _minScanFrequency = 0.5f;
        [SerializeField, Min(0), Tooltip("Maximum frequency in revolutions per second the radar can be set up to.")] private float _maxScanFrequency = 10f;
        [SerializeField, Min(0), Tooltip("Frequency change to add or subtract when the player attempts to change the frequency.")] private float _scanFrequencyStep = 0.5f;

        private float _scanRange = 10f;
        private float _scanFrequency = 1f;

        private float _scanAngle;

        private Vector2 _scanDirection;
        private Vector2 _previousScanDirection;

        private Camera _radarCamera;
        private LineRenderer _radarScanLine;
        private RodManager _rodManager;
        private FishableGrid _fishableGrid;

        private void OnValidate() {
            if (_minimumOrthographicSize > _maximumOrthographicSize) {
                _minimumOrthographicSize = _maximumOrthographicSize;
            }

            if (_minScanRange > _maxScanRange) {
                _minScanRange = _maxScanRange;
            }

            if (_minScanFrequency > _maxScanFrequency) {
                _minScanFrequency = _maxScanFrequency;
            }
        }

        private void Start() {
            _rodManager = RodManager.Instance;
            _fishableGrid = FishableGrid.instance;

            _radarCamera = Instantiate(_radarCameraPrefab).GetComponent<Camera>();
            _radarScanLine = _radarCamera.GetComponent<LineRenderer>();

            _scanAngle = 0f;

            _scanDirection = Vector2.up;
            _previousScanDirection = Vector2.up;

            _fishDropdown.ClearOptions();
            List<string> fishableTypeStrings = new List<string> (Enum.GetNames(typeof(Fishable.ItemTypes)));
            _fishDropdown.AddOptions(fishableTypeStrings);
            _fishDropdown.value = 0;

            _radarCamera.orthographicSize = (_minimumOrthographicSize + _maximumOrthographicSize) * 0.5f;
            _scanRange = (_minScanRange + _maxScanRange) * 0.5f;
            _scanFrequency = (_minScanFrequency + _maxScanFrequency) * 0.5f;
        }

        private void Update() {
            _radarCamera.transform.position = _rodManager.EquippedRod.Hook.transform.position;

            _previousScanDirection = _scanDirection;

            _scanAngle += (-360f * Time.deltaTime / _scanFrequency) % 360f;
            _scanDirection = new Vector2(Mathf.Cos(_scanAngle * Mathf.Deg2Rad), Mathf.Sin(_scanAngle * Mathf.Deg2Rad)).normalized;

            CheckForScannableObjects();

            SetLinePositions(_radarCamera.transform.position, (Vector2)_radarCamera.transform.position + _scanDirection * _scanRange);
        }

        private void CheckForScannableObjects() {
            Enum.TryParse(_fishDropdown.options[_fishDropdown.value].text, true, out Fishable.ItemTypes scanType);

            int[] currentGridSquare = _fishableGrid.Vector2ToGrid(_radarCamera.transform.position);
            List<Fishable> fishables = _fishableGrid.GetNearbyFishables(currentGridSquare[0], currentGridSquare[1], _scanRange);

            for (int i = 0; i < fishables.Count; i++) {
                if (scanType != fishables[i].FishableType) {
                    continue;
                }

                float fishableDistance = Vector2.Distance(fishables[i].transform.position, _radarCamera.transform.position);
                if (fishableDistance > _scanRange) {
                    continue;
                }

                float scanDirectionsDot = Vector2.Dot(_previousScanDirection, _scanDirection);
                float fishableDot = Vector2.Dot(_previousScanDirection, (fishables[i].transform.position - _radarCamera.transform.position).normalized);

                if (fishableDot >= scanDirectionsDot) {
                    fishables[i].MinimapIndicator.Scan();
                }
            }
        }

        private void SetLinePositions(Vector2 _fromPos, Vector2 _toPos) {
            _radarScanLine.SetPosition(0, _fromPos);
            _radarScanLine.SetPosition(1, _toPos);
        }

        public void IncreaseZoom() {
            _radarCamera.orthographicSize -= _zoomStep;

            if (_radarCamera.orthographicSize < _minimumOrthographicSize) {
                _radarCamera.orthographicSize = _minimumOrthographicSize;
            }
        }

        public void DecreaseZoom() {
            _radarCamera.orthographicSize += _zoomStep;

            if (_radarCamera.orthographicSize > _maximumOrthographicSize) {
                _radarCamera.orthographicSize = _maximumOrthographicSize;
            }
        }

        public void IncreaseRange() {
            _scanRange += _scanRangeStep;

            if (_scanRange > _maxScanRange) {
                _scanRange = _maxScanRange;
            }
        }

        public void DecreaseRange() {
            _scanRange -= _scanRangeStep;

            if (_scanRange < _minScanRange) {
                _scanRange = _minScanRange;
            }
        }

        public void IncreaseFrequency() {
            _scanFrequency += _scanFrequencyStep;

            if (_scanFrequency > _maxScanFrequency) {
                _scanFrequency = _maxScanFrequency;
            }
        }
        public void DecreaseFrequency() {
            _scanFrequency -= _scanFrequencyStep;

            if (_scanFrequency < _minScanFrequency) {
                _scanFrequency = _minScanFrequency;
            }
        }
    }
}
