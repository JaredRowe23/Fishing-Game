using UnityEngine;
using UnityEngine.UI;
using Fishing.IO;
using Fishing.FishingMechanics;

namespace Fishing.UI {
    public abstract class BaitUI : MonoBehaviour {
        [SerializeField, Tooltip("Text UI that displays the title of this bait.")] protected Text _title;
        [SerializeField, Tooltip("Image UI that displays the sprite of this bait.")] protected Image _sprite;
        [SerializeField, Tooltip("Text UI that displays the amount of this bait the player has.")] protected Text _countText;

        protected BaitScriptable _baitScriptable;
        protected BaitSaveData _baitSaveData;

        public void UpdateSlot(BaitSaveData data) {
            _baitSaveData = data;
            _baitScriptable = ItemLookupTable.Instance.StringToBaitScriptable(_baitSaveData.BaitName);

            _title.text = _baitSaveData.BaitName;
            _sprite.sprite = _baitScriptable.InventorySprite;
            _countText.text = $"x{_baitSaveData.Amount}";
        }
    }
}
