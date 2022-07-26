using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.FishingMechanics
{
    [CreateAssetMenu(fileName = "New Fishing Rod", menuName = "Fishing Rod")]
    public class RodScriptable : ScriptableObject
    {
        public Sprite inventorySprite;
        public string rodName;
        public string description;
        public GameObject prefab;
        public float cost;
        public float lineLength;
        public float reelSpeed;
        public float minCastStrength;
        public float maxCastStrength;
        public float maxCastAngle;
        public float chargeFrequency;
        public float angleFrequency;
    }
}
