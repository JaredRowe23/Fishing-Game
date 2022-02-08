using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GearMenu : MonoBehaviour
{
    private bool initialized;

    public void ShowRodMenu()
    {
        this.gameObject.SetActive(!this.gameObject.activeSelf);
        InventoryMenu.instance.UpdateActiveMenu(2);
    }
}
