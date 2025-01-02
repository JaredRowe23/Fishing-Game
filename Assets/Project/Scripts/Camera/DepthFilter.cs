using Fishing.Util;
using System.ComponentModel;
using UnityEngine;

namespace Fishing.PlayerCamera {
    public class DepthFilter : MonoBehaviour {
        [SerializeField, Range(0, 1f), Tooltip("The maximum alpha value the filter will be set to when it reaches the set maximum depth.")] private float _maxAlpha;
        [SerializeField, Tooltip("The depth the filter will begin to show itself at when moving down, or conversely when it's fully hidden when moving up.")] private float _minDepth;
        [SerializeField, Tooltip("The depth the filter will be at maximum transparency.")] private float _maxDepth;
        private SpriteRenderer _sprite;

        [Header("Gizmos")]
        [SerializeField, Tooltip("The color of the line that represents the minimum depth.")] private Color _minDepthGizmoColor;
        [SerializeField, Tooltip("The color of the line that represents the maximum depth.")] private Color _maxDepthGizmoColor;

        private void OnValidate() {
            if (_minDepth > _maxDepth) {
                _minDepth = _maxDepth;
            }
        }

        private void Start() {
            _sprite = GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate() {
            AdjustTransparency();
        }

        private void AdjustTransparency() {
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
