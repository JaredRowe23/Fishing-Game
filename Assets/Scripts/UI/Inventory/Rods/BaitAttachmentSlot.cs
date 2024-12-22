using Fishing.FishingMechanics;
using Fishing.IO;
using UnityEngine;

namespace Fishing.UI {
    public class BaitAttachmentSlot : BaitUI {
        [SerializeField, Tooltip("Check for if this option is for choosing to have no bait on the hook.")] private bool _isNoBaitOption;
        public bool IsNoBaitOption { get => _isNoBaitOption;}

        private PlayerData _playerData;
        private RodManager _rodManager;
        private BaitManager _baitManager;
        private RodInfoMenu _rodInfoMenu;
        private BaitAttachmentButton _baitAttachmentButton;

        private void Awake() {
            _playerData = SaveManager.Instance.LoadedPlayerData;
            _rodManager = RodManager.Instance;
            _baitManager = BaitManager.Instance;
            _rodInfoMenu = RodInfoMenu.Instance;
            _baitAttachmentButton = BaitAttachmentButton.Instance;
        }

        public void EquipBait() {
            UnequipCurrentBait();
            _playerData.EquippedRod.EquippedBait = _baitSaveData;
            _baitManager.SpawnBait();
            _rodInfoMenu.UpdateRodInfo();
            _baitAttachmentButton.UpdateSlotOptions();
        }

        private void UnequipCurrentBait() {
            if (string.IsNullOrEmpty(_playerData.EquippedRod.EquippedBait?.BaitName)) {
                return;
            }

            _playerData.EquippedRod.EquippedBait.Amount++;
            Destroy(_rodManager.EquippedRod.EquippedBait.gameObject);
            _playerData.EquippedRod.EquippedBait = null;
        }

        public void UnequipSelectedBait() {
            UnequipCurrentBait();
            _rodInfoMenu.UpdateRodInfo();
            _baitAttachmentButton.UpdateSlotOptions();
        }
    }
}
