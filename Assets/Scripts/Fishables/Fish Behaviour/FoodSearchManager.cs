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

        public static FoodSearchManager instance;

        private FoodSearchManager() => instance = this;

        private void Update()
        {
            NativeArray<FoodSearchData> _foodSearchDataArray = new NativeArray<FoodSearchData>(fish.Count, Allocator.TempJob);
            for (int i = 0; i < fish.Count; i++)
            {
                FoodSearch _search = fish[i].GetComponent<FoodSearch>();
                _foodSearchDataArray[i] = new FoodSearchData(fish[i].transform.position, -fish[i].transform.right, _search.GetSightRange(), _search.GetSightAngle(), _search.GetSmellRange(), i, _search.GetFoodTypes());
                
            }

            NativeArray<Vector2> _potentialFoodPositionArray = new NativeArray<Vector2>(edibleItems.Count, Allocator.TempJob);
            NativeArray<int> _potentialFoodTypeArray = new NativeArray<int>(edibleItems.Count, Allocator.TempJob);
            NativeArray<bool> _isOccupiedHookArray = new NativeArray<bool>(edibleItems.Count, Allocator.TempJob);

            for (int i = 0; i < edibleItems.Count; i++)
            {
                Vector2 pos = edibleItems[i].transform.position;
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
                    if (fish[i].desiredFood != null)
                    {
                        if (fish[i].desiredFood.GetComponent<Fish>()) fish[i].desiredFood.GetComponent<Fish>().activePredator = null;
                    }
                    fish[i].desiredFood = null;
                    continue;
                }
                fish[i].desiredFood = edibleItems[_foodSearchDataArray[i].nearestFoodIndex].gameObject;
                if (!fish[i].desiredFood.GetComponent<Fish>()) continue;
                Fish desiredFish = fish[i].desiredFood.GetComponent<Fish>();
                if (desiredFish.activePredator != null)
                {
                    if (Vector3.Distance(fish[i].transform.position, fish[i].desiredFood.transform.position) >= Vector3.Distance(desiredFish.activePredator.transform.position, fish[i].desiredFood.transform.position)) continue;
                }
                fish[i].desiredFood.GetComponent<Fish>().activePredator = fish[i].gameObject;
            }

            _foodSearchDataArray.Dispose();
            _potentialFoodPositionArray.Dispose();
            _potentialFoodTypeArray.Dispose();
            _isOccupiedHookArray.Dispose();
        }
        public void AddFood(Edible _food) => edibleItems.Add(_food);
        public void AddFish(FoodSearch _fish) => fish.Add(_fish);
        public void RemoveFish(FoodSearch _fish)
        {
            if (fish.Contains(_fish)) fish.Remove(_fish);
        }
        public void RemoveFood(Edible _food)
        {
            if (edibleItems.Contains(_food)) edibleItems.Remove(_food);
        }
    }

}