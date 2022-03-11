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
            foreach (Transform _child in transform)
            {
                if (_child.GetComponent<Text>())
                {
                    RodsMenu.instance.EquipRod(_child.GetComponent<Text>().text, true);
                }
            }
        }
    }

}