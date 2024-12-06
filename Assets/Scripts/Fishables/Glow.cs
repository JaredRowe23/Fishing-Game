using Fishing.Util;
using UnityEngine;

namespace Fishing.Fishables {
    public class Glow : MonoBehaviour {
        [SerializeField, Tooltip("The SpriteRenderer used for normal rendering of the fish.")] private SpriteRenderer _diffuseSprite;
        [SerializeField, Tooltip("The SpriteRenderer used for creating the fish glow")] private SpriteRenderer _glowSprite;
        [SerializeField, Tooltip("Distance the fish begins to glow at")] private float _minGlowDistance;
        [SerializeField, Tooltip("Distance for the fish to reach its maximum glow amount")] private float _maxGlowDistance;

        private RodManager rodManager;

        private void OnValidate() {
            if (_maxGlowDistance > _minGlowDistance) {
                _maxGlowDistance = _minGlowDistance;
            }
        }

        private void Start() {
            rodManager = RodManager.instance;
        }

        void FixedUpdate() {
            _glowSprite.flipY = _diffuseSprite.flipY;

            float _distance = Vector2.Distance(rodManager.equippedRod.GetHook().transform.position, transform.position);
            if (_distance >= _minGlowDistance) { 
                _glowSprite.color = Utilities.SetTransparency(_glowSprite.color, 0);
            }
            else if (_distance <= _maxGlowDistance) {
                _glowSprite.color = Utilities.SetTransparency(_glowSprite.color, 1);
            }
            else {
                _glowSprite.color = Utilities.SetTransparency(_glowSprite.color, Mathf.InverseLerp(_minGlowDistance, _maxGlowDistance, _distance));
            }
        }
    }
}
