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
        public float currentFood;

        private FoodSearch foodSearch;

        private void Awake()
        {
            foodSearch = GetComponent<FoodSearch>();
        }

        void Start() => currentFood = foodStart + Random.Range(-foodStartVariance, foodStartVariance);

        void Update()
        {
            if (foodSearch.desiredFood)
            {
                if (Vector2.Distance(transform.position, foodSearch.desiredFood.transform.position) <= foodSearch.eatDistance) foodSearch.Eat();
            }
            currentFood -= Time.deltaTime * decayRate;
            if (currentFood <= 0) GetComponent<IEdible>().Despawn();
            else if (currentFood <= hungerStart)
            {
                if (!FoodSearchManager.instance.fish.Contains(foodSearch)) FoodSearchManager.instance.AddFish(foodSearch);
            }
            else if (FoodSearchManager.instance.fish.Contains(foodSearch)) FoodSearchManager.instance.RemoveFish(foodSearch);
        }
        public void AddFood(GameObject _food)
        {
            float _lengthScalar = Mathf.InverseLerp(_food.GetComponent<Fishable>().GetMinLength(), _food.GetComponent<Fishable>().GetMaxLength(), _food.GetComponent<Fishable>().GetLength());
            float _weightScalar = Mathf.InverseLerp(_food.GetComponent<Fishable>().GetMinWeight(), _food.GetComponent<Fishable>().GetMaxWeight(), _food.GetComponent<Fishable>().GetWeight());
            if (_lengthScalar == 0) _lengthScalar = 0.5f;
            if (_weightScalar == 0) _weightScalar = 0.5f;
            currentFood += _food.GetComponent<Edible>().baseFoodAmount * (_lengthScalar * 2) * (_weightScalar * 2);
        }
    }
}
