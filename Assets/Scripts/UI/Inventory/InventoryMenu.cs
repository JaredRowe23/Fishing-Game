using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour
{
    private int activeMenu = 0;
    [SerializeField] private List<GameObject> menuPages;
    [SerializeField] private List<Image> tabButtons;

    public Sprite activeTabBackground;
    public Sprite inactiveTabBackground;

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
        if (GameController.instance.mouseOverUI == this.gameObject)
        {
            GameController.instance.mouseOverUI = null;
        }

        this.gameObject.SetActive(!this.gameObject.activeSelf);

        if (!this.gameObject.activeSelf)
        {
            foreach(GameObject page in menuPages)
            {
                page.SetActive(false);
            }
        }
        else
        {
            menuPages[activeMenu].SetActive(true);
        }
        GameController.instance.bucketMenuButton.gameObject.SetActive(!this.gameObject.activeSelf);
        GameController.instance.inventoryMenuButton.gameObject.SetActive(!this.gameObject.activeSelf);
    }

    public void UpdateActiveMenu(int menu)
    {
        if (menu != activeMenu)
        {
            foreach(Image tab in tabButtons)
            {
                tab.transform.SetAsFirstSibling();
            }

            menuPages[activeMenu].SetActive(false);
            tabButtons[activeMenu].sprite = inactiveTabBackground;

            activeMenu = menu;
            menuPages[activeMenu].SetActive(true);
            tabButtons[activeMenu].sprite = activeTabBackground;
            tabButtons[activeMenu].transform.SetAsLastSibling();
        }
    }
}
