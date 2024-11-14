using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fishing.FishingMechanics;
using Fishing.IO;

namespace Fishing.UI
{
    public class BaitAttachmentSlot : MonoBehaviour
    {
        [SerializeField] private Text title;
        [SerializeField] private Image sprite;
        [SerializeField] private Text countText;
        public BaitScriptable baitScriptable;
        public BaitSaveData baitSaveData;

        private PlayerData playerData;
        private RodManager rodManager;

        private void Awake()
        {
            playerData = PlayerData.instance;
            rodManager = RodManager.instance;
        }

        public void UpdateSlot() {
            baitScriptable = ItemLookupTable.instance.StringToBait(baitSaveData.baitName);
            title.text = baitSaveData.baitName;
            sprite.sprite = baitScriptable.inventorySprite;
            countText.text = $"x{baitSaveData.amount}";
        }

        public void EquipBait()
        {
            UnequipCurrentBait();

            playerData.equippedRod.equippedBait = baitSaveData;
            rodManager.SpawnBait();
            playerData.equippedRod.equippedBait.amount--;

            RodInfoMenu.instance.UpdateRodInfo(rodManager.equippedRod);
        }

        private void UnequipCurrentBait()
        {
            if (string.IsNullOrEmpty(playerData.equippedRod.equippedBait?.baitName)) return;

            playerData.equippedRod.equippedBait.amount++;
            rodManager.equippedRod.equippedBait.Despawn();
            playerData.equippedRod.equippedBait = null;
        }

        public void UnequipSelectedBait()
        {
            UnequipCurrentBait();
            RodInfoMenu.instance.UpdateRodInfo(RodManager.instance.equippedRod);
        }
    }
}
