using Fishing.Fishables;
using Fishing.FishingMechanics;
using Fishing.IO;
using Fishing.UI;
using System.Collections.Generic;
using UnityEngine;
using Fishing.Audio;

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
        private BaitManager _baitManager;

        private static BucketBehaviour _instance;
        public static BucketBehaviour Instance { get => _instance; private set => _instance = value; }

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            _rodManager = RodManager.Instance;
            _playerData = SaveManager.Instance.LoadedPlayerData;
            _audioManager = AudioManager.Instance;
            _tooltipSystem = TooltipSystem.Instance;
            _tutorialSystem = TutorialSystem.Instance;
            _overflowItem = UIManager.Instance.OverflowItem;
            _bucketMenu = BucketMenu.Instance;
            _baitManager = BaitManager.Instance;

            BucketList = _playerData.BucketItemSaveData;
        }

        public void AddToBucket(Fishable fishable) {
            _audioManager.PlaySound("Catch Fish");

            BucketItemSaveData bucketItemData = new BucketItemSaveData(fishable.FishableScriptable.ItemName, fishable.FishableScriptable.ItemDescription, fishable.Weight, fishable.Length, fishable.Value);

            if (BucketList.Count >= MaxItems) {
                OnBucketOverflow(bucketItemData);
                return;
            }

            _audioManager.PlaySound("Add To Bucket");
            _tooltipSystem.NewTooltip($"You caught a {bucketItemData.ItemName} worth {bucketItemData.Value.ToString("C")}");

            BucketList.Add(bucketItemData);
            _rodManager.EquippedRod.Hook.DestroyHookedObject();
            _baitManager.SpawnBait();

            _playerData.UpdateFishRecordData(bucketItemData);

            if (!_playerData.HasSeenTutorialData.BucketTutorial) {
                ShowBucketTutorial();
            }
        }

        private void OnBucketOverflow(BucketItemSaveData itemData) {
            _tooltipSystem.NewTooltip("You've filled your bucket! Pick something to throw away to make room!");
            _bucketMenu.ToggleBucketMenu();
            _overflowItem.gameObject.SetActive(true);
            _overflowItem.UpdateInfo(itemData);
        }

        private void ShowBucketTutorial() {
            _tutorialSystem.QueueTutorial("Press B or click the bucket icon in the top-left corner to access your bucket");
            _playerData.HasSeenTutorialData.BucketTutorial = true;
        }
    }

}