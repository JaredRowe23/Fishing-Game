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
            this.gameObject.SetActive(!this.gameObject.activeSelf);

            if (this.gameObject.activeSelf)
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
            foreach (FishData item in bucket.bucketList)
            {
                BucketMenuItem menu = Instantiate(bucketItemPrefab, content.transform).GetComponent<BucketMenuItem>();
                menu.UpdateName(item.itemName);
                menu.UpdateWeight(item.itemWeight);
                menu.UpdateLength(item.itemLength);
                menu.UpdateReference(item);
            }
            capacityBar.value = bucket.bucketList.Count;
            capacityBar.maxValue = bucket.maxItems;
            capacityText.text = bucket.bucketList.Count.ToString() + "/" + bucket.maxItems.ToString();
        }

        void DestroyMenu()
        {
            foreach (Transform child in content.transform)
            {
                if (child.GetComponent<BucketMenuItem>())
                {
                    Destroy(child.gameObject);
                }
            }
        }

        void RefreshMenu()
        {
            DestroyMenu();
            InitializeMenu();
        }

        public void ThrowAway(FishData itemReference, GameObject modelReference, GameObject menuItem)
        {

            if (GameController.instance.overflowItem.activeSelf)
            {
                if (menuItem == GameController.instance.overflowItem)
                {
                    if (modelReference)
                    {
                        Destroy(modelReference);
                    }
                    bucket.bucketList.Remove(itemReference);
                    GameController.instance.itemInfoMenu.SetActive(false);
                    menuItem.SetActive(false);
                    ShowBucketMenu();
                }
                else
                {
                    if (modelReference)
                    {
                        Destroy(modelReference);
                    }
                    bucket.bucketList.Remove(itemReference);
                    Destroy(menuItem);
                    RefreshMenu();

                    GameController.instance.equippedRod.GetHook().AddToBucket();
                    GameController.instance.itemInfoMenu.SetActive(false);
                    menuItem.SetActive(false);
                    GameController.instance.overflowItem.SetActive(false);
                    ShowBucketMenu();
                }
            }
            else
            {
                if (modelReference)
                {
                    Destroy(modelReference);
                }
                bucket.bucketList.Remove(itemReference);
                Destroy(menuItem);
                RefreshMenu();
            }
            AudioManager.instance.PlaySound("Throwaway Fish");
        }
    }

}