using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.IO;

namespace Fishing.UI
{
    public class RodsMenu : MonoBehaviour, IInventoryTab
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

        public void ShowRodMenu() {
            InventoryMenu.instance.UpdateActiveMenu(gameObject);
        }

        public void ShowTab() {
            DestroySlots();
            GenerateSlots();
        }

        public void HideTab() {
            rodInfoMenu.HideAttachmentScrollRects();
        }

        public void GenerateSlots()
        {
            for (int i = 0; i < playerData.fishingRodSaveData.Count; i++)
            {
                RodInventorySlot _newSlot = Instantiate(slotPrefab, content.transform).GetComponent<RodInventorySlot>();

                _newSlot.Title.text = playerData.fishingRodSaveData[i].rodName;

                for (int j = 0; j < rodManager.rodPrefabs.Count; j++)
                {
                    if (rodManager.rodPrefabs[i].name != playerData.fishingRodSaveData[i].rodName) continue;

                    _newSlot.itemReference = rodManager.rodPrefabs[i];
                    _newSlot.Sprite.sprite = rodManager.rodSprites[i];
                    break;
                }

                UpdateEquippedCheckmark();
            }
        }

        private void DestroySlots() {
            foreach (Transform _child in content.transform) {
                Destroy(_child.gameObject);
            }
        }

        public void UpdateEquippedCheckmark()
        {
            foreach (Transform _slot in content.transform)
            {
                RodInventorySlot _invSlot = _slot.GetComponent<RodInventorySlot>();
                if (_invSlot.itemReference.name == playerData.equippedRod.rodName)
                {
                    _invSlot.EquippedCheck.SetActive(true);
                }
                else
                {
                    _invSlot.EquippedCheck.SetActive(false);
                }
            }
        }
    }
}