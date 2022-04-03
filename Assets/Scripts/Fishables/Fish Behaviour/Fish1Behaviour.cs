using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.FishingMechanics;

namespace Fishing.Fishables.Fish
{
    [RequireComponent(typeof(Edible))]
    [RequireComponent(typeof(FoodSearch))]
    [RequireComponent(typeof(Fishable))]
    public class Fish1Behaviour : MonoBehaviour, IEdible
    {
        [Header("Movement")]
        [SerializeField] private int generateWanderPositionPasses;
        [SerializeField] private float wanderSpeed;
        [SerializeField] private float wanderDistance;
        [SerializeField] private float wanderDistanceVariation;

        [SerializeField] private float maxHomeDistance;
        [SerializeField] private float maxHomeDistanceVariation;

        [SerializeField] private float distanceThreshold;

        [SerializeField] private float chaseSpeed;
        [SerializeField] private float eatDistance;

        [Header("Hold")]
        [SerializeField] private float holdTime;

        private Vector3 targetPos;

        private WaitForSeconds holdTimer;
        private FoodSearch foodSearch;
        private SpawnZone spawn;

        private RodManager rodManager;

        private void Awake()
        {
            rodManager = RodManager.instance;
            foodSearch = GetComponent<FoodSearch>();
            spawn = transform.parent.GetComponent<SpawnZone>();
        }

        private void Start()
        {
            wanderDistance += Random.Range(-wanderDistanceVariation, wanderDistanceVariation);
            maxHomeDistance += Random.Range(-maxHomeDistanceVariation, maxHomeDistanceVariation);
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
                if (Vector2.Distance(transform.position, targetPos) <= distanceThreshold)
                {
                    return;
                }
                else
                {
                    transform.position = Vector2.MoveTowards(transform.position, targetPos, wanderSpeed * Time.deltaTime);
                }
            }
            else
            {
                targetPos = foodSearch.desiredFood.transform.position;
                if (Vector2.Distance(transform.position, targetPos) > eatDistance)
                {
                    transform.position = Vector2.MoveTowards(transform.position, targetPos, chaseSpeed * Time.deltaTime);
                }
                else
                {
                    Eat();
                }
            }
            FlipTowardsTarget();
        }

        private IEnumerator Co_SetWanderPoint()
        {
            while (true)
            {
                if (Vector2.Distance(transform.position, transform.parent.position) >= maxHomeDistance)
                {
                    targetPos = transform.parent.position;
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
                        if (_distanceFromHome > maxHomeDistance) continue;
                        targetPos = new Vector2(transform.position.x + _rand.x, transform.position.y + _rand.y);
                        break;
                    }

                }
                yield return holdTimer;
            }
        }

        private void Eat()
        {
            if (foodSearch.desiredFood.GetComponent<HookBehaviour>())
            {
                foodSearch.desiredFood.GetComponent<HookBehaviour>().SetHook(GetComponent<Fishable>());
                return;
            }

            if (foodSearch.desiredFood.GetComponent<Fishable>().isHooked)
            {
                GetComponent<AudioSource>().Play();
                foodSearch.desiredFood.GetComponent<IEdible>().Despawn();
                rodManager.equippedRod.GetHook().hookedObject = null;
                rodManager.equippedRod.GetHook().SetHook(GetComponent<Fishable>());
                return;
            }

            GetComponent<AudioSource>().Play();
            foodSearch.desiredFood.GetComponent<IEdible>().Despawn();
            foodSearch.desiredFood = null;
        }

        private void FlipTowardsTarget()
        {
            if (targetPos.x < transform.position.x)
            {
                transform.rotation = Quaternion.identity;
            }
            else if (targetPos.x > transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (!GetComponent<Fishable>().isHooked)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(transform.position, wanderDistance);
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.parent.position, maxHomeDistance);
                if (foodSearch.desiredFood == null)
                {
                    Debug.DrawRay(transform.position, targetPos - transform.position, Color.green);
                }
            }
        }

        public void Despawn()
        {
            spawn.spawnList.Remove(gameObject);
            FoodSearchManager.instance.RemoveFish(GetComponent<FoodSearch>());
            FoodSearchManager.instance.RemoveFood(GetComponent<Edible>());
            DestroyImmediate(gameObject);
        }
    }
}
