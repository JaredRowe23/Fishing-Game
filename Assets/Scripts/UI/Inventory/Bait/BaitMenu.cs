using Fishing.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI {
    public class BaitMenu : InactiveSingleton {
        [SerializeField, Tooltip("Prefabs that will spawn for each bait type the player has.")] private GameObject _slotPrefab;
        [SerializeField, Tooltip("ScrollRect UI that will hold each bait type listing.")] private ScrollRect _listingsScrollRect;

        private InventoryMenu _inventoryMenu;
        private BaitInfoMenu _baitInfoMenu;
        private PlayerData _playerData;

        private BaitMenu _instance;
        public BaitMenu Instance { get => _instance; private set { _instance = value; } }

        public void ShowBaitMenu() {
            _inventoryMenu.UpdateActiveMenu(gameObject);
        }

        public void GenerateSlots() {
            for (int i = 0; i < _playerData.BaitSaveData.Count; i++) {
                BaitInventorySlot newSlot = Instantiate(_slotPrefab, _listingsScrollRect.content.transform).GetComponent<BaitInventorySlot>();
                newSlot.UpdateSlot(_playerData.BaitSaveData[i]);
            }
        }

        private void DestroySlots() {
            foreach (Transform child in _listingsScrollRect.content.transform) {
                Destroy(child.gameObject);
            }
        }

        private void OnEnable() {
            GenerateSlots();
        }
        private void OnDisable() {
            DestroySlots();
            _baitInfoMenu.gameObject.SetActive(false);
        }

        public override void SetInstanceReference() {
            Instance = this;
        }

        public override void SetDepenencyReferences() {
            _inventoryMenu = InventoryMenu.Instance;
            _baitInfoMenu = BaitInfoMenu.Instance;
            _playerData = SaveManager.Instance.LoadedPlayerData;
        }
    }

}