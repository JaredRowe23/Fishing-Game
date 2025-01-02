using Fishing.Util.Collision;
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
        private SpawnZone _spawner;

        private void Awake() {
            _movement = GetComponent<FishMovement>();
            _floorColliders = GameObject.FindGameObjectWithTag("Fishing Level Terrain").GetComponentsInChildren<PolygonCollider2D>(); // TODO: Replace with searching for a static instance of a FishingLevelTerrain script
            _spawner = transform.parent.GetComponent<SpawnZone>();
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
                Vector2 rand = Random.insideUnitCircle * _movement.MaxHomeDistance + (Vector2)_spawner.transform.position;

                bool aboveWater = rand.y + transform.position.y >= 0f;
                if (aboveWater) {
                    rand.y = 0f;
                }

                _movement.TargetPos = (Vector2)transform.position + rand;

                SurfacePositionInfo surfacePositionInfo = new SurfacePositionInfo(_movement.TargetPos, _floorColliders);
                if (surfacePositionInfo.PositionInsideTerrain) {
                    _movement.TargetPos = surfacePositionInfo.SurfacePosition;
                }

                yield return new WaitForSeconds(_wanderPositionTimeout);
            }
        }
    }
}
