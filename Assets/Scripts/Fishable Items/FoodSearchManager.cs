using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

namespace Fishing
{
    public class FoodSearchManager : MonoBehaviour
    {
        public List<FoodSearch> fish;
        public List<FishableItem> fishableItems;
        [SerializeField] private int innerloopBatchCount;

        private void Update()
        {
            NativeArray<FoodSearch.Data> foodSearchDataArray = new NativeArray<FoodSearch.Data>(fish.Count, Allocator.TempJob);
            for (int i = 0; i < fish.Count; i++)
            {
                FoodSearch search = fish[i].GetComponent<FoodSearch>();
                foodSearchDataArray[i] = new FoodSearch.Data(fish[i].transform.position, -fish[i].transform.right, search.GetSightRange(), search.GetSightAngle(), search.GetSmellRange(), i, search.GetFoodTypes());
            }

            NativeArray<Vector3> potentialFoodPositionArray = new NativeArray<Vector3>(fishableItems.Count, Allocator.TempJob);
            NativeArray<int> potentialFoodTypeArray = new NativeArray<int>(fishableItems.Count, Allocator.TempJob);

            for (int i = 0; i < fishableItems.Count; i++)
            {
                Vector3 pos = fishableItems[i].transform.position;
                potentialFoodPositionArray[i] = pos;

                int type = fishableItems[i].GetFoodType();
                potentialFoodTypeArray[i] = type;
            }

            FoodSearchUpdateJob job = new FoodSearchUpdateJob
            {
                FoodSearchDataArray = foodSearchDataArray,
                PotentialFoodPositionArray = potentialFoodPositionArray,
                PotentialFoodTypeArray = potentialFoodTypeArray
            };

            JobHandle jobHandle = job.Schedule(foodSearchDataArray.Length, innerloopBatchCount);
            jobHandle.Complete();

            for (int i = 0; i < fish.Count; i++)
            {
                if (foodSearchDataArray[i].nearestFoodIndex == -1)
                {
                    fish[i].desiredFood = null;
                    continue;
                }
                fish[i].desiredFood = fishableItems[foodSearchDataArray[i].nearestFoodIndex].gameObject;
            }

            foodSearchDataArray.Dispose();
            potentialFoodPositionArray.Dispose();
            potentialFoodTypeArray.Dispose();
        }
    }

}