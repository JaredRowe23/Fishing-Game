using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }

        void Update()
        {
            transform.Translate(0f, 0f, Input.GetAxis("Mouse ScrollWheel") * zoomMagnitude);

            if (Mathf.Abs(transform.position.z) > maxZoom) transform.position = new Vector3(transform.position.x, transform.position.y, -maxZoom);

            else if (Mathf.Abs(transform.position.z) < minZoom) transform.position = new Vector3(transform.position.x, transform.position.y, -minZoom);
        }

        public bool IsInFrame(Vector3 _pos)
        {
            Vector3 _viewportPos = cam.WorldToViewportPoint(_pos);

            if (_viewportPos.x < 0f || _viewportPos.x > 1f || _viewportPos.y < 0f || _viewportPos.y > 1f) return false;

            else return true;
        }
    }
}
