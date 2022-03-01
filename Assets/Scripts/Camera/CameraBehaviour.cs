using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing
{
    public class CameraBehaviour : MonoBehaviour
    {
        public HookControl hook;

        [SerializeField] private float followSpeed;
        [SerializeField] private float followThreshold;

        [SerializeField] private float zoomMagnitude;
        [SerializeField] private float maxZoom;
        [SerializeField] private float minZoom;

        public static CameraBehaviour instance;
        public Camera cam;

        private void Awake()
        {
            instance = this;
            cam = GetComponent<Camera>();
        }

        void Update()
        {
            //float dist = Vector3.Distance(transform.position, hook.transform.position);
            //if (dist > followThreshold)
            //{
            //    Vector3 targetPos = new Vector3(hook.transform.position.x, hook.transform.position.y, transform.position.z);
            //    transform.position = Vector3.MoveTowards(transform.position, targetPos, Mathf.Pow(followSpeed * Time.deltaTime * dist, 2));
            //}

            transform.Translate(0f, 0f, Input.GetAxis("Mouse ScrollWheel") * zoomMagnitude);

            if (Mathf.Abs(transform.position.z) > maxZoom) transform.position = new Vector3(transform.position.x, transform.position.y, -maxZoom);

            else if (Mathf.Abs(transform.position.z) < minZoom) transform.position = new Vector3(transform.position.x, transform.position.y, -minZoom);

        }

        public bool IsInFrame(Vector3 pos)
        {
            Vector3 viewportPos = cam.WorldToViewportPoint(pos);

            if (viewportPos.x < 0f || viewportPos.x > 1f || viewportPos.y < 0f || viewportPos.y > 1f) return false;

            else return true;
        }
    }
}
