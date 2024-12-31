using Fishing.FishingMechanics;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI {
    public class BaitStoreListing : StoreItem {
        private BaitScriptable _referenceScriptable;
        public BaitScriptable ReferenceScriptable { get => _referenceScriptable; private set { _referenceScriptable = value; } }

        [SerializeField, Tooltip("Text UI that displays the list of fish types this bait attracts.")] private Text _attractsText;
        [SerializeField, Tooltip("Text UI that displays the effects this bait has.")] private Text _effectsText;

        public void UpdateInfo(BaitScriptable bait) {
            ReferenceScriptable = bait;

            _nameText.text = ReferenceScriptable.BaitName;
            _costText.text = ReferenceScriptable.Cost.ToString("C");
            if (ReferenceScriptable.GetFoodTypesAsString() != null) _attractsText.text = $"Attracts: x{bait.GetFoodTypesAsString().Count}";
            _effectsText.text = $"Effects: x{ReferenceScriptable.Effects.Count}";
            _itemImage.sprite = ReferenceScriptable.InventorySprite;

            Availability = ItemAvailablility.Available;
        }
    }
}
