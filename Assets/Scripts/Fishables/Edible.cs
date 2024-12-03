using System;
using UnityEngine;

namespace Fishing.Fishables.Fish {
    public class Edible : MonoBehaviour {
        [SerializeField, Min(0), Tooltip("The base amount of food for a \"normal\" sized fish of this type.")] private float _baseFoodAmount = 10f;
        public float BaseFoodAmount { get => _baseFoodAmount; private set { } }
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
        [SerializeField, Tooltip("The type of food this is.")] private FoodTypes _foodType;
        public FoodTypes FoodType { get => _foodType; private set { } }


        private FoodSearch _foodSearch;
        private ISpawn _spawn;

        private void Awake() {
            _foodSearch = GetComponent<FoodSearch>();
            _spawn = transform.parent.GetComponent<ISpawn>();
        }
    }
}
