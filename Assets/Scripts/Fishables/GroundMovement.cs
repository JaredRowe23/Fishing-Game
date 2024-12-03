using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.FishingMechanics;
using Fishing.Util;

namespace Fishing.Fishables.Fish
{
    public class GroundMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float groundOffset;
        [SerializeField] private float obstacleAvoidanceDistance;
        [SerializeField] private float directionChangeTime;
        [SerializeField] private float maxRangeFromHome;

        private int moveDirection;
        private float directionChangeCount;
        private PolygonCollider2D[] floorColliders;
        private Vector2 closestFloorPoint;
        private Vector2 spawnerNearestFloorPosition;

        private void Awake()
        {
            floorColliders = GameObject.Find("Grid").GetComponentsInChildren<PolygonCollider2D>();
        }

        private void Start()
        {
            moveDirection = 1;
            directionChangeCount = directionChangeTime;
            spawnerNearestFloorPosition = Utilities.ClosestPointFromColliders(transform.parent.position, floorColliders).position;
        }

        void Update()
        {
            if (GetComponent<Fishable>().IsHooked) return;

            DetermineMovementDirection();
            Move();

            //Vector3 _surfacingCheck = transform.position - (transform.right * obstacleAvoidanceDistance);
            //float _distToFloor = Vector2.Distance(transform.position, closestFloorPoint);

            //if (_surfacingCheck.y >= 0)
            //{
            //    if (_trueRotation <= 180 && _trueRotation > 0)
            //    {
            //        rotationDir = obstacleAvoidanceDirWeight;
            //    }
            //    else
            //    {
            //        rotationDir = -obstacleAvoidanceDirWeight;
            //    }
            //}

            //else if (_distToFloor < obstacleAvoidanceDistance)
            //{
            //    if (_rotationToFloor - _trueRotation > 0)
            //    {
            //        rotationDir = obstacleAvoidanceDirWeight;
            //    }
            //    else
            //    {
            //        rotationDir = -obstacleAvoidanceDirWeight;
            //    }
            //}
        }

        private void DetermineMovementDirection()
        {
            directionChangeCount -= Time.deltaTime;
            if (Vector2.Distance(transform.position, spawnerNearestFloorPosition) > maxRangeFromHome)
            {
                moveDirection = transform.parent.position.x - transform.position.x >= 0 ? 1 : -1;
            }
            else if (directionChangeCount <= 0)
            {
                moveDirection = Random.Range(0, 2) == 1 ? 1 : -1;
                directionChangeCount = directionChangeTime;
            }
        }

        private void Move()
        {
            closestFloorPoint = Utilities.ClosestPointFromColliders(transform.position, floorColliders).position;

            transform.Translate(transform.right * moveSpeed * moveDirection * Time.deltaTime);
            transform.position = closestFloorPoint + (Vector2)(Vector3.Normalize((Vector2)transform.position - closestFloorPoint) * groundOffset);

            float _angleFromFloor = Vector2.Angle(Vector2.up, (Vector2)transform.position - closestFloorPoint);
            transform.rotation = Quaternion.Euler(0, 0, _angleFromFloor);
        }
    }
}
