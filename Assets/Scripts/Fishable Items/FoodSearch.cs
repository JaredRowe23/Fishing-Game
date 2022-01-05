using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSearch : MonoBehaviour
{
    [SerializeField] private float sightAngle;
    [SerializeField] private float sightDistance;
    [SerializeField] private float sightDensity;
    [SerializeField] private float smellRadius;
    [SerializeField] private float chaseDistance;
    [SerializeField] private List<string> foodTypes;
    public GameObject desiredFood;

    // Update is called once per frame
    void Update()
    {
        // If we have food to chase, check if we should still chase it
        if (desiredFood != null)
        {
            // Distance check
            if (Vector3.Distance(transform.position, desiredFood.transform.position) > chaseDistance || desiredFood.transform.position.y >= 0f)
            {
                desiredFood = null;
            }
            
            // If we're chasing the hook, check if we've already hooked something
            else if (desiredFood.GetComponent<HookObject>() != null)
            {
                if (desiredFood.GetComponent<HookObject>().hookedObject != null || desiredFood.GetComponent<HookObject>().hookedObject != gameObject)
                {
                    desiredFood = null;
                }
            }
        }
        
        if (!GetComponent<FishableItem>().isHooked)
        {
            // Raycast multiple angles in front
            for (int i = 0; i < sightDensity; i++)
            {
                Vector3 dir = -transform.right;
                dir = Quaternion.Euler(0f, 0f, -sightAngle + (i * sightAngle * 2 / sightDensity)) * dir;
                RaycastHit hit;
                if (Physics.Raycast(transform.position, dir, out hit, sightDistance))
                {
                    if (hit.collider.gameObject != gameObject)
                    {
                        // Check to see if raycast hit matches one of our food types we want
                        foreach (string type in foodTypes)
                        {
                            System.Type typeFromString = System.Type.GetType(type);
                            if (hit.collider.GetComponent(typeFromString))
                            {
                                // Check if food is in the water
                                if (hit.collider.transform.position.y <= 0f)
                                {
                                    // If we already are chasing food, check if this food is closer
                                    if (desiredFood != null)
                                    {
                                        if (Vector3.Distance(desiredFood.transform.position, transform.position) > Vector3.Distance(hit.collider.transform.position, transform.position))
                                        {
                                            // If food is the hook, check if we already have hooked something else
                                            if (hit.collider.GetComponent<HookObject>())
                                            {
                                                if (hit.collider.GetComponent<HookObject>().hookedObject == null)
                                                {
                                                    desiredFood = hit.collider.gameObject;
                                                }
                                            }
                                            else
                                            {
                                                desiredFood = hit.collider.gameObject;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (hit.collider.GetComponent<HookObject>())
                                        {
                                            if (hit.collider.GetComponent<HookObject>().hookedObject == null)
                                            {
                                                desiredFood = hit.collider.gameObject;
                                            }
                                        }
                                        else
                                        {
                                            desiredFood = hit.collider.gameObject;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }


            // Check within a sphere to see if we smell any food
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, smellRadius);
            foreach (Collider col in hitColliders)
            {
                if (col.gameObject != gameObject)
                {
                    // Check if food matches our food types
                    foreach (string type in foodTypes)
                    {
                        System.Type typeFromString = System.Type.GetType(type);
                        if (col.GetComponent(typeFromString))
                        {
                            // Check if food is under the water
                            if (col.transform.position.y <= 0f)
                            {
                                // If we have food to chase, check if this food is closer
                                if (desiredFood != null)
                                {
                                    if (Vector3.Distance(desiredFood.transform.position, transform.position) > Vector3.Distance(col.transform.position, transform.position))
                                    {
                                        // If food it the fishing hook, check if it already has hooked something
                                        if (col.GetComponent<HookObject>())
                                        {
                                            if (col.GetComponent<HookObject>().hookedObject == null)
                                            {
                                                desiredFood = col.gameObject;
                                            }
                                        }
                                        else
                                        {
                                            desiredFood = col.gameObject;
                                        }
                                    }
                                }
                                else
                                {
                                    if (col.GetComponent<HookObject>())
                                    {
                                        if (col.GetComponent<HookObject>().hookedObject == null)
                                        {
                                            desiredFood = col.gameObject;
                                        }
                                    }
                                    else
                                    {
                                        desiredFood = col.gameObject;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!desiredFood)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, smellRadius);

            for (int i = 0; i < sightDensity; i++)
            {
                Vector3 dir = -transform.right;
                dir = Quaternion.Euler(0f, 0f, -sightAngle + (i * sightAngle * 2 / sightDensity)) * dir;
                Debug.DrawRay(transform.position, dir * sightDistance, Color.green);
            }
        }
        else
        {
            Debug.DrawRay(transform.position, (desiredFood.transform.position - transform.position), Color.red);
        }
    }
}
