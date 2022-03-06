// This controls the menu used to see what's held in the bucket
// and also handles removing items from the bucket

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing
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

        // Generate UI elements from data within each item
        // in the bucket and update our capacity slider/text
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

        // Destroy all the UI elements in our menu
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
                    menuItem.gameObject.SetActive(false);
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
                    menuItem.gameObject.SetActive(false);
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