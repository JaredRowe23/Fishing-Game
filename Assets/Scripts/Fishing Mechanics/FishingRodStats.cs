using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.FishingMechanics
{
    public class FishingRodStats : MonoBehaviour
    {
        [SerializeField] private string description;
        [SerializeField] private float lineLength;
        [SerializeField] private float reelSpeed;
        [SerializeField] private float minCastStrength;
        [SerializeField] private float maxCastStrength;
        [SerializeField] private float maxCastAngle;
        [SerializeField] private float chargeFrequency;
        [SerializeField] private float angleFrequency;

        public string GetDescription() => description;
        public float GetLineLength() => lineLength;
        public float GetReelSpeed() => reelSpeed;
        public float GetMinCastStrength() => minCastStrength;
        public float GetMaxCastStrength() => maxCastStrength;
        public float GetMaxCastAngle() => maxCastAngle;
        public float GetChargeFrequency() => chargeFrequency;
        public float GetAngleFrequency() => angleFrequency;
    }
}
