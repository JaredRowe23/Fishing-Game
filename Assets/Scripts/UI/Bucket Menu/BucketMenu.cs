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


        void Update()
        {
            if (!gameObject.activeSelf) return;

            if (Input.GetKeyDown(KeyCode.B))
            {
                ShowBucketMenu();
            }
        }

        public void ShowBucketMenu()
        {
            if (UIManager.instance.overflowItem.activeSelf) return;

            gameObject.SetActive(!gameObject.activeSelf);

            if (gameObject.activeSelf)
            {
                if (InventoryMenu.instance.gameObject.activeSelf) InventoryMenu.instance.ShowInventoryMenu();
                AudioManager.instance.PlaySound("Open Bucket");
                InitializeMenu();
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

            for (int i = 0; i < PlayerData.instance.bucketFish.Count; i++)
            {
                if (PlayerData.instance.bucketFish[i] != _itemReference.itemName) continue;
                if (PlayerData.instance.bucketFishDescription[i] != _itemReference.itemDescription) continue;
                if (PlayerData.instance.bucketFishLength[i] != _itemReference.itemLength) continue;
                if (PlayerData.instance.bucketFishWeight[i] != _itemReference.itemWeight) continue;
                if (PlayerData.instance.bucketFishValue[i] != _itemReference.itemValue) continue;

                if (_isSelling)
                {
                    PlayerData.instance.money += PlayerData.instance.bucketFishValue[i];
                }

                PlayerData.instance.bucketFish.Remove(PlayerData.instance.bucketFish[i]);
                PlayerData.instance.bucketFishDescription.Remove(PlayerData.instance.bucketFishDescription[i]);
                PlayerData.instance.bucketFishLength.Remove(PlayerData.instance.bucketFishLength[i]);
                PlayerData.instance.bucketFishWeight.Remove(PlayerData.instance.bucketFishWeight[i]);
                PlayerData.instance.bucketFishValue.Remove(PlayerData.instance.bucketFishValue[i]);
            }

            bucket.bucketList.Remove(_itemReference);
            Destroy(_menuItem);

            if (_modelReference) Destroy(_modelReference);
            AudioManager.instance.PlaySound("Throwaway Fish");
            UIManager.instance.itemInfoMenu.SetActive(false);
            RefreshMenu();
            if (!_isSelling) ShowBucketMenu();
        }

        public void ConvertToBait(FishData _itemReference, GameObject _modelReference, GameObject _menuItem)
        {
            for (int i = 0; i < PlayerData.instance.bucketFish.Count; i++)
            {
                if (PlayerData.instance.bucketFish[i] != _itemReference.itemName) continue;
                if (PlayerData.instance.bucketFishDescription[i] != _itemReference.itemDescription) continue;
                if (PlayerData.instance.bucketFishLength[i] != _itemReference.itemLength) continue;
                if (PlayerData.instance.bucketFishWeight[i] != _itemReference.itemWeight) continue;
                if (PlayerData.instance.bucketFishValue[i] != _itemReference.itemValue) continue;

                PlayerData.instance.bucketFish.Remove(PlayerData.instance.bucketFish[i]);
                PlayerData.instance.bucketFishDescription.Remove(PlayerData.instance.bucketFishDescription[i]);
                PlayerData.instance.bucketFishLength.Remove(PlayerData.instance.bucketFishLength[i]);
                PlayerData.instance.bucketFishWeight.Remove(PlayerData.instance.bucketFishWeight[i]);
                PlayerData.instance.bucketFishValue.Remove(PlayerData.instance.bucketFishValue[i]);
            }

            PlayerData.instance.AddBait(_itemReference.itemName);

            bucket.bucketList.Remove(_itemReference);
            Destroy(_menuItem);

            if (_modelReference) Destroy(_modelReference);
            AudioManager.instance.PlaySound("Throwaway Fish");
            UIManager.instance.itemInfoMenu.SetActive(false);
            RefreshMenu();
        }
    }
}