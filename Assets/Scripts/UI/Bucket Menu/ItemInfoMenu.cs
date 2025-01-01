using Fishing.Fishables;
using Fishing.Inventory;
using Fishing.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI {
    public class ItemInfoMenu : InactiveSingleton {
        [SerializeField, Tooltip("Image UI to use for displaying the bucket item.")] private Image _itemImage;
        [SerializeField, Tooltip("Text UI to use for displaying the bucket item name.")] private Text _itemNameText;
        [SerializeField, Tooltip("Text UI to use for displaying the bucket item description.")] private Text _itemDescriptionText;
        [SerializeField, Tooltip("Text UI to use for displaying the bucket item value.")] private Text _itemValueText;
        [SerializeField, Tooltip("Text UI to use for displaying the bucket item weight.")] private Text _itemWeightText;
        [SerializeField, Tooltip("Text UI to use for displaying the bucket item length.")] private Text _itemLengthText;

        private BucketItemSaveData _dataReference;
        private BucketMenuItem _menuListingReference;

        private PlayerData _playerData;
        private UIManager _manager;
        private BucketBehaviour _bucket;
        private BucketMenu _bucketMenu;
        private TooltipSystem _tooltipSystem;
        private RodManager _rodManager;
        private AudioManager _audioManager;
        private TutorialSystem _tutorialSystem;

        private static ItemInfoMenu _instance;
        public static ItemInfoMenu Instance { get => _instance; private set { _instance = value; } }

        public void UpdateMenu(BucketItemSaveData reference, BucketMenuItem menuListing) {
            _menuListingReference = menuListing;

            _dataReference = reference;
            _itemImage.sprite = ItemLookupTable.Instance.StringToFishScriptable(_dataReference.ItemName).InventorySprite;
            _itemNameText.text = _dataReference.ItemName;
            _itemValueText.text = _dataReference.Value.ToString();
            _itemWeightText.text = _dataReference.Weight.ToString();
            _itemLengthText.text = _dataReference.Length.ToString();
            _itemDescriptionText.text = _dataReference.Description;
        }

        public void ThrowAwayItem() {
            _tooltipSystem.NewTooltip($"Threw away the {_dataReference.ItemName}");
            if (_manager.overflowItem.gameObject.activeSelf) {
                HandleOverflowItem();
            }
            RemoveItem();
        }

        public void SellItem() {
            _playerData.SaveFileData.Money += _dataReference.Value;
            _tooltipSystem.NewTooltip($"Sold the {_dataReference.ItemName} for {_dataReference.Value.ToString("C")}");
            RemoveItem();
        }

        public void ConvertToBait() {
            _playerData.AddBait(_dataReference.ItemName, 1);
            if (_manager.overflowItem.gameObject.activeSelf) {
                HandleOverflowItem();
            }
            _tooltipSystem.NewTooltip($"Converted the {_dataReference.ItemName} into bait");
            if (!_playerData.HasSeenTutorialData.BaitTutorial) {
                ShowBaitTutorial();
            }
            RemoveItem();
        }

        public void HandleOverflowItem() {
            if (_menuListingReference != _manager.overflowItem) {
                _bucket.AddToBucket(_rodManager.EquippedRod.Hook.HookedObject.GetComponent<Fishable>());
            }

            _rodManager.EquippedRod.Hook.DestroyHookedObject();
            _manager.overflowItem.gameObject.SetActive(false);
            _bucketMenu.ToggleBucketMenu();
        }

        private void RemoveItem() {
            _bucket.BucketList.Remove(_dataReference);
            if (_menuListingReference != _manager.overflowItem) {
                Destroy(_menuListingReference);
            }

            _audioManager.PlaySound("Throwaway Fish");

            _manager.itemInfoMenu.SetActive(false);
            _bucketMenu.RefreshMenu();
            gameObject.SetActive(false);
        }

        private void ShowBaitTutorial() {
            _tutorialSystem.QueueTutorial("Bait can help you catch fish that aren't interested in just your hook as is. Close the bucket menu and open the inventory menu (I) to equip it!");
            _playerData.HasSeenTutorialData.BaitTutorial = true;
        }

        public override void SetInstanceReference() {
            Instance = this;
        }

        public override void SetDepenencyReferences() {
            _playerData = SaveManager.Instance.LoadedPlayerData;
            _manager = UIManager.instance;
            _bucketMenu = BucketMenu.Instance;
            _tooltipSystem = TooltipSystem.Instance;
            _rodManager = RodManager.Instance;
            _bucket = BucketBehaviour.Instance;
            _audioManager = AudioManager.instance;
            _tutorialSystem = TutorialSystem.Instance;
        }

        private void OnDisable() {
            gameObject.SetActive(false);
        }
    }
}