using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.IO;
using Fishing.UI;

namespace Fishing.PlayerCamera
{
    public class CameraBehaviour : MonoBehaviour
    {
        public bool lockZoom = false;
        public bool lockPosition = false;
        public bool lockPlayerControls = false;

        [SerializeField] private Vector2 defaultPosition;

        [SerializeField] private Vector2 desiredPosition;
        private float desiredZoom;

        [Range(0f, 1.0f)]
        [SerializeField] private float minFollowSpeed = 0.1f;
        [Range(0f, 1.0f)]
        [SerializeField] private float maxFollowSpeed = 1f;
        [SerializeField] private float followThreshold = 0.1f;

        [Range(0f, 1.0f)]
        [SerializeField] private float zoomSpeed = 0.5f;
        [SerializeField] private float zoomThreshold = 0.1f;


        [SerializeField] private float zoomMagnitude = 1;
        [SerializeField] private float maxPlayerZoom = 20;
        [SerializeField] private float minPlayerZoom = 1;
        private float playerZoom = 20;

        public static CameraBehaviour instance;
        public Camera cam;

        private CameraBehaviour() => instance = this;

        private void Awake()
        {
            cam = GetComponent<Camera>();
            InputManager.onZoomIn += CameraZoomIn;
            InputManager.onZoomOut += CameraZoomOut;
        }

        private void Start()
        {
            lockZoom = lockPosition = lockPlayerControls = false;
            playerZoom = desiredZoom = cam.orthographicSize;
            desiredPosition = defaultPosition = (Vector2)cam.transform.position;
        }

        private void Update()
        {
            lockPlayerControls = desiredZoom != playerZoom;
            if (UIManager.instance.IsActiveUI()) lockPlayerControls = true;
            if (!lockZoom) HandleCameraZoom();
            if (!lockPosition) HandleCameraPosition();
        }

        private void HandleCameraZoom()
        {
            if (Mathf.Abs(cam.orthographicSize - desiredZoom) <= zoomThreshold) return;
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, desiredZoom, zoomSpeed);
        }

        private void HandleCameraPosition()
        {
            float _distance = Vector2.Distance((Vector2)cam.transform.position, desiredPosition);
            Vector2 _desiredPosViewport = (Vector2)cam.WorldToViewportPoint(desiredPosition);
            _desiredPosViewport = new Vector2(_desiredPosViewport.x * 2 - 1, _desiredPosViewport.y * 2 - 1);
            float _viewportDistanceMagnitude = Mathf.Sqrt(_desiredPosViewport.x * _desiredPosViewport.x + _desiredPosViewport.y * _desiredPosViewport.y);
            if (_viewportDistanceMagnitude < followThreshold) return;
            float _speed = Mathf.Lerp(minFollowSpeed, maxFollowSpeed, _viewportDistanceMagnitude);
            Vector2 _newPos = Vector2.MoveTowards((Vector2)cam.transform.position, desiredPosition, _distance * _speed);
            cam.transform.position = new Vector3(_newPos.x, _newPos.y, cam.transform.position.z);
        }

        private void CameraZoomIn()
        {
            if (lockPlayerControls) return;
            playerZoom -= zoomMagnitude;
            playerZoom = desiredZoom = Mathf.Clamp(playerZoom, minPlayerZoom, maxPlayerZoom);
        }
        private void CameraZoomOut()
        {
            if (lockPlayerControls) return;
            playerZoom += zoomMagnitude;
            playerZoom = desiredZoom = Mathf.Clamp(playerZoom, minPlayerZoom, maxPlayerZoom);
        }

        public void ReturnHome() => desiredPosition = defaultPosition;

        /// <summary>
        /// Determines whether the given position is within the camera's frame.
        /// </summary>
        public bool IsInFrame(Vector2 _pos)
        {
            Vector2 _viewportPos = cam.WorldToViewportPoint(_pos);

            if (_viewportPos.x < 0f || _viewportPos.x > 1f || _viewportPos.y < 0f || _viewportPos.y > 1f) return false;

            else return true;
        }

        /// <summary>
        /// Returns orthographic size needed to cover the given parameter's distance.
        /// </summary>
        public float ViewDistanceToCameraZoom(float _distance)
        {
            // orthographic size determines the radius of vertical view distance of the camera.
            // So we convert vertical to horizontal (based on the 16:9 aspect ratio) and half it
            // to return the orthographic size needed to cover a known vertical distance.
            return _distance *= 0.5625f * 0.5f;
        }

        public void SetDesiredPosition(Vector2 _pos) => desiredPosition = _pos;
        public void SetDesiredZoom(float _zoom) => desiredZoom = _zoom;
        public void SetToPlayerZoom() => desiredZoom = playerZoom;
    }
}
