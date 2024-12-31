using Fishing.FishingMechanics;
using Fishing.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI {
    public class BaitStoreInfo : StoreInfoPanel {
        [SerializeField] private Text _attractsText;
        [SerializeField] private List<BaitEffectsListing> _effects;

        private BaitScriptable _currentBait;

        private PlayerData _playerData;
        private TooltipSystem _tooltipSystem;

        private void Awake() {
            _playerData = SaveManager.Instance.LoadedPlayerData;
            _tooltipSystem = TooltipSystem.Instance;
        }

        public void UpdateInfo(BaitScriptable _bait) {
            gameObject.SetActive(true);

            _currentBait = _bait;

            _nameText.text = _currentBait.BaitName;
            _itemImage.sprite = _currentBait.InventorySprite;
            _descriptionText.text = _currentBait.Description;
            _costText.text = _currentBait.Cost.ToString("C");

            _attractsText.text = "";
            List<string> _foodTypes = _currentBait.GetFoodTypesAsString();
            _attractsText.text = $"{string.Join(", ", _foodTypes)}.";

            for (int i = 0; i < _effects.Count; i++) {
                _effects[i].DisableListing();
            }
            for (int i = 0; i < _currentBait.Effects.Count; i++) {
                _effects[i].UpdateEffect(_currentBait.Effects[i], _currentBait.EffectsSprites[i]);
            }
        }

        public override void PurchaseItem() {
            if (_playerData.SaveFileData.Money < _currentBait.Cost) {
                _tooltipSystem.NewTooltip("You don't have enough money to buy this bait");
                return;
            } 

            _tooltipSystem.NewTooltip($"You bought the {_currentBait.BaitName} for {_currentBait.Cost.ToString("C")}");
            _playerData.SaveFileData.Money -= _currentBait.Cost;
            _playerData.AddBait(_currentBait.BaitName, 1);
            BaitStoreMenu.Instance.RefreshStore();
            gameObject.SetActive(false);
        }

        private void OnDisable() {
            gameObject.SetActive(false);
        }
    }
}
