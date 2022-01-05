using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Text title;
    public Image sprite;
    public GameObject equippedCheck;
    public GameObject itemReference;

    // Start is called before the first frame update
    public void EquipRod()
    {
        foreach(Transform child in transform)
        {
            if (child.GetComponent<Text>())
            {
                GameController.instance.rodsMenu.EquipRod(child.GetComponent<Text>().text);
            }
        }
    }
}
