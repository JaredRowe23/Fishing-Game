using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.Util;
using System.Linq;

namespace Fishing.Fishables.Fish
{
    [RequireComponent(typeof(Fishable))]
    [RequireComponent(typeof(FoodSearch))]
    public class FishMovement : MonoBehaviour
    {
        public GameObject activePredator;

        public Vector2 targetPos;
        public float swimSpeed;
        public float rotationSpeed;
        public float obstacleAvoidanceDistance;

        public float targetPosDirWeight = 1f;
        public float obstacleAvoidanceDirWeight = 1f;

        [SerializeField] private float baseMaxHomeDistance;
        [SerializeField] private float maxHomeDistanceVariation;

        [Range(-1, 1)]
        public float targetPosDir = 0;
        [Range(-1, 1)]
        public float rotationDir = 0;

        private FoodSearch foodSearch;
        private SpawnZone spawn;
        private PolygonCollider2D[] floorColliders;
        private float maxHomeDistance;

        private void Awake()
        {
            foodSearch = GetComponent<FoodSearch>();
            spawn = transform.parent.GetComponent<SpawnZone>();
            floorColliders = GameObject.Find("Grid").GetComponentsInChildren<PolygonCollider2D>();
        }

        private void Start()
        {
            maxHomeDistance = baseMaxHomeDistance + Random.Range(-maxHomeDistanceVariation, maxHomeDistanceVariation);
        }

        private void Update()
        {
            if (GetComponent<Fishable>().isHooked) return;


            Vector2 _surfacingCheck = transform.position - (transform.right * obstacleAvoidanceDistance);
            ClosestPointInfo _closestPointInfo = Utilities.ClosestPointFromColliders(transform.position, floorColliders);
            float _distToFloor = Vector2.Distance(transform.position, _closestPointInfo.position);
            float _trueRotation = (360 - transform.rotation.eulerAngles.z + 270) % 360;

            if (_surfacingCheck.y >= 0)
            {
                if (_trueRotation <= 180 && _trueRotation > 0)
                {
                    rotationDir = obstacleAvoidanceDirWeight;
                }
                else
                {
                    rotationDir = -obstacleAvoidanceDirWeight;
                }
            }

            else if (_distToFloor < obstacleAvoidanceDistance)
            {
                float _rotationToFloor = Vector2.Angle(Vector2.up, (Vector2)transform.position - _closestPointInfo.position);
                if (_rotationToFloor - _trueRotation > 0)
                {
                    rotationDir = obstacleAvoidanceDirWeight;
                }
                else
                {
                    rotationDir = -obstacleAvoidanceDirWeight;
                }
            }

            else if (activePredator != null)
            {
                float _rotationToPredator = Vector2.Angle(Vector2.up, (Vector2)transform.position - (Vector2)activePredator.transform.position);
                if (_rotationToPredator - _trueRotation > 0)
                {
                    rotationDir = obstacleAvoidanceDirWeight;
                }
                else
                {
                    rotationDir = -obstacleAvoidanceDirWeight;
                }
            }

            else if (Vector2.Distance(transform.position, spawn.transform.position) >= maxHomeDistance)
            {
                targetPos = spawn.transform.position;
                TurnToTarget();
            }

            else if (foodSearch.desiredFood != null)
            {
                targetPos = foodSearch.desiredFood.transform.position;
                TurnToTarget();
            } 

            else GetComponent<IMovement>().Movement();

            Move();
            FlipSprite();

            if (!activePredator) return;
            if (!activePredator.GetComponent<FoodSearch>()) return;
            if (activePredator.GetComponent<FoodSearch>().desiredFood == null) activePredator = null;
        }


        private void Move()
        {
            transform.Rotate(0f, 0f, -rotationSpeed * rotationDir * Time.deltaTime);
            transform.position = Vector2.MoveTowards(transform.position, -transform.right + transform.position, swimSpeed * Time.deltaTime);
        }

        public void TurnToTarget()
        {
            float _calculatedDir;
            float _trueRotation = (360 - transform.rotation.eulerAngles.z + 270) % 360;

            float _angleToTargetPos = (360 - SignedToUnsignedAngle(Vector2.SignedAngle(-Vector2.up, (Vector2)transform.position - targetPos)));
            float _targetPosAngleDelta = Mathf.DeltaAngle(_trueRotation, _angleToTargetPos);
            targetPosDir = _targetPosAngleDelta < 180 && _targetPosAngleDelta > 0 ? 1 : -1;

            _calculatedDir = targetPosDir * targetPosDirWeight;

            rotationDir = Mathf.Clamp(_calculatedDir, -1, 1);
        }

        private float SignedToUnsignedAngle(float _angle)
        {
            if (_angle < 0) _angle += 360f;
            return _angle % 360;
        }

        private void FlipSprite()
        {
            if ((360 - transform.rotation.eulerAngles.z + 270) % 360 < 180)
            {
                foreach (Transform child in transform)
                {
                    if (child.GetComponent<SpriteRenderer>() != null && child.gameObject.layer != LayerMask.NameToLayer("Minimap"))
                    {
                        child.GetComponent<SpriteRenderer>().flipY = true;
                    }
                }
            }
            else
            {
                foreach (Transform child in transform)
                {
                    if (child.GetComponent<SpriteRenderer>() != null && child.gameObject.layer != LayerMask.NameToLayer("Minimap"))
                    {
                        child.GetComponent<SpriteRenderer>().flipY = false;
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (!GetComponent<Fishable>().isHooked)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(spawn.transform.position, maxHomeDistance);
                Gizmos.color = new Color(Color.cyan.r, Color.cyan.g, Color.cyan.b, 0.25f);
                Gizmos.DrawWireSphere(spawn.transform.position, baseMaxHomeDistance + maxHomeDistanceVariation);
                Gizmos.DrawWireSphere(spawn.transform.position, baseMaxHomeDistance - maxHomeDistanceVariation);
            }
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(targetPos, 1);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, obstacleAvoidanceDistance);
        }

        public float GetMaxHomeDistance() => maxHomeDistance;
    }
}
