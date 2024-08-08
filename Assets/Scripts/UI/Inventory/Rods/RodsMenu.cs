using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.IO;

namespace Fishing.UI
{
    public class RodsMenu : MonoBehaviour
    {
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private GameObject content;
        [SerializeField] private RodInfoMenu rodInfoMenu;

        private PlayerData playerData;
        private RodManager rodManager;

        public static RodsMenu instance;

        private RodsMenu() => instance = this;

        private void Awake()
        {
            rodManager = RodManager.instance;
            playerData = PlayerData.instance;
        }


        private void Start() => GenerateSlots();

        public void ShowRodMenu(bool _active)
        {
            gameObject.SetActive(_active);
            InventoryMenu.instance.UpdateActiveMenu(0);
            GenerateSlots();
            if (!gameObject.activeSelf) rodInfoMenu.HideButtonScrollViews();
        }

        public void GenerateSlots()
        {
            foreach (Transform _child in content.transform)
            {
                Destroy(_child.gameObject);
            }

            for (int i = 0; i < playerData.fishingRodSaveData.Count; i++)
            {
                RodInventorySlot _newSlot = Instantiate(slotPrefab, content.transform).GetComponent<RodInventorySlot>();

                _newSlot.title.text = playerData.fishingRodSaveData[i].rodName;

                for (int j = 0; j < rodManager.rodPrefabs.Count; j++)
                {
                    if (rodManager.rodPrefabs[i].name != playerData.fishingRodSaveData[i].rodName) continue;

                    _newSlot.itemReference = rodManager.rodPrefabs[i];
                    _newSlot.sprite.sprite = rodManager.rodSprites[i];
                    break;
                }

                UpdateEquippedRod();
            }
        }

        public void UpdateEquippedRod()
        {
            foreach (Transform _slot in content.transform)
            {
                RodInventorySlot _invSlot = _slot.GetComponent<RodInventorySlot>();
                if (_invSlot.itemReference.name == playerData.equippedRod.rodName)
                {
                    _invSlot.equippedCheck.SetActive(true);
                }
                else
                {
                    _invSlot.equippedCheck.SetActive(false);
                }
            }
        }
    }
}