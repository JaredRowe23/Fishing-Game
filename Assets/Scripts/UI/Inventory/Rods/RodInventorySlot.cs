using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fishing.FishingMechanics;

namespace Fishing.UI
{
    public class RodInventorySlot : MonoBehaviour
    {
        [SerializeField] private Text title;
        public Text Title { get { return title; } private set { } }
        [SerializeField] private Image sprite;
        public Image Sprite { get { return sprite; } private set { } }
        [SerializeField] private GameObject equippedCheck;
        public GameObject EquippedCheck { get { return equippedCheck; } private set { } }
        public GameObject itemReference;

        private RodManager rodManager;

        private void Awake()
        {
            rodManager = RodManager.instance;
        }

        public void UpdateInfoMenu()
        {
            if (!RodInfoMenu.instance.gameObject.activeSelf) {
                RodInfoMenu.instance.gameObject.SetActive(true);
            }
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