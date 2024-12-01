using UnityEngine;
using Fishing.PlayerInput;

namespace Fishing.PlayerCamera {
    public class CameraBehaviour : MonoBehaviour {
        [Header("Position And Tracking")]
        [SerializeField, Tooltip("Position camera rests at when player is idle. Set on start.")] private Vector2 _defaultPosition;

        [SerializeField, Range(0f, 1.0f), Tooltip("Minimum speed for lerping camera to target position.")] private float _minFollowSpeed = 0.001f;
        [SerializeField, Range(0f, 1.0f), Tooltip("Maximum speed for lerping camera to target position.")] private float _maxFollowSpeed = 0.05f;
        [SerializeField, Range(0f, 1.0f), Tooltip("Distance camera will stop moving to follow target position.")] private float _followThreshold = 0.1f;

        [Header("Zoom")]
        [SerializeField, Range(0f, 1.0f), Tooltip("Speed camera will zoom in at (not scaled with deltaTime).")] private float _zoomSpeed = 0.5f;
        [SerializeField, Tooltip("Value difference camera will stop adjusting zoom to match target zoom.")] private float _zoomThreshold = 0.01f;

        [SerializeField, Min(0), Tooltip("Amount Camera.orthographicSize is adjusted for each zoom action.")] private float _zoomMagnitude = 1;
        [SerializeField, Min(0), Tooltip("Minimum value for Camera.orthographicSize when zooming.")] private float _minPlayerZoom = 1;
        [SerializeField, Min(0), Tooltip("Maximum value for Camera.orthographicSize when zooming.")] private float _maxPlayerZoom = 20;

        private bool _lockZoom = false;
        private bool _lockPosition = false;
        private bool _lockPlayerControls = false;

        private Vector2 _desiredPosition;
        private float _desiredZoom;

        private float _playerZoom;
        private float _tempZoom;

        private bool _activeUI;

        public bool ActiveUI {
            get { return _activeUI; }
            set { _activeUI = value; }
        }

        private bool _activeUILastFrame;

        private static CameraBehaviour _instance;
        public static CameraBehaviour Instance { get => _instance; private set => _instance = value; }

        private Camera _camera;
        public Camera Camera { get => _camera; set => _camera = value; }


        private CameraBehaviour() => Instance = this;

        private void Awake() {
            Camera = GetComponent<Camera>();
            InputManager.onZoomIn += CameraZoomIn;
            InputManager.onZoomOut += CameraZoomOut;
        }

        private void Start() {
            _lockZoom = _lockPosition = _lockPlayerControls = _activeUILastFrame = false;
            _playerZoom = _desiredZoom = _tempZoom = Camera.orthographicSize;
            _desiredPosition = _defaultPosition = Camera.transform.position;
        }

        private void Update() {
            if (ActiveUI) {
                _lockPlayerControls = true;
            }

            else if (_activeUILastFrame) {
                _lockPlayerControls = false; // not a great solution, potentially replace with subscription to a "close active ui" subscription to turn off control lock
            }

            _desiredZoom = _lockPlayerControls ? _tempZoom : _playerZoom;
            if (!_lockZoom) {
                HandleCameraZoom();
            }
            if (!_lockPosition) {
                HandleCameraPosition();
            }

            _activeUILastFrame = ActiveUI;
        }

        private void HandleCameraZoom() {
            if (Mathf.Abs(Camera.orthographicSize - _desiredZoom) <= _zoomThreshold) { 
                return;
            }
            Camera.orthographicSize = Mathf.Lerp(Camera.orthographicSize, _desiredZoom, _zoomSpeed);
        }

        private void HandleCameraPosition() {
            float viewportSqrDistance = GetViewportSqrDistanceFromCenter(_desiredPosition);
            if (viewportSqrDistance < _followThreshold * _followThreshold) { 
                return; 
            }

            float speed = Mathf.Lerp(_minFollowSpeed, _maxFollowSpeed, viewportSqrDistance);
            float distance = Vector2.Distance(Camera.transform.position, _desiredPosition);
            Vector2 newPos = Vector2.MoveTowards(Camera.transform.position, _desiredPosition, distance * speed);
            Camera.transform.position = new Vector3(newPos.x, newPos.y, Camera.transform.position.z);
        }

        private float GetViewportSqrDistanceFromCenter(Vector2 pos) {
            Vector2 posViewport = Camera.WorldToViewportPoint(pos);
            posViewport = new Vector2(posViewport.x * 2 - 1, posViewport.y * 2 - 1);
            float viewportDistanceMagnitude = posViewport.x * posViewport.x + posViewport.y * posViewport.y;

            return viewportDistanceMagnitude;
        }

        private void CameraZoomIn() {
            if (_lockPlayerControls) { 
                return; 
            }
            _playerZoom = Mathf.Clamp(_playerZoom - _zoomMagnitude, _minPlayerZoom, _maxPlayerZoom);
        }
        private void CameraZoomOut() {
            if (_lockPlayerControls) { 
                return; 
            }
            _playerZoom = Mathf.Clamp(_playerZoom + _zoomMagnitude, _minPlayerZoom, _maxPlayerZoom);
        }

        public void ReturnHome() => _desiredPosition = _defaultPosition;

        /// <summary>
        /// Determines whether the given position is within the camera's frame.
        /// </summary>
        public bool IsInFrame(Vector2 pos) {
            Vector2 viewportPos = Camera.WorldToViewportPoint(pos);

            if (viewportPos.x < 0f || viewportPos.x > 1f || viewportPos.y < 0f || viewportPos.y > 1f) { 
                return false; 
            }

            else return true;
        }

        /// <summary>
        /// Returns orthographic size needed to cover the given parameter's distance.
        /// </summary>
        public float ViewDistanceToCameraZoom(float distance) {
            // orthographic size determines the radius of vertical view distance of the camera.
            // So we convert vertical to horizontal (based on the 16:9 aspect ratio) and half it
            // to return the orthographic size needed to cover a known vertical distance.
            return distance *= 0.5625f * 0.5f;
        }

        public void SetDesiredPosition(Vector2 pos) => _desiredPosition = pos;

        public void EnablePlayerControls() => _lockPlayerControls = false;
        public void DisablePlayerControls() => _lockPlayerControls = true;
        public void SetTempZoom(float tempZoom) => _tempZoom = tempZoom;
    }
}
