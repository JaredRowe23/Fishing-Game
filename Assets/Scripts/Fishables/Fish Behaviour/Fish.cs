using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.FishingMechanics;

namespace Fishing.Fishables.Fish
{
    public class Fish : MonoBehaviour
    {
        public float maxHomeDistance;
        public float maxHomeDistanceVariation;
        public SpawnZone spawn;

        public float swimSpeed;
        public float eatDistance;
        public FoodSearch foodSearch;
        public Vector3 targetPos;

        [Header("Hunger")]
        [SerializeField] private float hungerStart = 100;
        [SerializeField] private float decayRate = 0.1f;
        [SerializeField] private float foodStart = 50;
        [SerializeField] private float foodStartVariance = 10;
        [SerializeField] private float currentFood;

        [Header("Growth")]
        [SerializeField] private float growthStart = 75;
        [SerializeField] private float growthCheckFrequency = 15;
        [SerializeField] private float growthVariance = 0.25f;
        [SerializeField] private float growthFoodCost = 25;
        [SerializeField] private float growthCheckCount;

        private RodManager rodManager;
        private PolygonCollider2D floorCol;
        private Fishable fishable;

        private void Awake()
        {
            rodManager = RodManager.instance;
        }

        private void Start()
        {
            growthCheckCount = growthCheckFrequency;
            foodSearch = GetComponent<FoodSearch>();
            fishable = GetComponent<Fishable>();
            spawn = transform.parent.GetComponent<SpawnZone>();
            maxHomeDistance += Random.Range(-maxHomeDistanceVariation, maxHomeDistanceVariation);
            floorCol = FindObjectOfType<PolygonCollider2D>();
            currentFood = foodStart + Random.Range(-foodStartVariance, foodStartVariance);
        }

        private void Update()
        {
            currentFood -= Time.deltaTime * decayRate;
            if (currentFood <= 0) GetComponent<IEdible>().Despawn();
            else if (currentFood <= hungerStart)
            {
                if (!FoodSearchManager.instance.fish.Contains(foodSearch)) FoodSearchManager.instance.AddFish(foodSearch);
            }
            else if (FoodSearchManager.instance.fish.Contains(foodSearch)) FoodSearchManager.instance.RemoveFish(foodSearch);

            growthCheckCount -= Time.deltaTime;
            if (growthCheckCount <= 0)
            {
                if (currentFood >= growthStart) Grow();
                growthCheckCount = growthCheckFrequency;
            }
        }

        public void Eat()
        {
            GameObject _food = foodSearch.desiredFood;

            if (_food.GetComponent<HookBehaviour>())
            {
                _food.GetComponent<HookBehaviour>().SetHook(fishable);
                return;
            }

            if (_food.GetComponent<Fishable>())
            {
                if (_food.GetComponent<Fishable>().isHooked)
                {
                    SetThisToHooked();
                    return;
                }
            }
            if (_food.GetComponent<BaitBehaviour>())
            {
                SetThisToHooked();
                return;
            }

            AddFood(_food);

            GetComponent<AudioSource>().Play();
            _food.GetComponent<IEdible>().Despawn();
            foodSearch.desiredFood = null;
        }

        private void AddFood(GameObject _food)
        {
            float _lengthScalar = Mathf.InverseLerp(_food.GetComponent<Fishable>().GetMinLength(), _food.GetComponent<Fishable>().GetMaxLength(), _food.GetComponent<Fishable>().GetLength());
            float _weightScalar = Mathf.InverseLerp(_food.GetComponent<Fishable>().GetMinWeight(), _food.GetComponent<Fishable>().GetMaxWeight(), _food.GetComponent<Fishable>().GetWeight());
            if (_lengthScalar == 0) _lengthScalar = 0.5f;
            if (_weightScalar == 0) _weightScalar = 0.5f;
            currentFood += _food.GetComponent<Edible>().baseFoodAmount * (_lengthScalar * 2) * (_weightScalar * 2);
        }

        private void Grow()
        {
            fishable.SetLength(Mathf.Lerp(fishable.GetLength(), fishable.GetMaxLength(), 0.5f + Random.Range(-growthVariance, growthVariance)));
            fishable.SetWeight(Mathf.Lerp(fishable.GetWeight(), fishable.GetMaxWeight(), 0.5f + Random.Range(-growthVariance, growthVariance)));
            fishable.RecalculateValue();
            currentFood -= growthFoodCost;
        }

        public void SetThisToHooked()
        {
            GetComponent<AudioSource>().Play();
            foodSearch.desiredFood.GetComponent<IEdible>().Despawn();
            rodManager.equippedRod.GetHook().hookedObject = null;
            rodManager.equippedRod.GetHook().SetHook(GetComponent<Fishable>());
        }
        public void FaceTarget()
        {
            float angleToTarget = 0f;
            if (targetPos.x < transform.position.x)
            {
                transform.rotation = Quaternion.identity;
                angleToTarget = Vector3.SignedAngle(-transform.right, targetPos - transform.position, Vector3.forward);
            }
            else if (targetPos.x > transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                angleToTarget = -Vector3.SignedAngle(-transform.right, targetPos - transform.position, Vector3.forward);
            }

            transform.Rotate(Vector3.forward, angleToTarget);
        }
        private void OnDrawGizmosSelected()
        {
            if (!GetComponent<Fishable>().isHooked)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.parent.position, maxHomeDistance);
                //if (foodSearch.desiredFood == null)
                //{
                //    Debug.DrawRay(transform.position, targetPos - transform.position, Color.green);
                //}
            }
        }
    }
}
