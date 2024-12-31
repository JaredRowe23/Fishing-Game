using Fishing.FishingMechanics;
using Fishing.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI {
    public class RodStoreInfo : StoreInfoPanel {
        [SerializeField, Tooltip("Text UI that displays the reel speed of the selected fishing rod.")] private Text _reelSpeedText;
        [SerializeField, Tooltip("Text UI that displays the cast strength of the selected fishing rod.")] private Text _castStrengthText;
        [SerializeField, Tooltip("Text UI that displays the cast angle of the selected fishing rod.")] private Text _castAngleText;
        [SerializeField, Tooltip("Text UI that displays the line length of the selected fishing rod.")] private Text _lineLengthText;
        [SerializeField, Tooltip("Text UI that displays the strength frequency of the selected fishing rod.")] private Text _strengthFrequencyText;
        [SerializeField, Tooltip("Text UI that displays the angle frequency of the selected fishing rod.")] private Text _angleFrequencyText;

        private RodScriptable _currentRodScriptable;

        private PlayerData _playerData;
        private TooltipSystem _tooltipSystem;

        private void Awake() {
            _playerData = SaveManager.Instance.LoadedPlayerData;
            _tooltipSystem = TooltipSystem.instance;
        }

        public void UpdateInfo(RodScriptable rod) {
            gameObject.SetActive(true);

            _currentRodScriptable = rod;

            _nameText.text = _currentRodScriptable.RodName;
            _itemImage.sprite = _currentRodScriptable.InventorySprite;
            _descriptionText.text = _currentRodScriptable.Description;
            _costText.text = _currentRodScriptable.Cost.ToString("C");
            _reelSpeedText.text = _currentRodScriptable.ReelForce.ToString();
            _castStrengthText.text = $"{_currentRodScriptable.MinCastStrength} / {_currentRodScriptable.MaxCastStrength}";
            _castAngleText.text = _currentRodScriptable.MaxCastAngle.ToString();
            _lineLengthText.text = _currentRodScriptable.LineLength.ToString();
            _strengthFrequencyText.text = _currentRodScriptable.ChargeFrequency.ToString();
            _angleFrequencyText.text = _currentRodScriptable.AngleFrequency.ToString();
        }

        public override void PurchaseItem() {
            if (_playerData.SaveFileData.Money < _currentRodScriptable.Cost) {
                _tooltipSystem.NewTooltip(5f, "You don't have enough money to buy this fishing rod");
                return;
            }

            _tooltipSystem.NewTooltip(5f, $"You bought the {_currentRodScriptable.RodName} for {_currentRodScriptable.Cost.ToString("C")}");
            _playerData.SaveFileData.Money -= _currentRodScriptable.Cost;
            _playerData.FishingRodSaveData.Add(new FishingRodSaveData(_currentRodScriptable.RodName));
            RodStoreMenu.Instance.RefreshStore();
            gameObject.SetActive(false);
        }

        private void OnDisable() {
            gameObject.SetActive(false);
        }
    }
}
