using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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

        private Controls _controls;

        private CameraBehaviour() => instance = this;

        private void Awake()
        {
            cam = GetComponent<Camera>();
            _controls = new Controls();
            _controls.FishingLevelInputs.Enable();
            _controls.FishingLevelInputs.CameraZoom.performed += CameraZoom;
        }

        private void CameraZoom(InputAction.CallbackContext _context)
        {
            if (!_context.performed) return;

            float _zoomDelta = _context.ReadValue<float>();
            Debug.Log(_zoomDelta);
            cam.orthographicSize += _zoomDelta * zoomMagnitude;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }

        public bool IsInFrame(Vector2 _pos)
        {
            Vector2 _viewportPos = cam.WorldToViewportPoint(_pos);

            if (_viewportPos.x < 0f || _viewportPos.x > 1f || _viewportPos.y < 0f || _viewportPos.y > 1f) return false;

            else return true;
        }

        private void OnEnable()
        {
            _controls.FishingLevelInputs.CameraZoom.performed += CameraZoom;
        }

        private void OnDisable()
        {
            _controls.FishingLevelInputs.CameraZoom.performed -= CameraZoom;
        }
    }
}
