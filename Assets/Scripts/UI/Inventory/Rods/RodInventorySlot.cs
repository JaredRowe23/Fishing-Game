using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fishing.FishingMechanics;

namespace Fishing.UI
{
    public class RodInventorySlot : MonoBehaviour
    {
        public Text title;
        public Image sprite;
        public GameObject equippedCheck;
        public GameObject itemReference;

        private RodManager rodManager;

        private void Awake()
        {
            rodManager = RodManager.instance;
        }

        public void UpdateInfoMenu()
        {
            RodInfoMenu.instance.gameObject.SetActive(true);
            RodInfoMenu.instance.UpdateRodInfo(rodManager.equippedRod.GetComponent<RodBehaviour>());
        }

        public void EquipRod()
        {
            foreach (Transform _child in transform)
            {
                if (_child.GetComponent<Text>())
                {
                    rodManager.EquipRod(itemReference.GetComponent<RodBehaviour>().scriptable.rodName, true);
                }
            }
        }
    }

}