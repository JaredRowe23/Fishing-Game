using Fishing.Fishables;
using Fishing.IO;
using Fishing.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.Inventory {
    public class BucketBehaviour : MonoBehaviour {
        [SerializeField, Min(0), Tooltip("Maximum amount of items this bucket can hold at once.")] private int _maxItems;
        public int MaxItems { get => _maxItems; private set => _maxItems = value; }

        private List<BucketItemSaveData> _bucketList;
        public List<BucketItemSaveData> BucketList { get => _bucketList; private set => _bucketList = value; }

        private PlayerData _playerData;
        private RodManager _rodManager;
        private AudioManager _audioManager;
        private TooltipSystem _tooltipSystem;
        private TutorialSystem _tutorialSystem;
        private BucketMenuItem _overflowItem;
        private BucketMenu _bucketMenu;

        private static BucketBehaviour _instance;
        public static BucketBehaviour Instance { get => _instance; private set => _instance = value; }

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            _rodManager = RodManager.Instance;
            _playerData = PlayerData.instance;
            _audioManager = AudioManager.instance;
            _tooltipSystem = TooltipSystem.instance;
            _tutorialSystem = TutorialSystem.instance;
            _overflowItem = UIManager.instance.overflowItem.GetComponent<BucketMenuItem>();
            _bucketMenu = BucketMenu.instance;
            BucketList = _playerData.bucketItemSaveData;
        }

        public void AddToBucket(Fishable fishable) {
            _audioManager.PlaySound("Catch Fish");

            BucketItemSaveData bucketItemData = new BucketItemSaveData(fishable.ItemName, fishable.ItemDescription, fishable.Weight, fishable.Length, fishable.Value);

            if (BucketList.Count >= MaxItems) {
                OnBucketOverflow(bucketItemData);
                return;
            }

            _audioManager.PlaySound("Add To Bucket");
            _tooltipSystem.NewTooltip(5f, $"You caught a {bucketItemData.itemName} worth {bucketItemData.value.ToString("C")}");

            BucketList.Add(bucketItemData);
            _rodManager.EquippedRod.Hook.DestroyHookedObject();
            _rodManager.EquippedRod.ReEquipBait();

            _playerData.UpdateFishRecordData(bucketItemData);

            if (!_playerData.hasSeenTutorialData.bucketTutorial) {
                ShowBucketTutorial();
            }
        }

        private void OnBucketOverflow(BucketItemSaveData itemData) {
            _tooltipSystem.NewTooltip(5f, "You've filled your bucket! Pick something to throw away to make room!");
            _bucketMenu.ToggleBucketMenu();
            _overflowItem.gameObject.SetActive(true);
            _overflowItem.UpdateInfo(itemData);
        }

        private void ShowBucketTutorial() {
            _tutorialSystem.QueueTutorial("Press B or click the bucket icon in the top-left corner to access your bucket");
            _playerData.hasSeenTutorialData.bucketTutorial = true;
        }
    }

}