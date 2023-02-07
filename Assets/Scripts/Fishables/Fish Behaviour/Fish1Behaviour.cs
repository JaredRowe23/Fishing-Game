using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.FishingMechanics;

namespace Fishing.Fishables.Fish
{
    [RequireComponent(typeof(Edible))]
    [RequireComponent(typeof(FoodSearch))]
    [RequireComponent(typeof(Fishable))]
    [RequireComponent(typeof(Fish))]
    public class Fish1Behaviour : MonoBehaviour, IEdible
    {
        [Header("Movement")]
        [SerializeField] private int generateWanderPositionPasses;
        [SerializeField] private float wanderSpeed;
        [SerializeField] private float wanderDistance;
        [SerializeField] private float wanderDistanceVariation;

        [SerializeField] private float distanceThreshold;

        [Header("Hold")]
        [SerializeField] private float holdTime;

        private WaitForSeconds holdTimer;
        private Fish fish;
        private FoodSearch foodSearch;
        private SpawnZone spawn;
        private PolygonCollider2D floorCol;

        private void Awake()
        {
            foodSearch = GetComponent<FoodSearch>();
            fish = GetComponent<Fish>();
            spawn = transform.parent.GetComponent<SpawnZone>();
            floorCol = FindObjectOfType<PolygonCollider2D>();
        }

        private void Start()
        {
            wanderDistance += Random.Range(-wanderDistanceVariation, wanderDistanceVariation);
            holdTimer = new WaitForSeconds(holdTime);
            StartCoroutine(Co_SetWanderPoint());
        }

        private void Update()
        {
            if (GetComponent<Fishable>().isHooked) return;

            MoveTowardsTarget();
        }

        private void MoveTowardsTarget()
        {
            if (!foodSearch.desiredFood)
            {
                if (Vector2.Distance(transform.position, fish.targetPos) <= distanceThreshold)
                {
                    return;
                }
                else
                {
                    transform.position = Vector2.MoveTowards(transform.position, fish.targetPos, wanderSpeed * Time.deltaTime);
                }
            }
            else
            {
                fish.targetPos = foodSearch.desiredFood.transform.position;
                if (Vector2.Distance(transform.position, fish.targetPos) > fish.eatDistance)
                {
                    transform.position = Vector2.MoveTowards(transform.position, fish.targetPos, fish.swimSpeed * Time.deltaTime);
                }
                else
                {
                    fish.Eat();
                }
            }
            fish.FaceTarget();
        }

        private IEnumerator Co_SetWanderPoint()
        {
            while (true)
            {
                if (Vector2.Distance(transform.position, transform.parent.position) >= fish.maxHomeDistance)
                {
                    fish.targetPos = transform.parent.position;
                }
                else
                {
                    Vector2 _rand = Random.insideUnitCircle * wanderDistance;
                    int i = 0;
                    while (true)
                    {
                        if (i >= generateWanderPositionPasses) break;

                        bool _aboveWater = _rand.y + transform.position.y >= 0f;
                        float _distanceFromHome = Vector2.Distance(new Vector2(_rand.x + transform.position.x, _rand.y + transform.position.y), transform.parent.position);
                        i++;

                        if (_aboveWater) continue;
                        if (_distanceFromHome > fish.maxHomeDistance) continue;
                        fish.targetPos = (Vector2)transform.position + _rand;
                        break;
                    }
                }

                if (floorCol.OverlapPoint(fish.targetPos)) 
                {
                    fish.targetPos = floorCol.ClosestPoint(transform.position);
                }

                yield return holdTimer;
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (!GetComponent<Fishable>().isHooked)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(transform.position, wanderDistance);
            }
            Gizmos.DrawSphere(fish.targetPos, 1);
        }

        public void Despawn()
        {
            spawn.spawnList.Remove(gameObject);
            FoodSearchManager.instance.RemoveFish(GetComponent<FoodSearch>());
            FoodSearchManager.instance.RemoveFood(GetComponent<Edible>());
            BaitManager.instance.RemoveFish(GetComponent<FoodSearch>());
            DestroyImmediate(gameObject);
        }
    }
}
