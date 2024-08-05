using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.Fishables.Fish
{
    [RequireComponent(typeof(Hunger))]
    public class Growth : MonoBehaviour
    {
        [SerializeField] private float growthStart = 75;
        [SerializeField] private float growthCheckFrequency = 15;
        [SerializeField] private float growthVariance = 0.25f;
        [SerializeField] private float growthFoodCost = 25;
        private float growthCheckCount;

        private Fishable fishable;
        private Hunger hunger;

        private void Awake()
        {
            fishable = GetComponent<Fishable>();
            hunger = GetComponent<Hunger>();
        }

        void Start() => growthCheckCount = growthCheckFrequency;

        void Update()
        {
            if (fishable.isHooked) return;
            HandleGrowth();
        }

        private void HandleGrowth()
        {
            growthCheckCount -= Time.deltaTime;
            if (growthCheckCount <= 0)
            {
                if (hunger.currentFood >= growthStart) Grow();
                growthCheckCount = growthCheckFrequency;
            }
        }

        private void Grow()
        {
            fishable.SetLength(Mathf.Lerp(fishable.GetLength(), fishable.GetMaxLength(), 0.5f + Random.Range(-growthVariance, growthVariance)));
            fishable.SetWeight(Mathf.Lerp(fishable.GetWeight(), fishable.GetMaxWeight(), 0.5f + Random.Range(-growthVariance, growthVariance)));
            fishable.AdjustValueAndDifficulty();
            hunger.currentFood -= growthFoodCost;
        }
    }
}
