using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.IO;

namespace Fishing.PlayerCamera
{
    public class CameraBehaviour : MonoBehaviour
    {
        [SerializeField] private float followSpeed;
        [SerializeField] private float followThreshold;

        [SerializeField] private float zoomMagnitude;
        [SerializeField] private float maxZoom;
        [SerializeField] private float minZoom;

        public static CameraBehaviour instance;
        public Camera cam;

        private CameraBehaviour() => instance = this;

        private void Awake()
        {
            cam = GetComponent<Camera>();
            InputManager.onZoomIn += CameraZoomIn;
            InputManager.onZoomOut += CameraZoomOut;
        }

        private void CameraZoomIn()
        {
            cam.orthographicSize -= zoomMagnitude;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
        private void CameraZoomOut()
        {
            cam.orthographicSize += zoomMagnitude;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }

        public bool IsInFrame(Vector2 _pos)
        {
            Vector2 _viewportPos = cam.WorldToViewportPoint(_pos);

            if (_viewportPos.x < 0f || _viewportPos.x > 1f || _viewportPos.y < 0f || _viewportPos.y > 1f) return false;

            else return true;
        }
    }
}
