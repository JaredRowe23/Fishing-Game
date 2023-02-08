using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.Fishables.Fish;

namespace Fishing.Fishables
{
    public class Edible : MonoBehaviour
    {
        private void Start() => FoodSearchManager.instance.AddFood(this);

        public enum FoodTypes { Hook, Salmon, TinCan, EarthWorm, Carp, Seaweed, Boot, Driftwood, Minnow, WaterLilyFruit, Fish10, Fish11 };
        [SerializeField] private FoodTypes foodType;

        public int GetFoodType() => (int)foodType;
    }
}
