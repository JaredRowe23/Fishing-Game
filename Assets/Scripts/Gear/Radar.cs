using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.FishingMechanics;
using Fishing.Fishables;
using UnityEngine.UI;
using Fishing.IO;

namespace Fishing.Gear
{
    public class Radar : MonoBehaviour
    {
        [SerializeField] private GameObject radarCameraPrefab;

        [Header("Target Fish")]
        [SerializeField] private List<string> fishableNames;
        [SerializeField] private string targetFishableType;

        [Header("Map Zoom")]
        [SerializeField] private float maxZoom;
        [SerializeField] private float minZoom;
        [SerializeField] private float zoomStep;

        [Header("Scan Range")]
        [SerializeField] private float scanRange;
        [SerializeField] private float maxScanRange;
        [SerializeField] private float minScanRange;
        [SerializeField] private float scanRangeStep;

        [Header("Scan Frequency")]
        [SerializeField] private float scanFrequency;
        [SerializeField] private float maxScanFrequency;
        [SerializeField] private float minScanFrequency;
        [SerializeField] private float scanFrequencyStep;

        [Header("Ping")]
        [SerializeField] private float pingRange;

        private Camera radarCamera;
        private Dropdown fishDropdown;
        private LineRenderer line;
        private Vector2 rayDirection;

        private HookBehaviour hook;

        public static Radar instance;

        private Radar() => instance = this;
        private void Awake()
        {
            radarCamera = Instantiate(radarCameraPrefab).GetComponent<Camera>();
            line = radarCamera.GetComponent<LineRenderer>();
            fishDropdown = GetComponentInChildren<Dropdown>();
        }

        private void Start()
        {
            rayDirection = Vector2.up;
            fishDropdown.ClearOptions();
            fishDropdown.AddOptions(fishableNames);
            fishDropdown.value = 0;
        }

        private void Update()
        {
            CenterCamera();

            rayDirection = Quaternion.Euler(0, 0, -360f * Time.deltaTime / scanFrequency) * rayDirection;
            CheckForScannableObjects();

            SetLinePositions(radarCamera.transform.position, rayDirection * scanRange);
        }

        private void CenterCamera()
        {
            hook = RodManager.instance.equippedRod.GetHook();
            radarCamera.transform.position = hook.transform.position;
        }

        private void CheckForScannableObjects()
        {
            RaycastHit2D[] _hits = Physics2D.RaycastAll(radarCamera.transform.position, rayDirection, scanRange);
            foreach (RaycastHit2D _hit in _hits)
            {
                if (!_hit.collider.gameObject.GetComponent<Fishable>()) continue;
                if (_hit.collider.gameObject.GetComponent<Fishable>().ItemName != fishDropdown.options[fishDropdown.value].text) continue;

                _hit.collider.gameObject.GetComponent<Fishable>().MinimapIndicator.GetComponent<RadarScanObject>().Scan();
            }
        }

        private void SetLinePositions(Vector2 _fromPos, Vector2 _toPos)
        {
            line.SetPosition(0, _fromPos);
            line.SetPosition(1, _toPos);
        }

        public void IncreaseZoom()
        {
            if (radarCamera.orthographicSize > maxZoom) radarCamera.orthographicSize -= zoomStep;
        }
        public void DecreaseZoom()
        {
            if (radarCamera.orthographicSize < minZoom) radarCamera.orthographicSize += zoomStep;
        }
        public void IncreaseRange()
        {
            if (scanRange < maxScanRange) scanRange += scanRangeStep;
        }
        public void DecreaseRange()
        {
            if (scanRange > minScanRange) scanRange -= scanRangeStep;
        }
        public void IncreaseFrequency()
        {
            if (scanFrequency < maxScanFrequency) scanFrequency += scanFrequencyStep;
        }
        public void DecreaseFrequency()
        {
            if (scanFrequency < maxScanFrequency) scanFrequency -= scanFrequencyStep;
        }
    }
}
