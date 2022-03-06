using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing
{
    public class Fish1Behaviour : MonoBehaviour, IEdible
    {
        [Header("Movement")]
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

        private void Awake()
        {
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
            if (GetComponent<FishableItem>().isHooked) return;

            MoveTowardsTarget();
        }

        private void MoveTowardsTarget()
        {
            if (!foodSearch.desiredFood)
            {
                if (Vector3.Distance(transform.position, targetPos) <= distanceThreshold)
                {
                    return;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, wanderSpeed * Time.deltaTime);
                }
            }
            else
            {
                targetPos = foodSearch.desiredFood.transform.position;
                if (Vector3.Distance(transform.position, targetPos) > eatDistance)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, chaseSpeed * Time.deltaTime);
                }
                else
                {
                    if (foodSearch.desiredFood.transform.parent)
                    {
                        if (foodSearch.desiredFood.transform.parent.GetComponent<SpawnZone>())
                        {
                            foodSearch.desiredFood.transform.parent.GetComponent<SpawnZone>().spawnList.Remove(foodSearch.desiredFood);
                        }
                    }
                    Eat();
                }
            }
            FlipTowardsTarget();
        }

        IEnumerator Co_SetWanderPoint()
        {
            while (true)
            {
                if (Vector3.Distance(transform.position, transform.parent.position) >= maxHomeDistance)
                {
                    targetPos = transform.parent.position;
                }
                else
                {
                    Vector2 rand = Random.insideUnitCircle * wanderDistance;
                    while (rand.y + transform.position.y >= 0f || Vector3.Distance(new Vector3(rand.x + transform.position.x, rand.y + transform.position.y, transform.position.z), transform.parent.position) >= maxHomeDistance)
                    {
                        rand = Random.insideUnitCircle * wanderDistance;
                    }
                    targetPos = new Vector3(transform.position.x + rand.x, transform.position.y + rand.y, 0f);
                }
                yield return holdTimer;
            }
        }

        private void Eat()
        {
            if (foodSearch.desiredFood.GetComponent<HookBehaviour>())
            {
                foodSearch.desiredFood.GetComponent<HookBehaviour>().SetHook(GetComponent<FishableItem>());
                return;
            }

            if (foodSearch.desiredFood.GetComponent<FishableItem>().isHooked)
            {
                GetComponent<AudioSource>().Play();
                foodSearch.desiredFood.GetComponent<IEdible>().Despawn();
                GameController.instance.equippedRod.GetHook().hookedObject = null;
                GameController.instance.equippedRod.GetHook().SetHook(GetComponent<FishableItem>());
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
            if (!GetComponent<FishableItem>().isHooked)
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
            GameController.instance.GetComponent<FoodSearchManager>().fish.Remove(GetComponent<FoodSearch>());
            GameController.instance.GetComponent<FoodSearchManager>().edibleItems.Remove(GetComponent<Edible>());
            DestroyImmediate(gameObject);
        }
    }
}
