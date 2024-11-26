using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.Fishables.Fish
{
    public class Edible : MonoBehaviour
    {
        public float baseFoodAmount;
        [Flags] public enum FoodTypes { 
            Anglerfish = 1, 
            Boot = 2, 
            Carp = 4, 
            Crab = 8, 
            Driftwood = 16, 
            EarthWorm = 32, 
            Hook = 64, 
            Minnow = 128, 
            Salmon = 256, 
            SeaSerpent = 512, 
            Seaweed = 1024, 
            TinCan = 2048, 
            WaterLilyFruit = 4096
        };
        [SerializeField] private FoodTypes _foodType;
        public FoodTypes FoodType { get => _foodType; private set { } }

        private FoodSearch foodSearch;
        [SerializeField] private ISpawn spawn;

        private void Awake()
        {
            foodSearch = GetComponent<FoodSearch>();
            spawn = transform.parent.GetComponent<ISpawn>();
        }

        public int GetFoodType() => (int)_foodType;

        public void Despawn()
        {
            RemovePredatorFromPrey();
            spawn.RemoveFromList(gameObject);
            DestroyImmediate(gameObject);
        }

        private void RemovePredatorFromPrey()
        {
            if (foodSearch == null) return;
            if (foodSearch.DesiredFood == null) return;
            if (!foodSearch.DesiredFood.GetComponent<FishMovement>()) return;

            foodSearch.DesiredFood.GetComponent<FishMovement>().activePredator = null;
        }
    }
}
