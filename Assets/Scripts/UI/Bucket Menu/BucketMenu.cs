using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fishing.Inventory;

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

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ShowBucketMenu();
            }
        }

        public void ShowBucketMenu()
        {
            gameObject.SetActive(!gameObject.activeSelf);

            if (gameObject.activeSelf)
            {
                AudioManager.instance.PlaySound("Open Bucket");
                InitializeMenu();
            }
            else
            {
                AudioManager.instance.PlaySound("Close Bucket");
                DestroyMenu();
                GameController.instance.itemInfoMenu.SetActive(false);
            }

        }

        void InitializeMenu()
        {
            foreach (FishData _item in bucket.bucketList)
            {
                BucketMenuItem _menu = Instantiate(bucketItemPrefab, content.transform).GetComponent<BucketMenuItem>();
                _menu.UpdateName(_item.itemName);
                _menu.UpdateWeight(_item.itemWeight);
                _menu.UpdateLength(_item.itemLength);
                _menu.UpdateReference(_item);
            }
            capacityBar.value = bucket.bucketList.Count;
            capacityBar.maxValue = bucket.maxItems;
            capacityText.text = bucket.bucketList.Count.ToString() + "/" + bucket.maxItems.ToString();
        }

        void DestroyMenu()
        {
            foreach (Transform _child in content.transform)
            {
                if (_child.GetComponent<BucketMenuItem>())
                {
                    Destroy(_child.gameObject);
                }
            }
        }

        void RefreshMenu()
        {
            DestroyMenu();
            InitializeMenu();
        }

        public void ThrowAway(FishData _itemReference, GameObject _modelReference, GameObject _menuItem)
        {
            if (GameController.instance.overflowItem.activeSelf)
            {
                bucket.bucketList.Remove(_itemReference);

                if (_menuItem != GameController.instance.overflowItem)
                {
                    GameController.instance.equippedRod.GetHook().AddToBucket();
                    Destroy(_menuItem);
                }

                GameController.instance.equippedRod.GetHook().DespawnHookedObject();
                _menuItem.SetActive(false);
                GameController.instance.overflowItem.SetActive(false);
            }

            else
            {
                bucket.bucketList.Remove(_itemReference);
                Destroy(_menuItem);
            }

            if (_modelReference) Destroy(_modelReference);
            AudioManager.instance.PlaySound("Throwaway Fish");
            GameController.instance.itemInfoMenu.SetActive(false);
            RefreshMenu();
            ShowBucketMenu();
        }
    }

}