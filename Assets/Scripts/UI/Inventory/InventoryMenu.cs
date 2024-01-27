using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fishing.IO;

namespace Fishing.UI
{
    public class InventoryMenu : MonoBehaviour
    {
        private int activeMenu = 0;
        [SerializeField] private List<GameObject> menuPages;
        [SerializeField] private List<GameObject> setDisabledOnClose;
        [SerializeField] private List<Image> tabButtons;

        public Sprite activeTabBackground;
        public Sprite inactiveTabBackground;

        public static InventoryMenu instance;

        private InventoryMenu() => instance = this;

        private void Start()
        {
            activeMenu = 0;
            UIManager.instance.rodMenuButton.GetComponent<Image>().sprite = activeTabBackground;
            UIManager.instance.baitMenuButton.GetComponent<Image>().sprite = inactiveTabBackground;
            UIManager.instance.gearMenuButton.GetComponent<Image>().sprite = inactiveTabBackground;
        }

        public void ToggleInventoryMenu()
        {
            UIManager.instance.mouseOverUI = null;

            gameObject.SetActive(!gameObject.activeSelf);

            if (!gameObject.activeSelf)
            {
                foreach(GameObject _obj in setDisabledOnClose)
                {
                    _obj.SetActive(false);
                }
            }
            else
            {
                if (BucketMenu.instance.gameObject.activeSelf) BucketMenu.instance.ToggleBucketMenu();
                menuPages[activeMenu].SetActive(true);

                if (PlayerData.instance.hasSeenInventoryTut) return;
                TutorialSystem.instance.QueueTutorial("Here, you can view attachment slots for your fishing rod (line, bait, and hook). You can also take inventory of bait you have and equip bits of gear (TBD)");
                PlayerData.instance.hasSeenInventoryTut = true;
            }
            UIManager.instance.bucketMenuButton.gameObject.SetActive(!gameObject.activeSelf);
            UIManager.instance.inventoryMenuButton.SetActive(!gameObject.activeSelf);
        }

        public void UpdateActiveMenu(int _menu)
        {
            if (_menu != activeMenu)
            {
                AudioManager.instance.PlaySound("Inventory Tab");
                foreach (Image _tab in tabButtons)
                {
                    _tab.transform.SetAsFirstSibling();
                }

                menuPages[activeMenu].SetActive(false);
                tabButtons[activeMenu].sprite = inactiveTabBackground;

                activeMenu = _menu;
                menuPages[activeMenu].SetActive(true);
                tabButtons[activeMenu].sprite = activeTabBackground;
                tabButtons[activeMenu].transform.SetAsLastSibling();
            }
        }
    }

}