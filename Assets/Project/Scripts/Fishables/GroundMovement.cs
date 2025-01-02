using Fishing.Util.Collision;
using UnityEngine;

namespace Fishing.Fishables.Fish {
    public class GroundMovement : MonoBehaviour {
        [SerializeField, Min(0), Tooltip("Speed in m/s this creature will move at.")] private float moveSpeed;
        [SerializeField, Min(0), Tooltip("Height in meters off the ground the creature should maintain in order to not clip through. Proportional to its scale.")] private float groundOffset;
        [SerializeField, Min(0), Tooltip("Distance this creature must be within to an obstacle (such as the water's surface) for it to begin avoiding.")] private float obstacleAvoidanceDistance;
        [SerializeField, Min(0), Tooltip("The amount of seconds this creature determines which direction to move in.")] private float directionChangeTime;
        [SerializeField, Min(0), Tooltip("The maximum distance this creature can reach before it's forced to move back toward its spawner.")] private float maxRangeFromHome;

        private int moveDirection;
        private float directionChangeCount;
        private PolygonCollider2D[] floorColliders;
        private Vector2 closestFloorPoint;
        private Vector2 spawnerNearestFloorPosition;

        [SerializeField] private bool _drawMoveDirectionGizmos = false;
        [SerializeField] private Color _moveDirectionGizmosColor = Color.yellow;
        [SerializeField] private bool _drawHomeGroundPositionGizmos = false;
        [SerializeField] private Color _homeGroundPositionGizmosColor = Color.green;
        [SerializeField] private bool _drawClosestFloorPointGizmos = false;
        [SerializeField] private Color _closestFloorPointGizmosColor = Color.red;

        private void Awake() {
            floorColliders = GameObject.FindGameObjectWithTag("Fishing Level Terrain").GetComponentsInChildren<PolygonCollider2D>(); // TODO: Change this to a static instance of a FishingLevelTerrain script
        }

        private void Start() {
            moveDirection = 1;
            directionChangeCount = directionChangeTime;

            SurfacePositionInfo surfacePositionInfo = new SurfacePositionInfo(transform.parent.position, floorColliders);
            spawnerNearestFloorPosition = surfacePositionInfo.SurfacePosition;
        }

        void FixedUpdate() {
            if (GetComponent<Fishable>().IsHooked) {
                return;
            }

            DetermineMovementDirection();
            Move();
        }

        private void DetermineMovementDirection() {
            directionChangeCount -= Time.fixedDeltaTime;
            if (Vector2.Distance(transform.position, spawnerNearestFloorPosition) > maxRangeFromHome) {
                moveDirection = transform.parent.position.x - transform.position.x >= 0 ? 1 : -1;
            }
            else if (directionChangeCount <= 0) {
                moveDirection = Random.Range(0, 2) == 1 ? 1 : -1;
                directionChangeCount = directionChangeTime;
            }
        }

        private void Move() {
            transform.Translate(transform.right * moveSpeed * moveDirection * Time.fixedDeltaTime);

            SurfacePositionInfo surfacePositionInfo = new SurfacePositionInfo(transform.position, floorColliders);
            closestFloorPoint = surfacePositionInfo.SurfacePosition;
            Vector2 directionToFloorPoint = (closestFloorPoint - (Vector2)transform.position).normalized;
            if (surfacePositionInfo.PositionInsideTerrain) {
                transform.position = closestFloorPoint + directionToFloorPoint * groundOffset * transform.localScale.y;
            }
            else {
                transform.position = closestFloorPoint + directionToFloorPoint * -groundOffset * transform.localScale.y;
            }

            transform.rotation = Quaternion.Euler(surfacePositionInfo.RotationFromFloor);
        }

        private void OnDrawGizmosSelected() {
            if (_drawHomeGroundPositionGizmos) {
                DrawHomeGroundPositionGizmos();
            }
            if (_drawClosestFloorPointGizmos) {
                DrawClosestFloorPointGizmos();
            }
            if (_drawMoveDirectionGizmos) {
                DrawMoveDirectionGizmos();
            }
        }

        private void DrawHomeGroundPositionGizmos() {
            Gizmos.color = _homeGroundPositionGizmosColor;
            Gizmos.DrawLine(transform.position, spawnerNearestFloorPosition);
        }
        private void DrawClosestFloorPointGizmos() {
            Gizmos.color = _closestFloorPointGizmosColor;
            Gizmos.DrawLine(transform.position, closestFloorPoint);
        }

        private void DrawMoveDirectionGizmos() {
            Gizmos.color = _moveDirectionGizmosColor;
            Gizmos.DrawRay(transform.position, transform.right * moveDirection);
        }
    }
}
