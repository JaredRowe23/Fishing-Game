using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

namespace Fishing.FishingMechanics
{
    public struct BaitUpdateJob : IJobParallelFor
    {
        [NativeDisableParallelForRestriction] public NativeArray<BaitData> BaitDataArray;

        public void Execute(int _index)
        {
            BaitData _data = BaitDataArray[_index];
            _data.Update();
            BaitDataArray[_index] = _data;
        }
    }
}
