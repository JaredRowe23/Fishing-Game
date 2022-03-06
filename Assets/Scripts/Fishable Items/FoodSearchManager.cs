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
        public List<Edible> edibleItems;
        [SerializeField] private int innerloopBatchCount;

        private void Update()
        {
            NativeArray<FoodSearch.Data> foodSearchDataArray = new NativeArray<FoodSearch.Data>(fish.Count, Allocator.TempJob);
            for (int i = 0; i < fish.Count; i++)
            {
                FoodSearch search = fish[i].GetComponent<FoodSearch>();
                foodSearchDataArray[i] = new FoodSearch.Data(fish[i].transform.position, -fish[i].transform.right, search.GetSightRange(), search.GetSightAngle(), search.GetSmellRange(), i, search.GetFoodTypes());
                
            }

            NativeArray<Vector3> potentialFoodPositionArray = new NativeArray<Vector3>(edibleItems.Count, Allocator.TempJob);
            NativeArray<int> potentialFoodTypeArray = new NativeArray<int>(edibleItems.Count, Allocator.TempJob);
            NativeArray<bool> isOccupiedHookArray = new NativeArray<bool>(edibleItems.Count, Allocator.TempJob);

            for (int i = 0; i < edibleItems.Count; i++)
            {
                Vector3 pos = edibleItems[i].transform.position;
                potentialFoodPositionArray[i] = pos;

                int type = edibleItems[i].GetFoodType();
                potentialFoodTypeArray[i] = type;

                isOccupiedHookArray[i] = false;
                if (!edibleItems[i].GetComponent<HookBehaviour>())
                {
                    continue;
                }

                if (edibleItems[i].GetComponent<HookBehaviour>().hookedObject != null)
                {
                    isOccupiedHookArray[i] = true;
                }
            }

            FoodSearchUpdateJob job = new FoodSearchUpdateJob
            {
                FoodSearchDataArray = foodSearchDataArray,
                PotentialFoodPositionArray = potentialFoodPositionArray,
                PotentialFoodTypeArray = potentialFoodTypeArray,
                IsOccupiedHookArray = isOccupiedHookArray
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
                fish[i].desiredFood = edibleItems[foodSearchDataArray[i].nearestFoodIndex].gameObject;
            }

            foodSearchDataArray.Dispose();
            potentialFoodPositionArray.Dispose();
            potentialFoodTypeArray.Dispose();
            isOccupiedHookArray.Dispose();
        }
    }

}