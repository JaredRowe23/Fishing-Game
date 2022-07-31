using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fishing.FishingMechanics;

namespace Fishing.UI
{
    public class BaitInventorySlot : MonoBehaviour
    {
        public Text title;
        public Image sprite;
        public Text countText;
        public BaitScriptable baitScriptable;

        public void UpdateSlot()
        {
            title.text = baitScriptable.baitName;
            sprite.sprite = baitScriptable.inventorySprite;
        }

        public void UpdateInfoMenu()
        {
            BaitInfoMenu.instance.gameObject.SetActive(true);
            BaitInfoMenu.instance.UpdateBaitInfoMenu(baitScriptable);
        }
    }
}