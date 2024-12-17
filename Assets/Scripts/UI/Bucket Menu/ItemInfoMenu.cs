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
            playerData = SaveManager.Instance.LoadedPlayerData;
            manager = UIManager.instance;
            bucketMenu = BucketMenu.instance;
            tooltipSystem = TooltipSystem.instance;
            rodManager = RodManager.Instance;
            bucket = BucketBehaviour.Instance;
        }

        public void UpdateMenu(BucketItemSaveData _reference, GameObject _menuListing)
        {
            itemReference = _reference;
            itemName.text = _reference.ItemName;
            itemValue.text = _reference.Value.ToString();
            itemWeight.text = _reference.Weight.ToString();
            itemLength.text = _reference.Length.ToString();
            itemDescription.text = _reference.Description;

            menuListingReference = _menuListing;
            GenerateModel(_reference.ItemName);

            ItemViewerCamera.instance.UpdateCurrentItem(currentModel);
        }

        public void ThrowAwayItem()
        {
            tooltipSystem.NewTooltip(5f, "Threw away the " + itemReference.ItemName + " worth $" + itemReference.Value.ToString("F2"));
            if (manager.overflowItem.activeSelf) HandleOverflowItem();
            RemoveItem();
        }

        public void SellItem()
        {
            playerData.SaveFileData.Money += itemReference.Value;
            tooltipSystem.NewTooltip(5f, "Sold the " + itemReference.ItemName + " for $" + itemReference.Value.ToString("F2"));
            RemoveItem();
        }

        public void ConvertToBait()
        {
            playerData.AddBait(itemReference.ItemName, 1);
            if (manager.overflowItem.activeSelf) HandleOverflowItem();
            tooltipSystem.NewTooltip(5f, "Converted the " + itemReference.ItemName + " into bait");
            if (!playerData.HasSeenTutorialData.BaitTutorial) ShowBaitTutorial();
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
            bucket.BucketList.Remove(itemReference);
            if (menuListingReference != manager.overflowItem) Destroy(menuListingReference);
            if (currentModel != null) Destroy(currentModel);
        }

        public void HandleOverflowItem()
        {
            if (menuListingReference != manager.overflowItem) bucket.AddToBucket(rodManager.EquippedRod.Hook.HookedObject.GetComponent<Fishable>());

            rodManager.EquippedRod.Hook.DestroyHookedObject();
            manager.overflowItem.SetActive(false);
            bucketMenu.ToggleBucketMenu();
        }

        private void ShowBaitTutorial()
        {
            TutorialSystem.instance.QueueTutorial("Bait can help you catch fish that aren't interested in just your hook as is. Close the bucket menu and open the inventory menu (I) to equip it!");
            playerData.HasSeenTutorialData.BaitTutorial = true;
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