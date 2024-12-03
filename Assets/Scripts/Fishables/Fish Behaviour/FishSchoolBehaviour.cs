using Fishing.Util;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.Fishables.Fish {
    public class FishSchoolBehaviour : MonoBehaviour {

        private List<Shoal> _shoals;
        public List<Shoal> Shoals { get => _shoals; private set { _shoals = value; } }

        private float _averageRotation;
        public float AverageRotation { get => _averageRotation; private set => _averageRotation = value; }

        private Vector2 _schoolCenter;
        public Vector2 SchoolCenter { get => _schoolCenter; private set => _schoolCenter = value; }

        private void Awake() {
            Shoals = new List<Shoal>();
        }

        public void FixedUpdate() {
            CalculateSchoolCenter();
            CalculateSchoolAverageRotation();
        }

        private void CalculateSchoolCenter() {
            SchoolCenter = Vector2.zero;
            foreach (Shoal shoal in Shoals) {
                SchoolCenter += (Vector2)shoal.transform.position;
            }
            SchoolCenter /= Shoals.Count;
        }

        private void CalculateSchoolAverageRotation() {
            Vector2 averageRotationVector = Vector2.zero;
            for (int i = 0; i < Shoals.Count; i++) {
                Vector2 shoalForward = Shoals[i].transform.up;
                averageRotationVector += shoalForward;
            }
            averageRotationVector /= Shoals.Count;
            AverageRotation = Vector2.SignedAngle(Vector2.up, averageRotationVector);
        }
    }
}
