using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.FishingMechanics;

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
        private PolygonCollider2D floorCol;

        private void Awake()
        {
            movement = GetComponent<FishMovement>();
            floorCol = FindObjectOfType<PolygonCollider2D>();
        }

        private void Start()
        {
            GenerateWanderPosition();
            holdCount = holdTime;
        }

        public void Movement()
        {
            if (Vector2.Distance(transform.position, movement.targetPos) <= distanceThreshold)
            {
                GenerateWanderPosition();
                return;
            }
            movement.TurnToTarget();

            holdCount -= Time.deltaTime;
            if (holdCount > 0) return;

            GenerateWanderPosition();

            holdCount = holdTime;
        }

        private void GenerateWanderPosition()
        {
            Vector2 _rand = Random.insideUnitCircle * movement.GetMaxHomeDistance();
            int i = 0;
            while (true)
            {
                if (i >= generateWanderPositionPasses) break;

                bool _aboveWater = _rand.y + transform.position.y >= 0f;
                float _distanceFromHome = Vector2.Distance(new Vector2(_rand.x + transform.position.x, _rand.y + transform.position.y), transform.parent.position);
                i++;

                if (_aboveWater) continue;
                if (_distanceFromHome > movement.GetMaxHomeDistance()) continue;
                movement.targetPos = (Vector2)transform.position + _rand;
                break;
            }

            if (floorCol.OverlapPoint(movement.targetPos)) movement.targetPos = floorCol.ClosestPoint(transform.position);
        }

        public void Despawn()
        {
            FoodSearchManager.instance.RemoveFish(GetComponent<FoodSearch>());
            BaitManager.instance.RemoveFish(GetComponent<FoodSearch>());
            GetComponent<Edible>().Despawn();
        }
        private float SignedToUnsignedAngle(float _angle)
        {
            if (_angle < 0) _angle += 360f;
            return _angle % 360;
        }
    }
}
