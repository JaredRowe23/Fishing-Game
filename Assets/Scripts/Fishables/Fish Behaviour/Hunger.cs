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
        private IEdible edible;

        private void Awake()
        {
            fishable = GetComponent<Fishable>();
            edible = GetComponent<IEdible>();
        }

        void Start()
        {
            currentFood = foodStart + Random.Range(-foodStartVariance, foodStartVariance);
        }

        void Update()
        {
            if (fishable.IsHooked) return;

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
            if (currentFood <= hungerStart) {
                // TODO: Start searching
            }
            else {
                // TODO: Stop searching
            }
        }

        public void AddFood(GameObject _food)
        {
            Fishable _foodFishable = _food.GetComponent<Fishable>();
            float _lengthScalar = Mathf.InverseLerp(_foodFishable.LengthMin, _foodFishable.LengthMax, _foodFishable.Length);
            float _weightScalar = Mathf.InverseLerp(_foodFishable.WeightMin, _foodFishable.WeightMax, _foodFishable.Weight);
            if (_lengthScalar == 0) _lengthScalar = 0.5f;
            if (_weightScalar == 0) _weightScalar = 0.5f;
            currentFood += _food.GetComponent<Edible>().baseFoodAmount * (_lengthScalar * 2) * (_weightScalar * 2);
        }
    }
}
