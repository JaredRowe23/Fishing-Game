using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;

public struct FoodSearchUpdateJob : IJobParallelFor
{
    [NativeDisableParallelForRestriction] public NativeArray<FoodSearch.Data> FoodSearchDataArray;
    [NativeDisableParallelForRestriction] public NativeArray<Vector3> PotentialFoodPositionArray;

    public void Execute(int index)
    {
        FoodSearch.Data data = FoodSearchDataArray[index];
        for (int i = 0; i < PotentialFoodPositionArray.Length; i++)
        {

            data.toCheckPos = PotentialFoodPositionArray[i];
            data.Update();
            FoodSearchDataArray[index] = data;
        }
    }
}
