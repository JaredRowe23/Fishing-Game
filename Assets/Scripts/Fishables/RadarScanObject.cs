using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing
{
    public class RadarScanObject : MonoBehaviour
    {
        [SerializeField] private float scannedScale = 25;
        [SerializeField] private float lifeTime = 5;
        private bool isScanned;
        private Transform parent;

        private void Awake()
        {
            parent = transform.parent;
            isScanned = false;

            transform.parent = null;
            transform.localScale = Vector3.zero;
            transform.parent = parent;
        }

        private void Update()
        {
            if (!isScanned) return;
            transform.parent = null;
            transform.localScale -= Vector3.one * Time.deltaTime * lifeTime;
            if (transform.localScale.x <= 0)
            {
                transform.localScale = Vector3.zero;
                isScanned = false;
            }
            transform.parent = parent;
        }

        public void Scan()
        {
            transform.parent = null;
            transform.localScale = Vector3.one * scannedScale;
            transform.parent = parent;
            isScanned = true;
        }
    }
}
