using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI
{
    public class InventoryMenu : MonoBehaviour
    {
        private int activeMenu = 0;
        [SerializeField] private List<GameObject> menuPages;
        [SerializeField] private List<Image> tabButtons;

        public Sprite activeTabBackground;
        public Sprite inactiveTabBackground;

        public static InventoryMenu instance;

        private InventoryMenu() => instance = this;

        private void Start()
        {
            activeMenu = 0;
            GameController.instance.rodMenuButton.GetComponent<Image>().sprite = activeTabBackground;
            GameController.instance.baitMenuButton.GetComponent<Image>().sprite = inactiveTabBackground;
            GameController.instance.gearMenuButton.GetComponent<Image>().sprite = inactiveTabBackground;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (gameObject.activeSelf)
                {
                    ShowInventoryMenu();
                }
            }
        }

        public void ShowInventoryMenu()
        {
            GameController.instance.mouseOverUI = null;

            this.gameObject.SetActive(!this.gameObject.activeSelf);

            if (!this.gameObject.activeSelf)
            {
                foreach (GameObject _page in menuPages)
                {
                    _page.SetActive(false);
                }
            }
            else
            {
                menuPages[activeMenu].SetActive(true);
            }
            GameController.instance.bucketMenuButton.gameObject.SetActive(!this.gameObject.activeSelf);
            GameController.instance.inventoryMenuButton.SetActive(!this.gameObject.activeSelf);
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