using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;

namespace Fishing.Fishables.Fish
{
    [BurstCompile]
    public struct FishSchoolUpdateJob : IJobParallelFor
    {
        [NativeDisableParallelForRestriction] public NativeArray<ShoalData> ShoalDataArray;
        [NativeDisableParallelForRestriction] public NativeArray<Vector2> ShoalmatePositionArray;

        public void Execute(int _index)
        {
            ShoalData _data = ShoalDataArray[_index];
            for (int i = 0; i < ShoalmatePositionArray.Length; i++)
            {
                _data.shoalmatePos = ShoalmatePositionArray[i];
                _data.FixedUpdate();
            }
            ShoalDataArray[_index] = _data;
        }
    }
}
