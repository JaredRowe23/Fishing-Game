using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish1Behaviour : MonoBehaviour
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

    private void Awake() => foodSearch = GetComponent<FoodSearch>();

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
                if (!foodSearch.desiredFood.GetComponent<HookObject>())
                {
                    Eat();
                }
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
        GetComponent<AudioSource>().Play();
        Destroy(foodSearch.desiredFood);
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
}
