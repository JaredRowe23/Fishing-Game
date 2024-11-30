using System.Collections;
using UnityEngine;

namespace Fishing.Fishables.Fish {
    [RequireComponent(typeof(Hunger))]
    public class Growth : MonoBehaviour {
        [SerializeField] private float _growthStart = 75;
        [SerializeField] private float _growthCheckFrequency = 15;
        [SerializeField] private float _growthVariance = 0.25f;
        [SerializeField] private float _growthFoodCost = 25;

        private Fishable _fishable;
        private Hunger _hunger;

        private void Awake() {
            _fishable = GetComponent<Fishable>();
            _hunger = GetComponent<Hunger>();
        }

        void Start() {
            StartCoroutine(Co_CheckForGrowth());
        }

        private IEnumerator Co_CheckForGrowth() {
            while (true) {
                if (_fishable.IsHooked) yield return new WaitForSeconds(_growthCheckFrequency);
                if (_hunger.CurrentFood >= _growthStart) {
                    Grow();
                }
                yield return new WaitForSeconds(_growthCheckFrequency);
            }
        }

        private void Grow() {
            _fishable.Length = Mathf.Lerp(_fishable.Length, _fishable.LengthMax, 0.5f + Random.Range(-_growthVariance, _growthVariance));
            _fishable.Weight = Mathf.Lerp(_fishable.Weight, _fishable.WeightMax, 0.5f + Random.Range(-_growthVariance, _growthVariance));
            _hunger.CurrentFood -= _growthFoodCost;
        }
    }
}
