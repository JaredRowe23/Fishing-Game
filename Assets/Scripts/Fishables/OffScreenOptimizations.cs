using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.FishingMechanics;
using Fishing.Fishables.Fish;
using Fishing.PlayerCamera;

namespace Fishing.Fishables
{
    public class OffScreenOptimizations : MonoBehaviour
    {
        [SerializeField] private float optimizationDistance;

        private Fishable fishable;
        private FoodSearch foodSearch;
        private FoodSearchManager foodManager;
        private Edible edible;
        private FishMovement fishMovement;
        private GroundMovement groundMovement;
        private SinkingObject sinkingObject;
        private IMovement movement;
        private Hunger hunger;
        private Growth growth;

        private CameraBehaviour cam;

        private void Awake()
        {
            fishable = GetComponent<Fishable>();
            foodSearch = GetComponent<FoodSearch>();
            edible = GetComponent<Edible>();
            fishMovement = GetComponent<FishMovement>();
            groundMovement = GetComponent<GroundMovement>();
            sinkingObject = GetComponent<SinkingObject>();
            movement = GetComponent<IMovement>();
            hunger = GetComponent<Hunger>();
            growth = GetComponent<Growth>();
        }

        void Start()
        {
            foodManager = FoodSearchManager.instance;
            cam = CameraBehaviour.instance;
        }

        private void Update()
        {
            if (cam.IsInFrame(transform.position))
            {
                EndOptimizing();
                return;
            }
            float _distance = Vector2.Distance(cam.transform.position, transform.position);
            if (_distance > optimizationDistance) StartOptimizing();
            else EndOptimizing();
        }

        private void StartOptimizing()
        {
            if (foodSearch)
            {
                foodManager.RemoveFish(foodSearch);
                foodManager.RemoveFood(edible);
                hunger.enabled = false;
                growth.enabled = false;
            }
            if (fishMovement)
            {
                fishMovement.enabled = false;
                if (fishMovement.activePredator != null)
                {
                    fishMovement.activePredator.GetComponent<FoodSearch>().desiredFood = null;
                    fishMovement.activePredator = null;
                }
                if (movement is MonoBehaviour mono)
                {
                    mono.enabled = false;
                }
            }
            else if (groundMovement)
            {
                groundMovement.enabled = false;
            }
            else if (sinkingObject)
            {
                sinkingObject.enabled = false;
            }
        }

        private void EndOptimizing()
        {
            if (foodSearch)
            {
                hunger.enabled = true;
                growth.enabled = true;
                if (!foodManager.fish.Contains(foodSearch)) foodManager.AddFish(foodSearch);
                if (!foodManager.edibleItems.Contains(edible)) foodManager.AddFood(edible);
            }
            if (fishMovement)
            {
                fishMovement.enabled = true;
                if (movement is MonoBehaviour mono)
                {
                    mono.enabled = true;
                }
            }
            else if (groundMovement)
            {
                groundMovement.enabled = true;
            }
            else if (sinkingObject)
            {
                sinkingObject.enabled = true;
            }
        }
    }
}