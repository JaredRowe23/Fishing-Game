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

        private PlayerData playerData;

        public static BucketMenu instance;

        private BucketMenu() => instance = this;

        private void Awake()
        {
            playerData = SaveManager.Instance.LoadedPlayerData;
        }

        public void ToggleBucketMenu()
        {
            if (UIManager.instance.overflowItem.activeSelf) return;

            gameObject.SetActive(!gameObject.activeSelf);

            if (gameObject.activeSelf) ShowBucketMenu();
            else HideBucketMenu();
        }

        public void ShowBucketMenu()
        {
            if (InventoryMenu.instance.gameObject.activeSelf) InventoryMenu.instance.HideInventoryMenu();
            AudioManager.instance.PlaySound("Open Bucket");
            InitializeMenu();

            if (!playerData.HasSeenTutorialData.BucketMenuTutorial) ShowBucketMenuTutorial();
        }

        public void HideBucketMenu()
        {
            AudioManager.instance.PlaySound("Close Bucket");
            DestroyMenu();
            UIManager.instance.itemInfoMenu.SetActive(false);
        }

        private void ShowBucketMenuTutorial()
        {
            TutorialSystem.instance.QueueTutorial("Here you can view each fish or item you've caught. Click on each one to view more details, throw it away, or turn it into bait!");
            playerData.HasSeenTutorialData.BucketMenuTutorial = true;
        }

        public void InitializeMenu()
        {
            for (int i = 0; i < bucket.BucketList.Count; i++)
            {
                BucketMenuItem _menu = Instantiate(bucketItemPrefab, content.transform).GetComponent<BucketMenuItem>();
                _menu.UpdateInfo(bucket.BucketList[i]);
            }

            UpdateCapacity();
        }

        private void UpdateCapacity()
        {
            capacityBar.maxValue = bucket.MaxItems;
            capacityBar.value = bucket.BucketList.Count;
            capacityText.text = bucket.BucketList.Count.ToString() + "/" + bucket.MaxItems.ToString();
        }

        public void DestroyMenu()
        {
            foreach (Transform _child in content.transform)
            {
                if (!_child.GetComponent<BucketMenuItem>()) continue;

                Destroy(_child.gameObject);
            }
        }

        public void RefreshMenu()
        {
            DestroyMenu();
            InitializeMenu();
        }
    }
}