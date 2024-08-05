using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.Util;

namespace Fishing
{
    public class RadarScanObject : MonoBehaviour
    {
        [SerializeField] private float scannedScale = 25;
        [SerializeField] private float lifeTime = 5;
        private bool isScanned;

        private void Awake()
        {
            isScanned = false;
        }

        private void Update()
        {
            if (!isScanned) return;

            transform.localScale -= (Vector3)Utilities.SetGlobalScale(transform, Time.deltaTime * lifeTime);
            if (transform.localScale.x <= 0) isScanned = false;
        }

        public void Scan()
        {
            transform.localScale = Utilities.SetGlobalScale(transform, scannedScale);
            isScanned = true;
        }
    }
}
