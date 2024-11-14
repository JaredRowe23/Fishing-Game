using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.Fishables;
using Fishing.UI;
using Fishing.IO;
using Fishing.Fishables.Fish;

namespace Fishing.Inventory
{
    public class BucketBehaviour : MonoBehaviour
    {
        [SerializeField] public List<BucketItemSaveData> bucketList = new List<BucketItemSaveData>();
        public int maxItems;

        private PlayerData playerData;
        private RodManager rodManager;

        public static BucketBehaviour instance;

        private void Awake()
        {
            instance = this;
            rodManager = RodManager.instance;

            playerData = PlayerData.instance;
            bucketList = playerData.bucketItemSaveData;
        }

        public void AddToBucket(Fishable _item)
        {
            AudioManager.instance.PlaySound("Catch Fish");

            BucketItemSaveData _bucketItemData = new BucketItemSaveData(_item.GetName(), _item.GetDescription(), _item.GetWeight(), _item.GetLength(), _item.GetValue());

            if (bucketList.Count >= maxItems)
            {
                OnBucketOverflow(_bucketItemData);
                return;
            }

            AudioManager.instance.PlaySound("Add To Bucket");
            TooltipSystem.instance.NewTooltip(5f, "You caught a " + _bucketItemData.itemName + " worth $" + _bucketItemData.value.ToString("F2"));

            _item.DisableMinimapIndicator();
            bucketList.Add(_bucketItemData);

            if (playerData.hasSeenTutorialData.bucketTutorial) ShowBucketTutorial();
            playerData.UpdateFishRecordData(_bucketItemData);

            rodManager.equippedRod.GetHook().DespawnHookedObject();
            rodManager.equippedRod.ReEquipBait();
        }

        private void ShowBucketTutorial()
        {
            TutorialSystem.instance.QueueTutorial("Press B or click the bucket icon in the top-left corner to access your bucket");
            playerData.hasSeenTutorialData.bucketTutorial = true;
        }

        private void OnBucketOverflow(BucketItemSaveData _itemData)
        {
            TooltipSystem.instance.NewTooltip(5f, "You've filled your bucket! Pick something to throw away to make room!");
            BucketMenu.instance.ToggleBucketMenu();
            UIManager.instance.overflowItem.SetActive(true);
            BucketMenuItem _overflowMenu = UIManager.instance.overflowItem.GetComponent<BucketMenuItem>();

            _overflowMenu.UpdateInfo(_itemData);
        }
    }

}