using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.FishingMechanics;

namespace Fishing.Fishables.Fish
{
    public class GroundMovement : MonoBehaviour, IEdible
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float groundOffset;
        [SerializeField] private float obstacleAvoidanceDistance;
        [SerializeField] private float directionChangeTime;
        [SerializeField] private float maxRangeFromHome;

        private int moveDirection;
        private float directionChangeCount;
        private PolygonCollider2D floorCol;

        private void Awake()
        {
            floorCol = FindObjectOfType<PolygonCollider2D>();
        }

        private void Start()
        {
            moveDirection = 1;
            directionChangeCount = directionChangeTime;
        }

        // Update is called once per frame
        void Update()
        {
            if (GetComponent<Fishable>().isHooked) return;

            directionChangeCount -= Time.deltaTime;
            if (Vector2.Distance(transform.position, floorCol.ClosestPoint(transform.parent.position)) > maxRangeFromHome)
            {
                moveDirection = Mathf.CeilToInt(Mathf.Clamp01(transform.parent.position.x - transform.position.x));
            }
            else if (directionChangeCount <= 0)
            {
                moveDirection = Random.Range(0, 2);
                directionChangeCount = directionChangeTime;
            }
            if (moveDirection == 0) moveDirection = -1;

            Vector3 _surfacingCheck = transform.position - (transform.right * obstacleAvoidanceDistance);
            float _rotationToFloor = Vector2.Angle(Vector2.up, (Vector2)transform.position - floorCol.ClosestPoint(transform.position));
            float _trueRotation = (360 - transform.rotation.eulerAngles.z) % 360;
            float _distToFloor = Vector2.Distance(transform.position, floorCol.ClosestPoint(transform.position));

            transform.Rotate(-Vector3.forward, _rotationToFloor - _trueRotation);
            transform.Translate(transform.right * moveSpeed * moveDirection * Time.deltaTime);
            transform.position = floorCol.ClosestPoint(transform.position) + (Vector2)(Vector3.Normalize((Vector2)transform.position - floorCol.ClosestPoint(transform.position)) * groundOffset);

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
        public void Despawn()
        {
            FoodSearchManager.instance.RemoveFish(GetComponent<FoodSearch>());
            BaitManager.instance.RemoveFish(GetComponent<FoodSearch>());
            GetComponent<Edible>().Despawn();
        }
    }
}