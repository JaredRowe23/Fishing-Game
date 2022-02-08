// For this fish, we mainly just want it to wander randomly up to
// a certain maximum distance from it's spawn, and stay for a moment

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
    [SerializeField] private float holdCount;

    private bool isWandering;
    private Vector3 targetPos;

    void Start()
    {
        wanderDistance += Random.Range(-wanderDistanceVariation, wanderDistanceVariation);
        maxHomeDistance += Random.Range(-maxHomeDistanceVariation, maxHomeDistanceVariation);
    }
    
    void Update()
    {
        // If we aren't hooked, then set our wander position to a random location in range if we aren't already wandering/holding.
        // if we are already wandering, move towards our desired position, and if we're close enough then hold until our timer runs out
        if (!GetComponent<FishableItem>().isHooked && transform.parent != GameController.instance.bucket.transform)
        {
            if (!GetComponent<FoodSearch>().desiredFood)
            {
                if (!isWandering)
                {
                    if (holdCount <= 0)
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
                        isWandering = true;
                    }
                    else
                    {
                        holdCount -= Time.deltaTime;
                    }
                }
                else if (isWandering)
                {
                    if (Vector3.Distance(transform.position, targetPos) > distanceThreshold)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, targetPos, wanderSpeed * Time.deltaTime);
                    }
                    else
                    {
                        holdCount = holdTime;
                        isWandering = false;
                    }
                }
            }
            else
            {
                targetPos = GetComponent<FoodSearch>().desiredFood.transform.position;
                if (Vector3.Distance(transform.position, targetPos) > eatDistance)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, chaseSpeed * Time.deltaTime);
                }
                else
                {
                    if (!GetComponent<FoodSearch>().desiredFood.GetComponent<HookObject>())
                    {
                        if (GetComponent<FoodSearch>().desiredFood.transform.parent)
                        {
                            if (GetComponent<FoodSearch>().desiredFood.transform.parent.GetComponent<SpawnZone>())
                            {
                                GetComponent<FoodSearch>().desiredFood.transform.parent.GetComponent<SpawnZone>().spawnList.Remove(GetComponent<FoodSearch>().desiredFood);
                            }
                        }
                        GetComponent<AudioSource>().Play();
                        Destroy(GetComponent<FoodSearch>().desiredFood);
                        GetComponent<FoodSearch>().desiredFood = null;
                    }
                }
            }

            if (targetPos.x < transform.position.x)
            {
                transform.rotation = Quaternion.identity;
            }
            else if (targetPos.x > transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
        }
    }

    // Draws our specific fish's wander distance range
    private void OnDrawGizmosSelected()
    {
        if (!GetComponent<FishableItem>().isHooked)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, wanderDistance);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.parent.position, maxHomeDistance);
            if (GetComponent<FoodSearch>().desiredFood == null)
            {
                Debug.DrawRay(transform.position, targetPos - transform.position, Color.green);
            }
        }
    }
}
