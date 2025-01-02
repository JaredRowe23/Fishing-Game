using Fishing.Util;
using UnityEngine;

namespace Fishing {
    public class RadarScanObject : MonoBehaviour {
        [SerializeField, Min(0), Tooltip("Local scale to set the radar scan sprite to.")] private float _scannedScale = 25f;
        [SerializeField, Min(0), Tooltip("Time it takes for the radar scan sprite to scale down to zero.")] private float _lifeTime = 5f;
        private bool _isScanned = false;

        private void FixedUpdate() {
            if (!_isScanned) {
                return;
            }

            transform.localScale -= (Vector3)Utilities.SetGlobalScale(transform, Time.fixedDeltaTime * _lifeTime);
            if (transform.localScale.x <= 0) {
                _isScanned = false;
            }
        }

        public void Scan() {
            transform.localScale = Utilities.SetGlobalScale(transform, _scannedScale);
            _isScanned = true;
        }
    }
}
