using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;

public struct FoodSearchUpdateJob : IJobParallelFor
{
    public void Execute(int index)
    {

    }
}

public class FoodSearchManager : MonoBehaviour
{
    [SerializeField] private List<FoodSearch> fish;

    private void Update()
    {
        FoodSearchUpdateJob job = new FoodSearchUpdateJob();
        JobHandle jobHandle = job.Schedule(fish.Count, 1);
        jobHandle.Complete();
    }
}

public struct Food
{
    public Vector3 position;
    public int inRange;
}

public class FoodSearch : MonoBehaviour
{
    [SerializeField] private float sightAngle;
    [SerializeField] private float sightDistance;
    [SerializeField] private float sightDensity;
    [SerializeField] private float smellRadius;
    [SerializeField] private float chaseDistance;
    [SerializeField] private List<string> foodTypes;
    public GameObject desiredFood;
    private FishableItem desiredFishableItem;
    private HookObject desiredHookObject;

    [SerializeField] private ComputeShader foodComputeShader;
    private Food[] data;

    //void Update()
    //{
    //    if (GetComponent<FishableItem>().isHooked) return;

    //    Look();
    //    SmellGPU();

    //    if (desiredFood != null) ReassessFood();
    //}

    private void Look()
    {
        // Raycast multiple angles in front
        for (int i = 0; i < sightDensity; i++)
        {
            Vector3 dir = -transform.right;
            dir = Quaternion.Euler(0f, 0f, -sightAngle + (i * sightAngle * 2 / sightDensity)) * dir;
            RaycastHit hit;

            // Rule out raycast not hitting anything
            if (!Physics.Raycast(transform.position, dir, out hit, sightDistance)) continue;

            GameObject newFood = IsFood(hit.collider.gameObject);
            if (newFood == null) continue;
            AssignFood(newFood);
        }
    }

    private void Smell()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, smellRadius);
        foreach (Collider col in hitColliders)
        {
            GameObject newFood = IsFood(col.gameObject);
            if (newFood == null) continue;

            AssignFood(newFood);
        }
    }

    private void SlowSmell()
    {
        List<Transform> foodTransforms = GameController.instance.foodTransforms;
        List<Vector3> foodPositions = new List<Vector3>();
        foreach(Transform foodTransform in foodTransforms)
        {
            foodPositions.Add(foodTransform.position);
        }
        for(int i = 0; i < foodPositions.Count; i++)
        {
            if (Vector3.Distance(transform.position, foodPositions[i]) > smellRadius) continue;

            GameObject newFood = IsFood(foodTransforms[i].gameObject);
            if (newFood == null) continue;

            AssignFood(newFood);
        }
    }

    private void SmellGPU()
    {
        List<Transform> foodTransforms = GameController.instance.foodTransforms;
        data = new Food[foodTransforms.Count];

        for(int i = 0; i < data.Length; i++)
        {
            data[i].position = foodTransforms[i].position;
            data[i].inRange = 0;
        }
        int vector3Size = sizeof(float) * 3;
        int intSize = sizeof(int);
        int totalSize = vector3Size + intSize;
        ComputeBuffer foodBuffer = new ComputeBuffer(data.Length, totalSize);
        foodBuffer.SetData(data);

        foodComputeShader.SetBuffer(0, "foods", foodBuffer);
        foodComputeShader.SetVector("fishPosition", transform.position);
        foodComputeShader.SetFloat("range", smellRadius);
        foodComputeShader.Dispatch(0, 8, 8, 1);

        foodBuffer.GetData(data);

        for(int i = 0; i < foodTransforms.Count; i++)
        {
            if (data[i].inRange != 1) continue;

            GameObject newFood = IsFood(foodTransforms[i].gameObject);
            if (newFood == null) continue;

            AssignFood(newFood);
        }

        foodBuffer.Dispose();
    }

    public void ReassessFood()
    {
        if (Vector3.Distance(transform.position, desiredFood.transform.position) > chaseDistance)
        {
            UnsassignFood();
            return;
        }

        // Below water check
        if (desiredFood.transform.position.y > 0f)
        {
            UnsassignFood();
            return;
        }

        // If we're chasing the hook, check if we've already hooked something
        if (desiredHookObject)
        {
            if (desiredHookObject.hookedObject != null)
            {
                UnsassignFood();
                return;
            }
        }

        if (desiredFishableItem)
        {
            if (desiredFishableItem.isHooked)
            {
                UnsassignFood();
                return;
            }
        }
    }

    public GameObject IsFood(GameObject food)
    {
        // Rule out this fish eating itself
        //if (food.gameObject == gameObject) return null;

        // Rule out overwater food
        //if (food.transform.position.y > 0f) return null;

        // Rule out hook object if it's already hooked something
        if (food.GetComponent<HookObject>())
        {
            if (food.GetComponent<HookObject>().hookedObject != null) return null;
        }

        // Rule out food further than we're chasing already
        //if (desiredFood != null)
        //{
        //    if (Vector3.Distance(desiredFood.transform.position, transform.position) <= Vector3.Distance(food.transform.position, transform.position)) return null;
        //}

        // Check if food matches our food types
        foreach (string type in foodTypes)
        {
            //Check for a type the fish wants to eat
            System.Type typeFromString = System.Type.GetType(type);
            if (!food.GetComponent(typeFromString)) continue;
            else return food;
        }

        return null;
    }

    private void AssignFood(GameObject food)
    {
        desiredFood = food;
        if (desiredFood.GetComponent<HookObject>()) desiredHookObject = desiredFood.GetComponent<HookObject>();
        else if (desiredFood.GetComponent<FishableItem>()) desiredFishableItem = desiredFood.GetComponent<FishableItem>();
    }

    private void UnsassignFood()
    {
        desiredFood = null;
        desiredFishableItem = null;
        desiredHookObject = null;
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

    public float GetSightRange() => sightDistance;
    public float GetSightAngle() => sightAngle;
    public float GetSmellRange() => smellRadius;
}
