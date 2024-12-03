using Fishing.FishingMechanics;
using Fishing.Util;
using System.Collections;
using UnityEngine;

namespace Fishing.Fishables.Fish {
    [RequireComponent(typeof(FishMovement))]
    public class Wander : MonoBehaviour, IMovement {
        [SerializeField, Min(0), Tooltip("Speed that the fish moves towards it's wander target position.")] private float _wanderSpeed;
        [SerializeField, Min(0), Tooltip("Minimum distance this must be within it's wander target for it to generate a new wander target.")] private float _distanceThreshold = 0.1f;
        [SerializeField, Min(0), Tooltip("The amount of seconds that must pass without this fish meeting the distance threshold to the wander target that another target is generated.")] private float _wanderPositionTimeout = 10f;

        private FishMovement _movement;
        private PolygonCollider2D[] _floorColliders;

        private void Awake() {
            _movement = GetComponent<FishMovement>();
            _floorColliders = GameObject.Find("Grid").GetComponentsInChildren<PolygonCollider2D>();
        }

        private void Start() {
            StartCoroutine(Co_GenerateWanderPosition());
        }

        public void Movement() {
            if (Vector2.Distance(transform.position, _movement.TargetPos) <= _distanceThreshold) {
                StopCoroutine(Co_GenerateWanderPosition());
                StartCoroutine(Co_GenerateWanderPosition());
            }
            _movement.CalculateTurnDirection(_movement.TargetPos);
        }

        private IEnumerator Co_GenerateWanderPosition() {
            while (true) {
                Vector2 _rand = Random.insideUnitCircle * _movement.MaxHomeDistance;

                bool _aboveWater = _rand.y + transform.position.y >= 0f;
                if (_aboveWater) {
                    _rand.y = 0f;
                }

                _movement.TargetPos = (Vector2)transform.position + _rand;

                ClosestPointInfo _closestPointInfo = Utilities.ClosestPointFromColliders(_movement.TargetPos, _floorColliders);
                if (_closestPointInfo.collider.OverlapPoint(_movement.TargetPos)) {
                    _movement.TargetPos = _closestPointInfo.collider.ClosestPoint(transform.position);
                }

                yield return new WaitForSeconds(_wanderPositionTimeout);
            }
        }
    }
}
