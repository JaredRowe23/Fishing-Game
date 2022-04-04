using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;

namespace Fishing.Fishables.Fish
{
    public struct FoodSearchUpdateJob : IJobParallelFor
    {
        [NativeDisableParallelForRestriction] public NativeArray<FoodSearchData> FoodSearchDataArray;
        [NativeDisableParallelForRestriction] public NativeArray<Vector2> PotentialFoodPositionArray;
        [NativeDisableParallelForRestriction] public NativeArray<int> PotentialFoodTypeArray;
        [NativeDisableParallelForRestriction] public NativeArray<bool> IsOccupiedHookArray;

        public void Execute(int _index)
        {
            FoodSearchData _data = FoodSearchDataArray[_index];
            for (int i = 0; i < PotentialFoodPositionArray.Length; i++)
            {
                _data.toCheckPos = PotentialFoodPositionArray[i];
                _data.toCheckType = PotentialFoodTypeArray[i];
                _data.isOccupiedHook = IsOccupiedHookArray[i];
                _data.foodSearchIndex = i;
                _data.Update();
            }
            FoodSearchDataArray[_index] = _data;
        }
    }

}