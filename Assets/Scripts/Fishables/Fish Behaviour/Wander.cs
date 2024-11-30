using Fishing.FishingMechanics;
using Fishing.Util;
using System.Collections;
using UnityEngine;

namespace Fishing.Fishables.Fish {
    [RequireComponent(typeof(FishMovement))]
    public class Wander : MonoBehaviour, IMovement, IEdible {
        [Header("Movement")]
        [SerializeField] private int _generateWanderPositionPasses;
        [SerializeField] private float _wanderSpeed;

        [SerializeField] private float _distanceThreshold;
        [SerializeField] private float _wanderPositionTimeout;

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
                int i = 0;
                while (true) {
                    if (i >= _generateWanderPositionPasses) {
                        _movement.TargetPos = transform.parent.position;
                        break;
                    }

                    Vector2 _rand = Random.insideUnitCircle * _movement.MaxHomeDistance;
                    bool _aboveWater = _rand.y + transform.position.y >= 0f;
                    float _distanceFromHome = Vector2.Distance(new Vector2(_rand.x + transform.position.x, _rand.y + transform.position.y), transform.parent.position);
                    i++;

                    if (_aboveWater) {
                        continue;
                    }
                    if (_distanceFromHome > _movement.MaxHomeDistance) {
                        continue;
                    }
                    _movement.TargetPos = (Vector2)transform.position + _rand;
                    break;
                }

                ClosestPointInfo _closestPointInfo = Utilities.ClosestPointFromColliders(_movement.TargetPos, _floorColliders);
                if (_closestPointInfo.collider.OverlapPoint(_movement.TargetPos)) {
                    _movement.TargetPos = _closestPointInfo.collider.ClosestPoint(transform.position);
                }

                yield return new WaitForSeconds(_wanderPositionTimeout);
            }
        }

        public void Despawn() {
            BaitManager.instance.RemoveFish(GetComponent<FoodSearch>());
            GetComponent<Edible>().Despawn();
        }
    }
}
