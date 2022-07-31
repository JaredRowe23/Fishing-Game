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

        public void UpdateSlot()
        {
            title.text = baitScriptable.baitName;
            sprite.sprite = baitScriptable.inventorySprite;
            prefab = baitScriptable.prefab;
        }

        public void EquipBait()
        {
            for(int i = 0; i < PlayerData.instance.bait.Count; i++)
            {
                if (PlayerData.instance.baitCounts[i] == 0) continue;
                if (PlayerData.instance.bait[i] != baitScriptable.baitName) continue;

                if (RodManager.instance.equippedRod.equippedBait != null)
                {
                    PlayerData.instance.AddBait(RodManager.instance.equippedRod.equippedBait.scriptable.baitName);
                    RodManager.instance.equippedRod.equippedBait.Despawn();
                }

                for (int j = 0; j < PlayerData.instance.fishingRods.Count; j++)
                {
                    if (PlayerData.instance.fishingRods[j] != RodManager.instance.equippedRod.scriptable.rodName) continue;
                    PlayerData.instance.equippedBaits[j] = baitScriptable.baitName;
                }

                RodManager.instance.SpawnBait();
                PlayerData.instance.baitCounts[i]--;
                RodInfoMenu.instance.UpdateRodInfo(RodManager.instance.equippedRod);
            }
        }

        public void UnequipBait()
        {
            for (int i = 0; i < PlayerData.instance.bait.Count; i++)
            {
                if (RodManager.instance.equippedRod.equippedBait == null) continue;

                PlayerData.instance.AddBait(RodManager.instance.equippedRod.equippedBait.scriptable.baitName);
                RodManager.instance.equippedRod.equippedBait.Despawn();

                for (int j = 0; j < PlayerData.instance.fishingRods.Count; j++)
                {
                    if (PlayerData.instance.fishingRods[j] != RodManager.instance.equippedRod.scriptable.rodName) continue;
                    PlayerData.instance.equippedBaits[j] = "";
                }

                RodInfoMenu.instance.UpdateRodInfo(RodManager.instance.equippedRod);
            }
        }
    }
}
