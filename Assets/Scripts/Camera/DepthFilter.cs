using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.Util;

namespace Fishing.PlayerCamera {
    public class DepthFilter : MonoBehaviour {
        [SerializeField, Range(0, 1f)] private float _maxAlpha;
        [SerializeField] private float _minDepth;
        [SerializeField] private float _maxDepth;
        private SpriteRenderer _sprite;

        [Header("Gizmos")]
        [SerializeField] private Color _minDepthGizmoColor;
        [SerializeField] private Color _maxDepthGizmoColor;

        private void OnValidate() {
            if (_minDepth > _maxDepth) {
                _minDepth = _maxDepth;
            }
        }

        private void Start() {
            _sprite = GetComponent<SpriteRenderer>();
        }

        void Update() {
            if (-transform.position.y < _minDepth || _sprite.color.a == 0f) { 
                return; 
            }
            _sprite.color = Utilities.SetTransparency(_sprite.color, Mathf.InverseLerp(_minDepth, _maxDepth, -transform.position.y) * _maxAlpha);
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = _minDepthGizmoColor;
            Gizmos.DrawLine(new Vector3(-1000, -_minDepth, 0), new Vector3(1000, -_minDepth, 0));
            Gizmos.color = _maxDepthGizmoColor;
            Gizmos.DrawLine(new Vector3(-1000, -_maxDepth, 0), new Vector3(1000, -_maxDepth, 0));
        }
    }
}
