using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI
{
    public class RodInventorySlot : MonoBehaviour
    {
        public Text title;
        public Image sprite;
        public GameObject equippedCheck;
        public GameObject itemReference;

        public void EquipRod()
        {
            foreach (Transform child in transform)
            {
                if (child.GetComponent<Text>())
                {
                    RodsMenu.instance.EquipRod(child.GetComponent<Text>().text, true);
                }
            }
        }
    }

}