using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

public class FoodSearchManager : MonoBehaviour
{
    public List<FoodSearch> fish;
    public List<FishableItem> fishableItems;

    private void Update()
    {
        NativeArray<FoodSearch.Data> foodSearchDataArray = new NativeArray<FoodSearch.Data>(fish.Count, Allocator.TempJob);
        for (int i = 0; i < fish.Count; i++)
        {
            FoodSearch search = fish[i].GetComponent<FoodSearch>();
            foodSearchDataArray[i] = new FoodSearch.Data(fish[i].transform.position, fish[i].transform.forward, search.GetSightRange(), search.GetSightAngle(), search.GetSmellRange());
        }

        NativeArray<Vector3> potentialFoodPositionArray = new NativeArray<Vector3>(fishableItems.Count, Allocator.TempJob);
        for (int j = 0; j < fishableItems.Count; j++)
        {
            Vector3 pos = fishableItems[j].transform.position;
            potentialFoodPositionArray[j] = pos;
        }

        FoodSearchUpdateJob job = new FoodSearchUpdateJob
        {
            FoodSearchDataArray = foodSearchDataArray,
            PotentialFoodPositionArray = potentialFoodPositionArray
        };

        JobHandle jobHandle = job.Schedule(foodSearchDataArray.Length, 4);
        jobHandle.Complete();
        foodSearchDataArray.Dispose();
        potentialFoodPositionArray.Dispose();
    }
}
