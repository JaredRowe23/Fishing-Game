using Fishing.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI {
    public class RodInventorySlot : MonoBehaviour {
        [SerializeField, Tooltip("Text UI that displays this fishing rod's name.")] private Text _fishingRodNameText;
        public Text FishingRodNameText { get { return _fishingRodNameText; } private set { } }
        [SerializeField, Tooltip("Image UI that displays this fishing rod's sprite.")] private Image _fishingRodSprite;
        public Image FishingRodSprite { get { return _fishingRodSprite; } private set { } }
        [SerializeField, Tooltip("Image UI that displays a checkmark for if this fishing rod is equipped or not.")] private Image _equippedCheckmark;
        public Image EquippedCheckmark { get { return _equippedCheckmark; } private set { } }

        private FishingRodSaveData _fishingRodData;
        public FishingRodSaveData FishingRodData { get => _fishingRodData; private set { _fishingRodData = value; } }

        private RodManager _rodManager;
        private RodInfoMenu _rodInfoMenu;
        private PlayerData _playerData;
        private TooltipSystem _tooltipSystem;

        private void Awake() {
            _rodManager = RodManager.Instance;
            _rodInfoMenu = RodInfoMenu.Instance;
            _playerData = SaveManager.Instance.LoadedPlayerData;
            _tooltipSystem = TooltipSystem.Instance;
        }

        public void UpdateSlot(FishingRodSaveData data) {
            FishingRodData = data;
            FishingRodNameText.text = data.RodName;
            FishingRodSprite.sprite = ItemLookupTable.Instance.StringToRodScriptable(data.RodName).InventorySprite;
            UpdateEquippedCheckmark();

        }

        public void UpdateEquippedCheckmark() {
            if (_playerData.EquippedRod.RodName == FishingRodData.RodName) {
                EquippedCheckmark.enabled = true;
            }
            else {
                EquippedCheckmark.enabled = false;
            }
        }

        public void UpdateInfoMenu() {
            _rodInfoMenu.gameObject.SetActive(true);
        }

        public void EquipRod() {
            _playerData.EquippedRod = FishingRodData;
            _rodManager.EquipRod(FishingRodData.RodName, true);
            _tooltipSystem.NewTooltip($"Equipped the {FishingRodData.RodName}");
        }

        private void OnEnable() {
            _playerData.ChangedEquippedRod += UpdateEquippedCheckmark;
        }

        private void OnDisable() {
            _playerData.ChangedEquippedRod -= UpdateEquippedCheckmark;
        }
    }
}