using Fishing.Fishables.Fish;
using Fishing.PlayerCamera;
using UnityEngine;

namespace Fishing.Fishables {
    public class OffScreenOptimizations : MonoBehaviour {
        [SerializeField, Min(0), Tooltip("Distance away from player camera for optimization to begin shutting down unnecessary gameplay systems for this object.")] private float _optimizationDistance;

        private FoodSearch _foodSearch;
        private FishMovement _fishMovement;
        private GroundMovement _groundMovement;
        private SinkingObject _sinkingObject;
        private IMovement _movement;
        private Hunger _hunger;
        private Growth _growth;

        private CameraBehaviour _camera;

        private void Awake() {
            _foodSearch = GetComponent<FoodSearch>();
            _fishMovement = GetComponent<FishMovement>();
            _groundMovement = GetComponent<GroundMovement>();
            _sinkingObject = GetComponent<SinkingObject>();
            _movement = GetComponent<IMovement>();
            _hunger = GetComponent<Hunger>();
            _growth = GetComponent<Growth>();
        }

        void Start() {
            _camera = CameraBehaviour.Instance;
        }

        private void FixedUpdate() {
            if (_camera.IsInFrame(transform.position)) {
                EndOptimizing();
                return;
            }
            float distance = Vector2.Distance(_camera.transform.position, transform.position);
            if (distance > _optimizationDistance) {
                StartOptimizing();
            }
            else {
                EndOptimizing();
            }
        }

        private void StartOptimizing()
        {
            if (_foodSearch) {
                _hunger.enabled = false;
                _growth.enabled = false;
            }

            if (_fishMovement) {
                _fishMovement.enabled = false;
                if (_fishMovement.ActivePredator != null) {
                    _fishMovement.ActivePredator.GetComponent<FoodSearch>().DesiredFood = null;
                    _fishMovement.ActivePredator = null;
                }
                if (_movement is MonoBehaviour mono) {
                    mono.enabled = false;
                }
            }
            else if (_groundMovement) {
                _groundMovement.enabled = false;
            }
            else if (_sinkingObject) {
                _sinkingObject.enabled = false;
            }
        }

        private void EndOptimizing() {
            if (_foodSearch) {
                _hunger.enabled = true;
                _growth.enabled = true;
            }

            if (_fishMovement) {
                _fishMovement.enabled = true;
                if (_movement is MonoBehaviour mono) {
                    mono.enabled = true;
                }
            }
            else if (_groundMovement) {
                _groundMovement.enabled = true;
            }
            else if (_sinkingObject) {
                _sinkingObject.enabled = true;
            }
        }
    }
}
