using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.PlayerCamera {
    public class BackgroundParallax : MonoBehaviour {
        [SerializeField] private List<Transform> _parallaxTransforms;
        [SerializeField] private List<int> _layers;
        [SerializeField] private float _parallaxStrength;
        [SerializeField] private Vector3 _parallaxOrigin;

        private List<Vector3> _originalPositions;

        private Camera _camera;

        void Awake() {
            _camera = CameraBehaviour.Instance.Camera;
            _parallaxOrigin = _camera.transform.position;
        }

        void Start() {
            _originalPositions = new List<Vector3>();
            for (int i = 0; i < _parallaxTransforms.Count; i++) {
                _originalPositions.Add(_parallaxTransforms[i].position);
            }
        }

        void Update() {
            for (int i = 0; i < _parallaxTransforms.Count; i++) {
                _parallaxTransforms[i].position = _originalPositions[i] + (_camera.transform.position - _parallaxOrigin) * _parallaxStrength * _layers[i];
            }
        }
    }
}
