using Fishing.IO;

namespace Fishing.UI {
    public class FishStoreListing : StoreItem {
        private BucketItemSaveData _referenceData;
        public BucketItemSaveData ReferenceData { get => _referenceData; private set { _referenceData = value; } }

        public void UpdateInfo(BucketItemSaveData data) {
            ReferenceData = data;

            _nameText.text = data.ItemName;
            _itemImage.sprite = ItemLookupTable.Instance.StringToFishScriptable(data.ItemName).InventorySprite;
            _costText.text = data.Value.ToString("C");
        }
    }
}