using UnityEngine;

namespace Fishing.Fishables.Fish {
    [RequireComponent(typeof(FoodSearch))]
    public class Hunger : MonoBehaviour {
        [SerializeField, Min(0), Tooltip("Food value this must be below to begin searching for food.")] private float _hungerStart = 100;
        [SerializeField, Min(0), Tooltip("Amount of food per second this loses over time.")] private float _decayRate = 0.1f;
        [SerializeField, Min(0), Tooltip("Food value this starts with.")] private float _foodStart = 50;
        [SerializeField, Min(0), Tooltip("Food value range above or below food start this starts with.")] private float _foodStartVariance = 10;
        private float _currentFood;
        public float CurrentFood { get => _currentFood; set => _currentFood = value; }

        private Fishable _fishable;
        private IEdible _edible;

        private void OnValidate() {
            if (_foodStartVariance > _foodStart) {
                _foodStartVariance = _foodStart;
            }
        }

        private void Awake() {
            _fishable = GetComponent<Fishable>();
            _edible = GetComponent<IEdible>();
        }

        void Start() {
            CurrentFood = _foodStart + Random.Range(-_foodStartVariance, _foodStartVariance);
        }

        void FixedUpdate() {
            if (_fishable.IsHooked) { 
                return;
            }

            CurrentFood -= Time.fixedDeltaTime * _decayRate;
            if (CurrentFood <= 0) { 
                Starve();
            }
            else { 
                HandleHunger();
            }
        }

        private void Starve() {
            _edible.Despawn();
        }

        private void HandleHunger() {
            if (CurrentFood <= _hungerStart) {
                // TODO: Start searching
            }
            else {
                // TODO: Stop searching
            }
        }

        public void AddFood(Edible food) {
            Fishable foodFishable = food.GetComponent<Fishable>();
            float lengthScalar = Mathf.InverseLerp(foodFishable.LengthMin, foodFishable.LengthMax, foodFishable.Length) + 0.5f;
            float weightScalar = Mathf.InverseLerp(foodFishable.WeightMin, foodFishable.WeightMax, foodFishable.Weight) + 0.5f;
            float scalarAverage = (lengthScalar + weightScalar) * 0.5f;
            CurrentFood += food.baseFoodAmount * scalarAverage;
        }
    }
}
