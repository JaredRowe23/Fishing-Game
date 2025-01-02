using Fishing.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI {
    public class RodsMenu : InactiveSingleton
    {
        [SerializeField, Tooltip("Prefab UI to display each player owned fishing rod.")] private GameObject _slotPrefab;
        [SerializeField, Tooltip("ScrollRect UI that displays each player owned fishing rod.")] private ScrollRect _scrollRect;
        [SerializeField, Tooltip("Menu that displays extra info about the selected (equipped) fishing rod.")] private RodInfoMenu _rodInfoMenu;

        private PlayerData _playerData;
        private InventoryMenu _inventoryMenu;

        private static RodsMenu _instance;
        public static RodsMenu Instance { get => _instance; private set => _instance = value; }

        public void ShowRodMenu() {
            _inventoryMenu.UpdateActiveMenu(gameObject);
        }

        public void GenerateSlots() {
            for (int i = 0; i < _playerData.FishingRodSaveData.Count; i++) {
                RodInventorySlot newSlot = Instantiate(_slotPrefab, _scrollRect.content.transform).GetComponent<RodInventorySlot>();
                newSlot.UpdateSlot(_playerData.FishingRodSaveData[i]);
            }
        }

        private void DestroySlots() {
            foreach (Transform _child in _scrollRect.content.transform) {
                Destroy(_child.gameObject);
            }
        }

        private void OnEnable() {
            GenerateSlots();
        }

        private void OnDisable() {
            DestroySlots();
        }

        public override void SetInstanceReference() {
            Instance = this;
        }

        public override void SetDepenencyReferences() {
            _playerData = SaveManager.Instance.LoadedPlayerData;
            _inventoryMenu = InventoryMenu.Instance;
        }
    }
}