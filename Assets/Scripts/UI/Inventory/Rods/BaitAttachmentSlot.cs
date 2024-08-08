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
        public Text title;
        public Image sprite;
        public Text countText;
        public BaitScriptable baitScriptable;
        public GameObject prefab;

        private PlayerData playerData;
        private RodManager rodManager;

        private void Awake()
        {
            playerData = PlayerData.instance;
            rodManager = RodManager.instance;
        }

        public void UpdateSlot()
        {
            title.text = baitScriptable.baitName;
            sprite.sprite = baitScriptable.inventorySprite;
            prefab = baitScriptable.prefab;
        }

        public void EquipBait()
        {
            DespawnBaitIfEquipped();

            for (int i = 0; i < playerData.baitSaveData.Count; i++)
            {
                if (playerData.baitSaveData[i].baitName != baitScriptable.baitName) continue;

                playerData.equippedRod.equippedBait = playerData.baitSaveData[i];
            }

            rodManager.SpawnBait();
            playerData.equippedRod.equippedBait.amount--;

            RodInfoMenu.instance.UpdateRodInfo(rodManager.equippedRod);
        }

        private void DespawnBaitIfEquipped()
        {
            if (playerData.equippedRod.equippedBait == null) return;
            if (playerData.equippedRod.equippedBait.baitName == null) return;
            if (playerData.equippedRod.equippedBait.baitName == "") return;

            playerData.equippedRod.equippedBait.amount++;
            rodManager.equippedRod.equippedBait.Despawn();
        }

        public void UnequipBait()
        {
            if (playerData.equippedRod.equippedBait == null) return;
            if (playerData.equippedRod.equippedBait.baitName == null) return;

            playerData.equippedRod.equippedBait.amount++;
            rodManager.equippedRod.equippedBait.Despawn();

            playerData.equippedRod.equippedBait = null;

            RodInfoMenu.instance.UpdateRodInfo(RodManager.instance.equippedRod);
        }
    }
}
