using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fishing.Inventory;
using Fishing.IO;
using Fishing.Fishables;

namespace Fishing.UI
{
    public class ItemInfoMenu : MonoBehaviour
    {
        [Header("Object Stats")]
        [SerializeField] private Image itemImage;
        [SerializeField] private Text itemName;
        [SerializeField] private Text itemWeight;
        [SerializeField] private Text itemLength;
        [SerializeField] private Text itemValue;
        [SerializeField] private Text itemDescription;
        [SerializeField] private List<GameObject> models;
        [SerializeField] private List<string> modelNames;
        private GameObject currentModel;

        private BucketItemSaveData itemReference;
        private GameObject menuListingReference;

        private PlayerData playerData;
        private UIManager manager;
        private BucketBehaviour bucket;
        private BucketMenu bucketMenu;
        private TooltipSystem tooltipSystem;
        private RodManager rodManager;

        private void Awake()
        {
            playerData = PlayerData.instance;
            manager = UIManager.instance;
            bucketMenu = BucketMenu.instance;
            tooltipSystem = TooltipSystem.instance;
            rodManager = RodManager.instance;
            bucket = BucketBehaviour.instance;
        }

        public void UpdateMenu(BucketItemSaveData _reference, GameObject _menuListing)
        {
            itemReference = _reference;
            itemName.text = _reference.itemName;
            itemValue.text = _reference.value.ToString();
            itemWeight.text = _reference.weight.ToString();
            itemLength.text = _reference.length.ToString();
            itemDescription.text = _reference.description;

            menuListingReference = _menuListing;
            GenerateModel(_reference.itemName);

            ItemViewerCamera.instance.UpdateCurrentItem(currentModel);
        }

        public void ThrowAwayItem()
        {
            tooltipSystem.NewTooltip(5f, "Threw away the " + itemReference.itemName + " worth $" + itemReference.value.ToString("F2"));
            if (manager.overflowItem.activeSelf) HandleOverflowItem();
            RemoveItem();
        }

        public void SellItem()
        {
            playerData.saveFileData.money += itemReference.value;
            tooltipSystem.NewTooltip(5f, "Sold the " + itemReference.itemName + " for $" + itemReference.value.ToString("F2"));
            RemoveItem();
        }

        public void ConvertToBait()
        {
            playerData.AddBait(itemReference.itemName);
            if (manager.overflowItem.activeSelf) HandleOverflowItem();
            tooltipSystem.NewTooltip(5f, "Converted the " + itemReference.itemName + " into bait");
            if (!playerData.hasSeenTutorialData.baitTutorial) ShowBaitTutorial();
            RemoveItem();
        }

        private void RemoveItem()
        {
            RemoveAllObjects();

            AudioManager.instance.PlaySound("Throwaway Fish");

            manager.itemInfoMenu.SetActive(false);
            bucketMenu.RefreshMenu();
            gameObject.SetActive(false);
        }

        private void RemoveAllObjects()
        {
            bucket.bucketList.Remove(itemReference);
            if (menuListingReference != manager.overflowItem) Destroy(menuListingReference);
            if (currentModel != null) Destroy(currentModel);
        }

        public void HandleOverflowItem()
        {
            if (menuListingReference != manager.overflowItem) bucket.AddToBucket(rodManager.equippedRod.GetHook().hookedObject.GetComponent<Fishable>());

            rodManager.equippedRod.GetHook().DestroyHookedObject();
            manager.overflowItem.SetActive(false);
            bucketMenu.ToggleBucketMenu();
        }

        private void ShowBaitTutorial()
        {
            TutorialSystem.instance.QueueTutorial("Bait can help you catch fish that aren't interested in just your hook as is. Close the bucket menu and open the inventory menu (I) to equip it!");
            playerData.hasSeenTutorialData.baitTutorial = true;
        }

        public GameObject GenerateModel(string _itemName) // to be removed when switching model viewer to sprite viewer
        {
            if (currentModel != null) Destroy(currentModel);

            for (int i = 0; i < models.Count; i++)
            {
                if (modelNames[i] == _itemName)
                {
                    currentModel = Instantiate(models[i]);
                }
            }

            currentModel.transform.parent = ItemViewerCamera.instance.transform.parent;
            currentModel.transform.position = ItemViewerCamera.instance.transform.parent.position;
            currentModel.transform.rotation = Quaternion.identity;

            return currentModel;

        }

        public List<string> GetModelNames() => modelNames;
    }

}