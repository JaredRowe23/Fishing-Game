using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.IO;
using Fishing.UI;

namespace Fishing.PlayerCamera
{
    public class CameraBehaviour : MonoBehaviour
    {
        [Header("Position And Tracking")]
        [SerializeField] private Vector2 defaultPosition;

        [Range(0f, 1.0f)]
        [SerializeField] private float minFollowSpeed = 0.1f;
        [Range(0f, 1.0f)]
        [SerializeField] private float maxFollowSpeed = 1f;
        [Range(0f, 1.0f)]
        [SerializeField] private float followThreshold = 0.1f;

        [Header("Zoom")]
        [Range(0f, 1.0f)]
        [SerializeField] private float zoomSpeed = 0.5f;
        [SerializeField] private float zoomThreshold = 0.1f;

        [SerializeField] private float zoomMagnitude = 1;
        [SerializeField] private float minPlayerZoom = 1;
        [SerializeField] private float maxPlayerZoom = 20;

        private bool lockZoom = false;
        private bool lockPosition = false;
        private bool lockPlayerControls = false;

        private Vector2 desiredPosition;
        private float desiredZoom;

        private float playerZoom;
        private float tempZoom;

        public static CameraBehaviour instance;
        [HideInInspector]
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
            playerZoom = desiredZoom = tempZoom = cam.orthographicSize;
            desiredPosition = defaultPosition = cam.transform.position;
        }

        private void Update()
        {
            if (UIManager.instance.IsActiveUI()) lockPlayerControls = true;
            desiredZoom = lockPlayerControls ? tempZoom : playerZoom;
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
            float _viewportSqrDistance = GetViewportSqrDistanceFromCenter(desiredPosition);
            if (_viewportSqrDistance < followThreshold * followThreshold) return;

            float _speed = Mathf.Lerp(minFollowSpeed, maxFollowSpeed, _viewportSqrDistance);
            float _distance = Vector2.Distance(cam.transform.position, desiredPosition);
            Vector2 _newPos = Vector2.MoveTowards(cam.transform.position, desiredPosition, _distance * _speed);
            cam.transform.position = new Vector3(_newPos.x, _newPos.y, cam.transform.position.z);
        }

        private float GetViewportSqrDistanceFromCenter(Vector2 _pos)
        {
            Vector2 _posViewport = cam.WorldToViewportPoint(_pos);
            _posViewport = new Vector2(_posViewport.x * 2 - 1, _posViewport.y * 2 - 1);
            float _viewportDistanceMagnitude = _posViewport.x * _posViewport.x + _posViewport.y * _posViewport.y;

            return _viewportDistanceMagnitude;
        }

        private void CameraZoomIn()
        {
            if (lockPlayerControls) return;
            playerZoom = desiredZoom = Mathf.Clamp(playerZoom - zoomMagnitude, minPlayerZoom, maxPlayerZoom);
        }
        private void CameraZoomOut()
        {
            if (lockPlayerControls) return;
            playerZoom = desiredZoom = Mathf.Clamp(playerZoom + zoomMagnitude, minPlayerZoom, maxPlayerZoom);
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

        public void EnablePlayerControls() => lockPlayerControls = false;
        public void DisablePlayerControls() => lockPlayerControls = true;
        public void SetTempZoom(float _tempZoom) => tempZoom = _tempZoom;
    }
}
