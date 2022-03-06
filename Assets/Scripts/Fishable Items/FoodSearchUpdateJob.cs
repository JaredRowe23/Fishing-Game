using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;

namespace Fishing
{
    public struct FoodSearchUpdateJob : IJobParallelFor
    {
        [NativeDisableParallelForRestriction] public NativeArray<FoodSearch.Data> FoodSearchDataArray;
        [NativeDisableParallelForRestriction] public NativeArray<Vector3> PotentialFoodPositionArray;
        [NativeDisableParallelForRestriction] public NativeArray<int> PotentialFoodTypeArray;
        [NativeDisableParallelForRestriction] public NativeArray<bool> IsOccupiedHookArray;

        public void Execute(int index)
        {
            FoodSearch.Data data = FoodSearchDataArray[index];
            for (int i = 0; i < PotentialFoodPositionArray.Length; i++)
            {
                data.toCheckPos = PotentialFoodPositionArray[i];
                data.toCheckType = PotentialFoodTypeArray[i];
                data.isOccupiedHook = IsOccupiedHookArray[i];
                data.foodSearchIndex = i;
                data.Update();
            }
            FoodSearchDataArray[index] = data;
        }
    }

}