using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fishing.Inventory;
using Fishing.IO;

namespace Fishing.UI
{
    public class BucketMenu : MonoBehaviour
    {
        [SerializeField] private BucketBehaviour bucket;
        [SerializeField] private GameObject content;
        [SerializeField] private Slider capacityBar;
        [SerializeField] private Text capacityText;
        [SerializeField] private GameObject bucketItemPrefab;

        public static BucketMenu instance;

        private BucketMenu() => instance = this;

        public void ToggleBucketMenu()
        {
            if (UIManager.instance.overflowItem.activeSelf) return;

            gameObject.SetActive(!gameObject.activeSelf);

            if (gameObject.activeSelf)
            {
                if (InventoryMenu.instance.gameObject.activeSelf) InventoryMenu.instance.ToggleInventoryMenu();
                AudioManager.instance.PlaySound("Open Bucket");
                InitializeMenu();

                if (PlayerData.instance.hasSeenTutorialData.bucketMenuTutorial) return;
                TutorialSystem.instance.QueueTutorial("Here you can view each fish or item you've caught. Click on each one to view more details, throw it away, or turn it into bait!");
                PlayerData.instance.hasSeenTutorialData.bucketMenuTutorial = true;
            }
            else
            {
                AudioManager.instance.PlaySound("Close Bucket");
                DestroyMenu();
                UIManager.instance.itemInfoMenu.SetActive(false);
            }

        }

        public void InitializeMenu()
        {
            foreach (FishData _item in bucket.bucketList)
            {
                BucketMenuItem _menu = Instantiate(bucketItemPrefab, content.transform).GetComponent<BucketMenuItem>();
                _menu.UpdateName(_item.itemName);
                _menu.UpdateWeight(_item.itemWeight);
                _menu.UpdateLength(_item.itemLength);
                _menu.UpdateValue(_item.itemValue);
                _menu.UpdateReference(_item);
            }

            capacityBar.maxValue = bucket.maxItems;
            capacityBar.value = bucket.bucketList.Count;
            capacityText.text = bucket.bucketList.Count.ToString() + "/" + bucket.maxItems.ToString();
        }

        public void DestroyMenu()
        {
            foreach (Transform _child in content.transform)
            {
                if (_child.GetComponent<BucketMenuItem>())
                {
                    Destroy(_child.gameObject);
                }
            }
        }

        public void RefreshMenu()
        {
            DestroyMenu();
            InitializeMenu();
        }

        public void ThrowAway(FishData _itemReference, GameObject _modelReference, GameObject _menuItem, bool _isSelling)
        {

            for (int i = 0; i < PlayerData.instance.bucketItemSaveData.Count; i++)
            {
                if (PlayerData.instance.bucketItemSaveData[i].itemName != _itemReference.itemName) continue;
                if (PlayerData.instance.bucketItemSaveData[i].description != _itemReference.itemDescription) continue;
                if (PlayerData.instance.bucketItemSaveData[i].length != _itemReference.itemLength) continue;
                if (PlayerData.instance.bucketItemSaveData[i].weight != _itemReference.itemWeight) continue;
                if (PlayerData.instance.bucketItemSaveData[i].value != _itemReference.itemValue) continue;

                if (_isSelling)
                {
                    PlayerData.instance.saveFileData.money += PlayerData.instance.bucketItemSaveData[i].value;
                }

                PlayerData.instance.bucketItemSaveData.Remove(PlayerData.instance.bucketItemSaveData[i]);
            }

            bucket.bucketList.Remove(_itemReference);
            Destroy(_menuItem);

            if (_modelReference) Destroy(_modelReference);
            AudioManager.instance.PlaySound("Throwaway Fish");
            UIManager.instance.itemInfoMenu.SetActive(false);
            RefreshMenu();
            if (!_isSelling) ToggleBucketMenu();
        }

        public void ConvertToBait(FishData _itemReference, GameObject _modelReference, GameObject _menuItem)
        {
            for (int i = 0; i < PlayerData.instance.bucketItemSaveData.Count; i++)
            {
                if (PlayerData.instance.bucketItemSaveData[i].itemName != _itemReference.itemName) continue;
                if (PlayerData.instance.bucketItemSaveData[i].description != _itemReference.itemDescription) continue;
                if (PlayerData.instance.bucketItemSaveData[i].length != _itemReference.itemLength) continue;
                if (PlayerData.instance.bucketItemSaveData[i].weight != _itemReference.itemWeight) continue;
                if (PlayerData.instance.bucketItemSaveData[i].value != _itemReference.itemValue) continue;

                PlayerData.instance.bucketItemSaveData.Remove(PlayerData.instance.bucketItemSaveData[i]);
            }

            PlayerData.instance.AddBait(_itemReference.itemName);

            bucket.bucketList.Remove(_itemReference);
            Destroy(_menuItem);

            if (_modelReference) Destroy(_modelReference);
            AudioManager.instance.PlaySound("Throwaway Fish");
            UIManager.instance.itemInfoMenu.SetActive(false);
            RefreshMenu();

            if (PlayerData.instance.hasSeenTutorialData.baitTutorial) return;
            TutorialSystem.instance.QueueTutorial("Bait can help you catch fish that aren't interested in just your hook as is. Close the bucket menu and open the inventory menu (I) to equip it!");
            PlayerData.instance.hasSeenTutorialData.baitTutorial = true;
        }
    }
}