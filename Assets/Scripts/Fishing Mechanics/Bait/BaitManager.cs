using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using Fishing.Fishables;
using Fishing.Fishables.Fish;

namespace Fishing.FishingMechanics
{
    public class BaitManager : MonoBehaviour
    {
        public List<FoodSearch> fish;
        [SerializeField] private int innerloopBatchCount;
        public BaitBehaviour bait;

        public static BaitManager instance;

        private BaitManager() => instance = this;

        private void Update()
        {
            if (bait == null) return;
            if (bait.transform.position.y >= 0f) return;

            NativeArray<BaitData> _baitDataArray = new NativeArray<BaitData>(fish.Count, Allocator.TempJob);
            for (int i = 0; i < fish.Count; i++)
            {
                FoodSearch _search = fish[i].GetComponent<FoodSearch>();
                _baitDataArray[i] = new BaitData(bait.transform.position, bait.GetScriptable().areaOfEffect, bait.GetScriptable().GetFoodTypes(), fish[i].transform.position, fish[i].GetComponent<Edible>().GetFoodType());

            }

            BaitUpdateJob _job = new BaitUpdateJob
            {
                BaitDataArray = _baitDataArray
            };

            JobHandle _jobHandle = _job.Schedule(_baitDataArray.Length, innerloopBatchCount);
            _jobHandle.Complete();

            for (int i = 0; i < fish.Count; i++)
            {
                if (_baitDataArray[i].isBaited)
                {
                    fish[i].desiredFood = bait.gameObject;
                    continue;
                }
            }

            _baitDataArray.Dispose();
        }

        public void AddFish(FoodSearch _fish) => fish.Add(_fish);

        public void RemoveFish(FoodSearch _fish)
        {
            if (fish.Contains(_fish)) fish.Remove(_fish);
        }
    }
}
