using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fishing.IO;
using System;

namespace Fishing.UI
{
    public class InventoryMenu : MonoBehaviour
    {
        [SerializeField] private List<InventoryTab> inventoryTabs;
        [SerializeField] private List<GameObject> setDisabledOnClose;

        public Color activeTabColor;
        public Color inactiveTabColor;

        public static InventoryMenu instance;

        private InventoryMenu() => instance = this;

        private void Start() {
            UpdateActiveMenu(inventoryTabs[0].menuObject);
        }

        public void ResetActiveMenu() {
            for (int i = 0; i < inventoryTabs.Count; i++) {
                if (i == 0) {
                    SetActiveMenu(inventoryTabs[i]);
                }
                else {
                    SetInactiveMenu(inventoryTabs[i]);
                }
            }
        }

        public void ToggleInventoryMenu()
        {
            UIManager.instance.mouseOverUI = null;

            if (!gameObject.activeSelf) {
                ShowInventoryMenu();
            }
            else {
                HideInventoryMenu();
            }
        }

        public void ShowInventoryMenu() {
            UIManager.instance.mouseOverUI = null;
            gameObject.SetActive(true);

            if (BucketMenu.instance.gameObject.activeSelf) {
                BucketMenu.instance.ToggleBucketMenu();
            }

            if (PlayerData.instance.hasSeenTutorialData.inventoryTutorial) return;
            TutorialSystem.instance.QueueTutorial("Here, you can view attachment slots for your fishing rod (line, bait, and hook). You can also take inventory of bait you have and equip bits of gear (TBD)");
            PlayerData.instance.hasSeenTutorialData.inventoryTutorial = true;

            UIManager.instance.HideHUDButtons();
        }

        public void HideInventoryMenu() {
            UIManager.instance.mouseOverUI = null;
            gameObject.SetActive(false);

            foreach (GameObject _obj in setDisabledOnClose) {
                _obj.SetActive(false);
            }

            UIManager.instance.ShowHUDButtons();
        }

        public void UpdateActiveMenu(GameObject _menu)
        {
            for (int i = 0; i < inventoryTabs.Count; i++) {
                InventoryTab _tab = inventoryTabs[i];
                if (_menu == _tab.menuObject) {
                    SetActiveMenu(_tab);
                }
                else {
                    SetInactiveMenu(_tab);
                }
            }
        }

        private void SetActiveMenu(InventoryTab _tab) {
            if (!_tab.isActiveMenu) {
                AudioManager.instance.PlaySound("Inventory Tab");
            }

            _tab.tabButton.transform.SetAsLastSibling();
            _tab.isActiveMenu = true;
            _tab.menuObject.SetActive(true);
            _tab.tabButton.image.color = activeTabColor;

            _tab.menuObject.GetComponent<IInventoryTab>().ShowTab();
        }

        private void SetInactiveMenu(InventoryTab _tab) {
            _tab.tabButton.transform.SetAsFirstSibling();
            _tab.isActiveMenu = false;
            _tab.menuObject.SetActive(false);
            _tab.tabButton.image.color = inactiveTabColor;
            _tab.menuObject.GetComponent<IInventoryTab>().HideTab();
        }
    }
}