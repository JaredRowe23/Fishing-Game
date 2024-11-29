using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.FishingMechanics;
using Fishing.Util;

namespace Fishing.Fishables.Fish
{
    [RequireComponent(typeof(FishMovement))]
    public class Wander : MonoBehaviour, IMovement, IEdible
    {
        [Header("Movement")]
        [SerializeField] private int generateWanderPositionPasses;
        [SerializeField] private float wanderSpeed;

        [SerializeField] private float distanceThreshold;

        [Header("Hold")]
        [SerializeField] private float holdTime;
        private float holdCount;

        private FishMovement movement;
        private PolygonCollider2D[] floorColliders;

        private void Awake()
        {
            movement = GetComponent<FishMovement>();
            floorColliders = GameObject.Find("Grid").GetComponentsInChildren<PolygonCollider2D>();
        }

        private void Start()
        {
            GenerateWanderPosition();
            holdCount = holdTime;
        }

        public void Movement()
        {
            if (Vector2.Distance(transform.position, movement.TargetPos) <= distanceThreshold)
            {
                GenerateWanderPosition();
                return;
            }
            movement.CalculateTurnDirection(movement.TargetPos);

            holdCount -= Time.deltaTime;
            if (holdCount > 0) return;

            GenerateWanderPosition();
        }

        private void GenerateWanderPosition()
        {
            int i = 0;
            while (true)
            {
                if (i >= generateWanderPositionPasses)
                {
                    movement.TargetPos = transform.parent.position;
                    break;
                }

                Vector2 _rand = Random.insideUnitCircle * movement.MaxHomeDistance;
                bool _aboveWater = _rand.y + transform.position.y >= 0f;
                float _distanceFromHome = Vector2.Distance(new Vector2(_rand.x + transform.position.x, _rand.y + transform.position.y), transform.parent.position);
                i++;

                if (_aboveWater) continue;
                if (_distanceFromHome > movement.MaxHomeDistance) continue;
                movement.TargetPos = (Vector2)transform.position + _rand;
                break;
            }

            ClosestPointInfo _closestPointInfo = Utilities.ClosestPointFromColliders(movement.TargetPos, floorColliders);
            if (_closestPointInfo.collider.OverlapPoint(movement.TargetPos)) movement.TargetPos = _closestPointInfo.collider.ClosestPoint(transform.position);

            holdCount = holdTime;
        }

        public void Despawn()
        {
            BaitManager.instance.RemoveFish(GetComponent<FoodSearch>());
            GetComponent<Edible>().Despawn();
        }
    }
}
