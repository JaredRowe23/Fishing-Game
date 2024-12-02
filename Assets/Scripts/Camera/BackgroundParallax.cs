using System.Collections.Generic;
using UnityEngine;

namespace Fishing.PlayerCamera {
    public class BackgroundParallax : MonoBehaviour {
        [SerializeField, Tooltip("Elements making up the layers of the parallax effect.")] private List<ParallaxingElement> _parallaxingElements;
        private Vector3 _parallaxOrigin;

        private Camera _camera;

        private void Awake() {
            _camera = GetComponent<Camera>();
        }

        private void Start() {
            _parallaxOrigin = _camera.transform.position;
            for (int i = 0; i < _parallaxingElements.Count; i++) {
                if (_parallaxingElements[i].ElementTransform == null) {
                    Debug.LogError("Parallaxing Element Transform not set", this);
                }
                _parallaxingElements[i].OriginalPosition = _parallaxingElements[i].ElementTransform.position;
            }
        }

        private void FixedUpdate() {
            AdjustParallaxPositions();
        }

        private void AdjustParallaxPositions() {
            for (int i = 0; i < _parallaxingElements.Count; i++) {
                ParallaxingElement element = _parallaxingElements[i];
                Vector2 cameraDeltaPosition = _camera.transform.position - _parallaxOrigin;
                element.ElementTransform.position = element.OriginalPosition + cameraDeltaPosition * element.ParallaxMultiplier;
            }
        }
    }
}
