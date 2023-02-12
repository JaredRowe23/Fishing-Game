using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.Fishables.Fish;

namespace Fishing.Fishables
{
    public class Edible : MonoBehaviour
    {
        public float baseFoodAmount;
        public enum FoodTypes { Hook, Salmon, TinCan, EarthWorm, Carp, Seaweed, Boot, Driftwood, Minnow, WaterLilyFruit, Anglerfish, Fish11 };
        [SerializeField] private FoodTypes foodType;

        private void Start() => FoodSearchManager.instance.AddFood(this);

        public int GetFoodType() => (int)foodType;
    }
}
