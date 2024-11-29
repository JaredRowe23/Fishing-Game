using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.Util;

namespace Fishing.PlayerCamera {
    public class DepthFilter : MonoBehaviour {
        [SerializeField] private float _maxAlpha;
        [SerializeField] private float _minDepth;
        [SerializeField] private float _maxDepth;
        private SpriteRenderer _sprite;

        private void Start() {
            _sprite = GetComponent<SpriteRenderer>();
        }

        void Update() {
            if (-transform.position.y < _minDepth) { 
                return; 
            }
            _sprite.color = Utilities.SetTransparency(_sprite.color, Mathf.InverseLerp(_minDepth, _maxDepth, -transform.position.y) * _maxAlpha);
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(new Vector3(-1000, -_minDepth, 0), new Vector3(1000, -_minDepth, 0));
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(-1000, -_maxDepth, 0), new Vector3(1000, -_maxDepth, 0));
        }
    }
}
