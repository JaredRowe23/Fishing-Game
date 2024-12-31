using Fishing.IO;
using Fishing.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI {
    public class FishStoreInfo : StoreInfoPanel {
        [SerializeField, Tooltip("Text UI that displays the selected fishable's weight.")] private Text _weightText;
        [SerializeField, Tooltip("Text UI that displays the selected fishable's length.")] private Text _lengthText;

        private BucketItemSaveData _selectedItem;

        private TooltipSystem _tooltipSystem;
        private PlayerData _playerData;

        private void Start() {
            _tooltipSystem = TooltipSystem.Instance;
            _playerData = SaveManager.Instance.LoadedPlayerData;
        }

        public void UpdateInfo(BucketItemSaveData data) {
            gameObject.SetActive(true);

            _selectedItem = data;

            _nameText.text = _selectedItem.ItemName;
            _descriptionText.text = _selectedItem.Description;
            _itemImage.sprite = ItemLookupTable.Instance.StringToFishScriptable(_selectedItem.ItemName).InventorySprite;
            _costText.text = _selectedItem.Value.ToString("C");

            _weightText.text = _selectedItem.Weight.ToString("F2");
            _lengthText.text = _selectedItem.Length.ToString("F2");
        }

        public override void PurchaseItem() { // TODO:  rename this to something like "make deal", since purchase doesn't apply when the player is selling the item.
            _tooltipSystem.NewTooltip($"You bought the {_selectedItem.ItemName} for {_selectedItem.Value.ToString("C")}");

            _playerData.SaveFileData.Money += _selectedItem.Value;
            _playerData.BucketItemSaveData.Remove(_selectedItem);
            FishStoreMenu.Instance.RefreshStore();
            gameObject.SetActive(false);
        }

        private void OnDisable() {
            gameObject.SetActive(false);
        }
    }
}