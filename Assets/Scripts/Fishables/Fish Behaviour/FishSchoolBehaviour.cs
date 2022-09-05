using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.Fishables.Fish
{
    public class FishSchoolBehaviour : MonoBehaviour
    {
        public float separationMaxDistance;
        public float separationMaxCloseDistance;
        [Range(0, 90)]
        public float separationAngle;
        [Range(0, 1)]
        public float alignment;
        [Range(0, 1)]
        public float cohesion;

        public List<Fishable> fish;
        public float averageAngle;
        public Vector2 schoolCenter;

        private SpawnZone spawnZone;

        public void Update()
        {
            schoolCenter = Vector2.zero;
            averageAngle = 0f;
            foreach (Fishable f in fish)
            {
                schoolCenter = schoolCenter + (Vector2)f.transform.position;
            }
            schoolCenter = schoolCenter / fish.Count;

            averageAngle = AverageRotation();
        }

        private float AverageRotation()
        {
            int _count = fish.Count;

            if (fish == null || _count < 1) return 0f;

            if (_count < 2) return (360 - fish[0].transform.rotation.eulerAngles.z + 270) % 360;

            float weight = 1.0f / (float)_count;
            Vector2 avg = Vector2.zero;
            for (int i = 0; i < _count; i++)
            {
                avg += new Vector2(Mathf.Sin(((360 - fish[i].transform.rotation.eulerAngles.z + 270) % 360) * Mathf.Deg2Rad), Mathf.Cos(((360 - fish[i].transform.rotation.eulerAngles.z + 270) % 360) * Mathf.Deg2Rad));
            }

            return (360 - (Mathf.Atan2(avg.y, avg.x) * Mathf.Rad2Deg) + 90) % 360;
        }
    }
}
