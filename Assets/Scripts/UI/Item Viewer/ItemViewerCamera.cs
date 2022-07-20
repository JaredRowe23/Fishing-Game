using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.UI
{
    public class ItemViewerCamera : MonoBehaviour
    {
        [SerializeField] private float rotateSpeed;
        [SerializeField] private float scrollMultiplier;
        [SerializeField] private float minimumOrthoSize;
        [SerializeField] private float maximumOrthoSize;
        [SerializeField] private float maximumZoomOutMultiplier;

        [SerializeField] private GameObject currentItem;
        private bool isDragging;

        private Camera cam;

        public static ItemViewerCamera instance;

        private ItemViewerCamera()
        {
            instance = this;
        }

        private void Awake()
        {
            cam = GetComponent<Camera>();
        }

        private void Update()
        {
            // enable mouse rotation when clicking and holding
            if (UIManager.instance.mouseOverUI == UIManager.instance.itemViewer.gameObject)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    isDragging = true;
                }

                // scrollwheel will move (zoom) the camera in and out
                // scaled off it's current distance and the item's scale
                float _scroll = Input.GetAxis("Mouse ScrollWheel");
                cam.orthographicSize -= _scroll * scrollMultiplier;
                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minimumOrthoSize, maximumOrthoSize);

                //Transform _parent = currentItem.transform.parent;
                //currentItem.transform.parent = null;

                //transform.Translate(0f, 0f, _scroll * scrollMultiplier * Mathf.Abs(transform.localPosition.z));

                //if (transform.localPosition.z > -currentItem.transform.localScale.z)
                //{
                //    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -currentItem.transform.localScale.z);
                //}
                //else if (transform.localPosition.z < -currentItem.transform.localScale.z * maximumZoomOutMultiplier)
                //{
                //    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -currentItem.transform.localScale.z * maximumZoomOutMultiplier);
                //}

                //currentItem.transform.parent = _parent;
            }

            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }

            // Rotate item off of mouse movement
            if (isDragging)
            {
                float x = Input.GetAxis("Mouse X") * Time.deltaTime * rotateSpeed;
                float y = Input.GetAxis("Mouse Y") * Time.deltaTime * rotateSpeed;
                currentItem.transform.Rotate(Vector3.down, x, Space.World);
                currentItem.transform.Rotate(Vector3.right, y, Space.World);
            }
        }

        // This is a runoff of the ItemInfoMenu's UpdateMenu function, which does most of the heavy lifting
        // Change all items to an "unrendered" layer to this camera and move our current item to the rendered layer
        // Also reset the rotation and set the location to fit into the camera
        public void UpdateCurrentItem(GameObject _item)
        {
            currentItem = _item;

            currentItem.layer = 8;
            foreach (Transform _child in currentItem.transform)
            {
                _child.gameObject.layer = 8;
            }

            currentItem.transform.rotation = Quaternion.identity;
            Transform _parent = currentItem.transform.parent;
            currentItem.transform.parent = null;

            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -currentItem.transform.localScale.z);

            currentItem.transform.parent = _parent;
        }
    }

}