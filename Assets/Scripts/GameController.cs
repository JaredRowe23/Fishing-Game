using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fishing.Fishables;
using Fishing.Fishables.Fish;
using Fishing.FishingMechanics;
using Fishing.UI;

namespace Fishing
{
    public class GameController : MonoBehaviour
    {
        public RodBehaviour equippedRod;
        public GameObject rodsMenu;

        private FoodSearchManager foodManager;

        public static GameController instance;

        private GameController() => instance = this;

        private void Awake()
        {
            foodManager = GetComponent<FoodSearchManager>();
        }

        public void SpawnRod(string _rodName) => rodsMenu.GetComponent<RodsMenu>().EquipRod(_rodName, false);

        public void AddFood(Edible _food) => foodManager.edibleItems.Add(_food);

        public void RemoveFood(Edible _food)
        {
            if (foodManager.edibleItems.Contains(_food)) foodManager.edibleItems.Remove(_food);
        }
        public void AddFish(FoodSearch _fish) => foodManager.fish.Add(_fish);

        public void RemoveFish(FoodSearch _fish)
        {
            if (foodManager.fish.Contains(_fish)) foodManager.fish.Remove(_fish);
        }
    }

}