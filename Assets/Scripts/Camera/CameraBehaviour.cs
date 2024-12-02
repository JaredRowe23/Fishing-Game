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
        [SerializeField, Min(0f), Tooltip("Value difference camera will stop adjusting zoom to match target zoom.")] private float _zoomThreshold = 0.01f;

        [SerializeField, Min(0), Tooltip("Amount Camera.orthographicSize is adjusted for each zoom action.")] private float _zoomMagnitude = 1;
        [SerializeField, Min(0), Tooltip("Minimum value for Camera.orthographicSize when zooming.")] private float _minPlayerZoom = 1;
        [SerializeField, Min(0), Tooltip("Maximum value for Camera.orthographicSize when zooming.")] private float _maxPlayerZoom = 20;

        private bool _lockZoom = false;
        public bool LockZoom { get => _lockZoom; set => _lockZoom = value; }
        private bool _lockPosition = false;
        public bool LockPosition { get => _lockPosition; set => _lockPosition = value; }
        private bool _lockPlayerControls = false;
        public bool LockPlayerControls { get => _lockPlayerControls; set => _lockPlayerControls = value; }

        private Vector2 _desiredPosition;
        public Vector2 DesiredPosition { get => _desiredPosition; set => _desiredPosition = value; }
        private float _desiredZoom;

        private float _playerZoom;
        private float _tempZoom;
        public float TempZoom { get => _tempZoom; set => _tempZoom = value; }

        private bool _activeUI;
        public bool ActiveUI { get { return _activeUI; } set { _activeUI = value; } }
        private bool _activeUILastFrame = false;

        private static CameraBehaviour _instance;
        public static CameraBehaviour Instance { get => _instance; private set => _instance = value; }

        private Camera _camera;
        public Camera Camera { get => _camera; private set => _camera = value; }

        private CameraBehaviour() => Instance = this;

        private void OnValidate() {
            if (_minFollowSpeed > _maxFollowSpeed) {
                _minFollowSpeed = _maxFollowSpeed;
            }
            if (_minPlayerZoom > _maxPlayerZoom) {
                _minPlayerZoom = _maxPlayerZoom;
            }
        }

        private void Awake() {
            Camera = GetComponent<Camera>();
            InputManager.onZoomIn += CameraZoomIn;
            InputManager.onZoomOut += CameraZoomOut;
        }

        private void Start() {
            _playerZoom = _desiredZoom = TempZoom = Camera.orthographicSize;
            DesiredPosition = _defaultPosition = Camera.transform.position;
        }

        private void Update() {
            if (ActiveUI) {
                LockPlayerControls = true;
            }

            else if (_activeUILastFrame) {
                LockPlayerControls = false; // not a great solution, potentially replace with subscription to a "close active ui" subscription to turn off control lock
            }

            _desiredZoom = LockPlayerControls ? TempZoom : _playerZoom;
            if (!LockZoom) {
                HandleCameraZoom();
            }
            if (!LockPosition) {
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
            float viewportSqrDistance = GetViewportSqrDistanceFromCenter(DesiredPosition);
            if (viewportSqrDistance < _followThreshold * _followThreshold) { 
                return; 
            }

            float speed = Mathf.Lerp(_minFollowSpeed, _maxFollowSpeed, viewportSqrDistance);
            float distance = Vector2.Distance(Camera.transform.position, DesiredPosition);
            Vector2 newPos = Vector2.MoveTowards(Camera.transform.position, DesiredPosition, distance * speed);
            Camera.transform.position = new Vector3(newPos.x, newPos.y, Camera.transform.position.z);
        }

        private float GetViewportSqrDistanceFromCenter(Vector2 pos) {
            Vector2 posViewport = Camera.WorldToViewportPoint(pos);
            posViewport = new Vector2(posViewport.x * 2 - 1, posViewport.y * 2 - 1);
            float viewportDistanceMagnitude = posViewport.x * posViewport.x + posViewport.y * posViewport.y;

            return viewportDistanceMagnitude;
        }

        public void CameraZoomIn() {
            if (LockPlayerControls) { 
                return; 
            }
            _playerZoom = Mathf.Clamp(_playerZoom - _zoomMagnitude, _minPlayerZoom, _maxPlayerZoom);
        }
        public void CameraZoomOut() {
            if (LockPlayerControls) { 
                return; 
            }
            _playerZoom = Mathf.Clamp(_playerZoom + _zoomMagnitude, _minPlayerZoom, _maxPlayerZoom);
        }

        public void ReturnHome() => DesiredPosition = _defaultPosition;

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
    }
}
