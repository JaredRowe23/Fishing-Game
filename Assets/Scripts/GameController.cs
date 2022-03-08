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
        public Button bucketMenuButton;
        public GameObject mouseOverUI;
        public GameObject itemInfoMenu;
        public GameObject overflowItem;
        public GameObject itemViewer;
        public Canvas rodCanvas;
        public GameObject inventoryMenuButton;
        public GameObject rodMenuButton;
        public GameObject baitMenuButton;
        public GameObject gearMenuButton;
        public RodBehaviour equippedRod;
        public GameObject rodsMenu;

        [SerializeField] private List<GameObject> interuptableUI;

        private FoodSearchManager foodManager;

        public static GameController instance;

        private GameController() => instance = this;

        private void Awake()
        {
            foodManager = GetComponent<FoodSearchManager>();
        }


        public bool IsActiveUI()
        {
            foreach (GameObject ui in interuptableUI)
            {
                if (ui.activeSelf)
                {
                    return true;
                }
            }
            return false;
        }

        public void SpawnRod(string _rodName) => rodsMenu.GetComponent<RodsMenu>().EquipRod(_rodName);

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