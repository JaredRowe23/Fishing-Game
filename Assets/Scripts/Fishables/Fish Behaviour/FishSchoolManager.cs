using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;

namespace Fishing.Fishables.Fish
{
    public class FishSchoolManager : MonoBehaviour
    {
        [SerializeField] private List<Fish> fish;
        [SerializeField] private int innerloopBatchCount;
        private FishSchoolBehaviour school;

        private void Start()
        {
            school = GetComponent<FishSchoolBehaviour>();
        }

        private void Update()
        {
            if (fish.Count == 0) return;
            NativeArray<ShoalData> _shoalDataArray = new NativeArray<ShoalData>(fish.Count, Allocator.TempJob);
            NativeArray<Vector2> _shoalmatePositionArray = new NativeArray<Vector2>(fish.Count, Allocator.TempJob);

            for (int i = 0; i < fish.Count; i++)
            {
                Fish f = fish[i];
                _shoalDataArray[i] = new ShoalData((Vector2)f.transform.position, f.transform.right, school.separationAngle, school.separationMaxDistance, school.separationMaxCloseDistance);
                _shoalmatePositionArray[i] = (Vector2)f.transform.position;
            }

            FishSchoolUpdateJob _job = new FishSchoolUpdateJob
            {
                ShoalDataArray = _shoalDataArray,
                ShoalmatePositionArray = _shoalmatePositionArray
            };

            JobHandle _jobHandle = _job.Schedule(_shoalDataArray.Length, innerloopBatchCount);
            _jobHandle.Complete();

            for (int i = 0; i < fish.Count; i++)
            {
                if (_shoalDataArray[i].desiredAngle == 0)
                {
                    fish[i].GetComponent<Shoal>().separationDir = 0f;
                    continue;
                }
                fish[i].GetComponent<Shoal>().separationDir = _shoalDataArray[i].desiredAngle;
            }

            _shoalDataArray.Dispose();
            _shoalmatePositionArray.Dispose();
        }

        public void AddFish(Fish _fish) => fish.Add(_fish);
        public void RemoveFish(Fish _fish)
        {
            if (fish.Contains(_fish)) fish.Remove(_fish);
        }
    }
}
