using Fishing.Inventory;
using Fishing.IO;
using Fishing.PlayerInput;
using UnityEngine;
using UnityEngine.UI;
using Fishing.Audio;

namespace Fishing.UI {
    public class BucketMenu : InactiveSingleton {
        [SerializeField, Tooltip("Slider UI that displays how full the bucket is.")] private Slider _capacityBar;
        [SerializeField, Tooltip("Text UI that displays the current and maximum capacity of the bucket.")] private Text _capacityText;
        [SerializeField, Tooltip("Prefab of UI items to generate for each item in the bucket.")] private GameObject _bucketItemPrefab;

        private BucketBehaviour _bucket;
        [SerializeField, Tooltip("ScrollRect UI that displays the list of bucket items.")] private ScrollRect _scrollRect;

        private PlayerData _playerData;
        private UIManager _UIManager;
        private TutorialSystem _tutorialSystem;
        private AudioManager _audioManager;

        private static BucketMenu _instance;
        public static BucketMenu Instance { get => _instance; private set => _instance = value; }

        public void ToggleBucketMenu() {
            if (_UIManager.OverflowItem.gameObject.activeSelf) {
                return;
            }

            gameObject.SetActive(!gameObject.activeSelf);
        }

        public void RefreshMenu() {
            DestroyMenuItems();
            InitializeMenu();
        }

        public void InitializeMenu() {
            for (int i = 0; i < _bucket.BucketList.Count; i++) {
                BucketMenuItem _menu = Instantiate(_bucketItemPrefab, _scrollRect.content.transform).GetComponent<BucketMenuItem>();
                _menu.UpdateInfo(_bucket.BucketList[i]);
            }

            UpdateCapacity();
        }

        private void UpdateCapacity() {
            _capacityBar.maxValue = _bucket.MaxItems;
            _capacityBar.value = _bucket.BucketList.Count;
            _capacityText.text = $"{_bucket.BucketList.Count}/{_bucket.MaxItems}";
        }

        public void DestroyMenuItems() {
            foreach (Transform _child in _scrollRect.content.transform) {
                Destroy(_child.gameObject);
            }
        }

        private void ShowBucketMenuTutorial() {
            _tutorialSystem.QueueTutorial("Here you can view each fish or item you've caught. Click on each one to view more details, throw it away, or turn it into bait!");
            _playerData.HasSeenTutorialData.BucketMenuTutorial = true;
        }

        public override void SetInstanceReference() {
            Instance = this;
        }

        public override void SetDepenencyReferences() {
            _playerData = SaveManager.Instance.LoadedPlayerData;
            _bucket = BucketBehaviour.Instance;
            _UIManager = UIManager.Instance;
            _tutorialSystem = TutorialSystem.Instance;
            _audioManager = AudioManager.Instance;
        }

        private void OnEnable() {
            if (InventoryMenu.Instance.gameObject.activeSelf) {
                InventoryMenu.Instance.gameObject.SetActive(false);
            }

            InitializeMenu();
            _audioManager.PlaySound("Open Bucket");

            if (!_playerData.HasSeenTutorialData.BucketMenuTutorial) {
                ShowBucketMenuTutorial();
            }

            InputManager.OnPauseMenu += ToggleBucketMenu; // TODO: The check at the beginning of ToggleBucketMenu might cause some unexpected behaviour, look into if a specific 
        }

        private void OnDisable() {
            DestroyMenuItems();
            _audioManager.PlaySound("Close Bucket");

            InputManager.OnPauseMenu -= ToggleBucketMenu;
        }
    }
}