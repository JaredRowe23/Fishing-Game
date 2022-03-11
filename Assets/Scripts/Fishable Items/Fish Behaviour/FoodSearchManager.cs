using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Fishing.FishingMechanics;

namespace Fishing.Fishables.Fish
{
    public class FoodSearchManager : MonoBehaviour
    {
        public List<FoodSearch> fish;
        public List<Edible> edibleItems;
        [SerializeField] private int innerloopBatchCount;

        private void Update()
        {
            NativeArray<FoodSearch.Data> _foodSearchDataArray = new NativeArray<FoodSearch.Data>(fish.Count, Allocator.TempJob);
            for (int i = 0; i < fish.Count; i++)
            {
                FoodSearch _search = fish[i].GetComponent<FoodSearch>();
                _foodSearchDataArray[i] = new FoodSearch.Data(fish[i].transform.position, -fish[i].transform.right, _search.GetSightRange(), _search.GetSightAngle(), _search.GetSmellRange(), i, _search.GetFoodTypes());
                
            }

            NativeArray<Vector3> _potentialFoodPositionArray = new NativeArray<Vector3>(edibleItems.Count, Allocator.TempJob);
            NativeArray<int> _potentialFoodTypeArray = new NativeArray<int>(edibleItems.Count, Allocator.TempJob);
            NativeArray<bool> _isOccupiedHookArray = new NativeArray<bool>(edibleItems.Count, Allocator.TempJob);

            for (int i = 0; i < edibleItems.Count; i++)
            {
                Vector3 pos = edibleItems[i].transform.position;
                _potentialFoodPositionArray[i] = pos;

                int type = edibleItems[i].GetFoodType();
                _potentialFoodTypeArray[i] = type;

                _isOccupiedHookArray[i] = false;
                if (!edibleItems[i].GetComponent<HookBehaviour>())
                {
                    continue;
                }

                if (edibleItems[i].GetComponent<HookBehaviour>().hookedObject != null)
                {
                    _isOccupiedHookArray[i] = true;
                }
            }

            FoodSearchUpdateJob _job = new FoodSearchUpdateJob
            {
                FoodSearchDataArray = _foodSearchDataArray,
                PotentialFoodPositionArray = _potentialFoodPositionArray,
                PotentialFoodTypeArray = _potentialFoodTypeArray,
                IsOccupiedHookArray = _isOccupiedHookArray
            };

            JobHandle _jobHandle = _job.Schedule(_foodSearchDataArray.Length, innerloopBatchCount);
            _jobHandle.Complete();

            for (int i = 0; i < fish.Count; i++)
            {
                if (_foodSearchDataArray[i].nearestFoodIndex == -1)
                {
                    fish[i].desiredFood = null;
                    continue;
                }
                fish[i].desiredFood = edibleItems[_foodSearchDataArray[i].nearestFoodIndex].gameObject;
            }

            _foodSearchDataArray.Dispose();
            _potentialFoodPositionArray.Dispose();
            _potentialFoodTypeArray.Dispose();
            _isOccupiedHookArray.Dispose();
        }
    }

}