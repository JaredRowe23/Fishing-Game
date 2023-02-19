using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.Fishables.Fish
{
    public class Edible : MonoBehaviour
    {
        public float baseFoodAmount;
        public enum FoodTypes { Hook, Salmon, TinCan, EarthWorm, Carp, Seaweed, Boot, Driftwood, Minnow, WaterLilyFruit, Anglerfish, SeaSerpent, Crab};
        [SerializeField] private FoodTypes foodType;

        private FoodSearch foodSearch;
        private ISpawn spawn;

        private void Awake()
        {
            foodSearch = GetComponent<FoodSearch>();
            spawn = transform.parent.GetComponent<ISpawn>();
        }

        private void Start() => FoodSearchManager.instance.AddFood(this);

        public int GetFoodType() => (int)foodType;

        public void Despawn()
        {
            if (foodSearch != null)
            {
                if (foodSearch.desiredFood != null)
                {
                    if (foodSearch.desiredFood.GetComponent<FishMovement>()) foodSearch.desiredFood.GetComponent<FishMovement>().activePredator = null;
                }
            }
            spawn.RemoveFromList(gameObject);
            FoodSearchManager.instance.RemoveFood(GetComponent<Edible>());
            DestroyImmediate(gameObject);
        }
    }
}
