using System;
using UnityEngine;

namespace Fishing.PlayerCamera {
    [Serializable]
    public class ParallaxingElement {
        [SerializeField, Tooltip("The object to be affected by parallax.")] private Transform _elementTransform;
        public Transform ElementTransform { get => _elementTransform; private set { } }
        [SerializeField, Range(0f, 1f), Tooltip("Strength of parallax effect on this object. 0 = follow camera, 1 = stays in place. Increasing gives the impression of being further away.")] private float _parallaxMultiplier = 1f;
        public float ParallaxMultiplier { get => _parallaxMultiplier; private set { } }
        private Vector2 _originalPosition;
        public Vector2 OriginalPosition { get => _originalPosition; set => _originalPosition = value; }
    }
}
