﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaitMenu : MonoBehaviour
{
    private bool initialized;

    public void ShowRodMenu()
    {
        this.gameObject.SetActive(!this.gameObject.activeSelf);
        GameController.instance.inventoryMenu.UpdateActiveMenu(1);
    }
}