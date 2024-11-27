using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.Fishables.Fish
{
    public class FishSchoolBehaviour : MonoBehaviour {
        [SerializeField] private float _separationMaxDistance;
        public float SeparationMaxDistance { get => _separationMaxDistance; private set { } }

        [SerializeField] private float _separationMaxCloseDistance;
        public float SeparationMaxCloseDistance { get => _separationMaxCloseDistance; private set { } }

        [SerializeField, Range(0, 90)] private float _separationAngle;
        public float SeparationAngle { get => _separationAngle; private set { } }

        [SerializeField] private List<Shoal> _shoals;
        public List<Shoal> Shoals { get => _shoals; set => _shoals = value; }

        [SerializeField] private float _averageAngle;
        public float AverageAngle { get => _averageAngle; private set => _averageAngle = value; }

        [SerializeField] private Vector2 _schoolCenter;
        public Vector2 SchoolCenter { get => _schoolCenter; private set => _schoolCenter = value; }

        public void FixedUpdate()
        {
            CalculateSchoolCenter();
            AverageAngle = AverageRotation();
        }

        private void CalculateSchoolCenter() {
            SchoolCenter = Vector2.zero;
            AverageAngle = 0f;
            foreach (Shoal shoal in Shoals) {
                SchoolCenter = SchoolCenter + (Vector2)shoal.transform.position;
            }
            SchoolCenter = SchoolCenter / Shoals.Count;
        }

        private float AverageRotation()
        {
            float averageRotation = 0;
            for (int i = 0; i < Shoals.Count; i++) {
                averageRotation += Shoals[i].transform.rotation.eulerAngles.z;
            }
            averageRotation /= Shoals.Count;
            return averageRotation;
        }
    }
}
