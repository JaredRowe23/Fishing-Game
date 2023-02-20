using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;

namespace Fishing.Fishables.Fish
{
    public class FishSchoolManager : MonoBehaviour
    {
        [SerializeField] private List<Shoal> shoals;
        [SerializeField] private int innerloopBatchCount;
        private FishSchoolBehaviour school;

        private void Start()
        {
            school = GetComponent<FishSchoolBehaviour>();
        }

        private void FixedUpdate()
        {
            if (shoals.Count == 0) return;
            NativeArray<ShoalData> _shoalDataArray = new NativeArray<ShoalData>(shoals.Count, Allocator.TempJob);
            NativeArray<Vector2> _shoalmatePositionArray = new NativeArray<Vector2>(shoals.Count, Allocator.TempJob);

            for (int i = 0; i < shoals.Count; i++)
            {
                Shoal f = shoals[i];
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

            for (int i = 0; i < shoals.Count; i++)
            {
                if (_shoalDataArray[i].desiredAngle == 0)
                {
                    shoals[i].GetComponent<Shoal>().separationDir = 0f;
                    continue;
                }
                shoals[i].GetComponent<Shoal>().separationDir = _shoalDataArray[i].desiredAngle;
            }

            _shoalDataArray.Dispose();
            _shoalmatePositionArray.Dispose();
        }

        public void AddShoal(Shoal _shoal) => shoals.Add(_shoal);
        public void RemoveShoal(Shoal _shoal)
        {
            if (shoals.Contains(_shoal)) shoals.Remove(_shoal);
        }
    }
}
