using Fishing.FishingMechanics;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI {
    public class RodStoreListing : StoreItem {
        private RodScriptable _referenceScriptable;
        public RodScriptable ReferenceScriptable { get => _referenceScriptable; private set { _referenceScriptable = value; } }

        public void UpdateInfo (RodScriptable rod) {
            ReferenceScriptable = rod;
            _nameText.text = rod.RodName;
            _costText.text = rod.Cost.ToString("C");
            _itemImage.sprite = rod.InventorySprite;
        }
    }
}
