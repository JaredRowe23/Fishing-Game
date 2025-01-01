using Fishing.IO;
using System.Collections.Generic;
using UnityEngine;
using Fishing.PlayerInput;

namespace Fishing.UI {
    public class InventoryMenu : InactiveSingleton {
        [SerializeField, Tooltip("List of InventoryTab objects that contain information related to each inventory tab.")] private List<InventoryTab> _inventoryTabs;
        [SerializeField, Tooltip("Color for active inventory tabs.")] private Color _activeTabColor;
        [SerializeField, Tooltip("Color for inactive inventory tabs.")] private Color _inactiveTabColor;

        private BucketMenu _bucketMenu;
        private PlayerData _playerData;
        private TutorialSystem _tutorialSystem;
        private UIManager _UIManager;
        private AudioManager _audioManager;

        private static InventoryMenu _instance;
        public static InventoryMenu Instance { get => _instance; private set => _instance = value; }

        private void Start() {
            UpdateActiveMenu(_inventoryTabs[0].menuObject);
        }

        public void UpdateActiveMenu(GameObject menu) { // TODO: Separate tab changing into its own class?
            for (int i = 0; i < _inventoryTabs.Count; i++) {
                InventoryTab tab = _inventoryTabs[i];
                if (menu == tab.menuObject) {
                    SetActiveMenu(tab);
                }
                else {
                    SetInactiveMenu(tab);
                }
            }
        }

        private void SetActiveMenu(InventoryTab tab) {
            if (!tab.isActiveMenu) {
                _audioManager.PlaySound("Inventory Tab");
            }

            tab.tabButton.transform.SetAsLastSibling();
            tab.isActiveMenu = true;
            tab.tabButton.image.color = _activeTabColor;
            tab.menuObject.SetActive(true);
        }

        private void SetInactiveMenu(InventoryTab tab) {
            tab.isActiveMenu = false;
            tab.menuObject.SetActive(false);
            tab.tabButton.image.color = _inactiveTabColor;
        }

        public void ToggleInventoryMenu() {
            gameObject.SetActive(!gameObject.activeSelf);
        }

        private void ShowInventoryMenu() {
            if (_bucketMenu.gameObject.activeSelf) {
                _bucketMenu.ToggleBucketMenu();
            }
            _UIManager.HideHUDButtons();

            if (_playerData.HasSeenTutorialData.InventoryTutorial) {
                return;
            }
            _tutorialSystem.QueueTutorial("Here, you can view attachment slots for your fishing rod (line, bait, and hook). You can also take inventory of bait you have and equip bits of gear (TBD)");
            _playerData.HasSeenTutorialData.InventoryTutorial = true;
        }

        private void HideInventoryMenu() {
            _UIManager.ShowHUDButtons();
        }

        private void OnEnable() {
            ShowInventoryMenu();

            InputManager.OnPauseMenu += ToggleInventoryMenu;
        }

        private void OnDisable() {
            HideInventoryMenu();

            InputManager.OnPauseMenu -= ToggleInventoryMenu;
        }

        public override void SetInstanceReference() {
            Instance = this;
        }

        public override void SetDepenencyReferences() {
            _bucketMenu = BucketMenu.Instance;
            _playerData = SaveManager.Instance.LoadedPlayerData;
            _tutorialSystem = TutorialSystem.Instance;
            _UIManager = UIManager.Instance;
            _audioManager = AudioManager.instance;
        }
    }
}