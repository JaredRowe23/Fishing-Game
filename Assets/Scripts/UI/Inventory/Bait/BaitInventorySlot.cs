using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fishing.FishingMechanics;
using Fishing.IO;

namespace Fishing.UI
{
    public class BaitInventorySlot : MonoBehaviour
    {
        [SerializeField] private Text title;
        [SerializeField] private Image sprite;
        [SerializeField] private Text countText;
        public BaitScriptable baitScriptable;
        public BaitSaveData baitSaveData;

        public void UpdateSlot() {
            baitScriptable = ItemLookupTable.instance.StringToBaitScriptable(baitSaveData.BaitName);
            title.text = baitSaveData.BaitName;
            sprite.sprite = baitScriptable.inventorySprite;
            countText.text = $"x{baitSaveData.Amount}";
        }

        public void UpdateInfoMenu()
        {
            BaitInfoMenu.instance.gameObject.SetActive(true);
            BaitInfoMenu.instance.UpdateBaitInfoMenu(baitScriptable);
        }
    }
}