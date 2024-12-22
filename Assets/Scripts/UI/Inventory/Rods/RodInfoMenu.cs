using Fishing.FishingMechanics;
using Fishing.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI {
    public class RodInfoMenu : InactiveSingleton {
        [SerializeField, Tooltip("Image UI that displays the sprite of the currently equipped fishing rod.")] private Image _rodSprite;
        [SerializeField, Tooltip("Text UI that displays the name of the currently equipped fishing rod.")] private Text _rodName;
        [SerializeField, Tooltip("Text UI that displays the description of the currently equipped fishing rod.")] private Text _rodDescription;

        [SerializeField, Tooltip("Button for selecting which bait to attach.")] private AttachmentSlotButton _baitButton;
        [SerializeField, Tooltip("Button for selecting which hook to attach.")] private AttachmentSlotButton _hookButton;
        [SerializeField, Tooltip("Button for selecting which line to attach.")] private AttachmentSlotButton _lineButton;

        private PlayerData _playerData;
        private RodManager _rodManager;

        private static RodInfoMenu instance;
        public static RodInfoMenu Instance { get => instance; set => instance = value; }

        private void Awake() {
            UpdateRodInfo();
        }

        public void UpdateRodInfo() {
            RodScriptable rodScriptable = ItemLookupTable.Instance.StringToRodScriptable(_playerData.EquippedRod.RodName);
            _rodSprite.sprite = rodScriptable.InventorySprite;
            _rodName.text = rodScriptable.RodName;
            _rodDescription.text = rodScriptable.Description;

            BaitBehaviour bait = _rodManager.EquippedRod.EquippedBait;
            if (bait != null) {
                _baitButton.UpdateButton(bait.Scriptable.BaitName, bait.Scriptable.InventorySprite);
            }
            else {
                _baitButton.UpdateButton("No Bait", null);
            }
        }

        public override void SetInstanceReference() {
            Instance = this;
        }

        public override void SetDepenencyReferences() {
            _playerData = SaveManager.Instance.LoadedPlayerData;
            _rodManager = RodManager.Instance;
        }

        public void OnEnable() {
            _playerData.ChangedEquippedRod += UpdateRodInfo;
        }

        public void OnDisable() {
            _playerData.ChangedEquippedRod -= UpdateRodInfo;
        }
    }
}