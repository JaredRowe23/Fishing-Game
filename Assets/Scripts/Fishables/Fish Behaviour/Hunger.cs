using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.Fishables.Fish
{
    [RequireComponent(typeof(FoodSearch))]
    public class Hunger : MonoBehaviour
    {
        [SerializeField] private float hungerStart = 100;
        [SerializeField] private float decayRate = 0.1f;
        [SerializeField] private float foodStart = 50;
        [SerializeField] private float foodStartVariance = 10;
        [HideInInspector] public float currentFood;

        private Fishable fishable;
        private FoodSearch foodSearch;
        private IEdible edible;
        private FoodSearchManager foodSearchManager;

        private void Awake()
        {
            fishable = GetComponent<Fishable>();
            foodSearch = GetComponent<FoodSearch>();
            edible = GetComponent<IEdible>();
        }

        void Start()
        {
            foodSearchManager = FoodSearchManager.instance;
            currentFood = foodStart + Random.Range(-foodStartVariance, foodStartVariance);
        }

        void Update()
        {
            if (fishable.isHooked) return;

            currentFood -= Time.deltaTime * decayRate;
            if (currentFood <= 0) Starve();
            else HandleHunger();
        }

        private void Starve()
        {
            edible.Despawn();
        }

        private void HandleHunger()
        {
            if (currentFood <= hungerStart)
            {
                if (!foodSearchManager.fish.Contains(foodSearch)) foodSearchManager.AddFish(foodSearch);
            }
            else if (foodSearchManager.fish.Contains(foodSearch)) foodSearchManager.RemoveFish(foodSearch);
        }

        public void AddFood(GameObject _food)
        {
            Fishable _foodFishable = _food.GetComponent<Fishable>();
            float _lengthScalar = Mathf.InverseLerp(_foodFishable.GetMinLength(), _foodFishable.GetMaxLength(), _foodFishable.GetLength());
            float _weightScalar = Mathf.InverseLerp(_foodFishable.GetMinWeight(), _foodFishable.GetMaxWeight(), _foodFishable.GetWeight());
            if (_lengthScalar == 0) _lengthScalar = 0.5f;
            if (_weightScalar == 0) _weightScalar = 0.5f;
            currentFood += _food.GetComponent<Edible>().baseFoodAmount * (_lengthScalar * 2) * (_weightScalar * 2);
        }
    }
}
