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

        private RodManager rodManager;

        private RodInventorySlot()
        {
            rodManager = RodManager.instance;
        }

        public void EquipRod()
        {
            foreach (Transform _child in transform)
            {
                if (_child.GetComponent<Text>())
                {
                    rodManager.EquipRod(_child.GetComponent<Text>().text, true);
                }
            }
        }
    }

}