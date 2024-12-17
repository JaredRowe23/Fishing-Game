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
        private BaitManager _baitManager;

        private void Awake()
        {
            playerData = SaveManager.Instance.LoadedPlayerData;
            rodManager = RodManager.Instance;
            _baitManager = BaitManager.Instance;
        }

        public void UpdateSlot() {
            baitScriptable = ItemLookupTable.instance.StringToBaitScriptable(baitSaveData.BaitName);
            title.text = baitSaveData.BaitName;
            sprite.sprite = baitScriptable.inventorySprite;
            countText.text = $"x{baitSaveData.Amount}";
        }

        public void EquipBait()
        {
            UnequipCurrentBait();

            playerData.EquippedRod.EquippedBait = baitSaveData;
            _baitManager.SpawnBait();
            playerData.EquippedRod.EquippedBait.Amount--;

            RodInfoMenu.instance.UpdateRodInfo(rodManager.EquippedRod);
        }

        private void UnequipCurrentBait()
        {
            if (string.IsNullOrEmpty(playerData.EquippedRod.EquippedBait?.BaitName)) return;

            playerData.EquippedRod.EquippedBait.Amount++;
            Destroy(rodManager.EquippedRod.EquippedBait);
            playerData.EquippedRod.EquippedBait = null;
        }

        public void UnequipSelectedBait()
        {
            UnequipCurrentBait();
            RodInfoMenu.instance.UpdateRodInfo(RodManager.Instance.EquippedRod);
        }
    }
}
