using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.IO;
using Fishing.Inventory;

namespace Fishing.UI
{
    public class OverflowItem : MonoBehaviour
    {
        [SerializeField] private BucketBehaviour bucket;

        private RodManager rodManager;
        public static OverflowItem instance;

        private OverflowItem() => instance = this;


        private void Awake()
        {
            rodManager = RodManager.instance;
        }
        public void ThrowAway(FishData _itemReference, GameObject _modelReference, GameObject _menuItem)
        {
            if (UIManager.instance.overflowItem.activeSelf) // This can be changed to be self referential rather than going through UIManager
            {
                bucket.bucketList.Remove(_itemReference);

                if (_menuItem != UIManager.instance.overflowItem)
                {
                    rodManager.equippedRod.GetHook().AddToBucket();
                    Destroy(_menuItem);
                }
                else
                {
                    TooltipSystem.instance.NewTooltip(5f, "Threw away the " + _itemReference.itemName + " you just caught");
                }

                rodManager.equippedRod.GetHook().DespawnHookedObject();
                _menuItem.SetActive(false);
                UIManager.instance.overflowItem.SetActive(false);
            }

            if (_modelReference) Destroy(_modelReference);
            AudioManager.instance.PlaySound("Throwaway Fish");
            UIManager.instance.itemInfoMenu.SetActive(false);
            BucketMenu.instance.RefreshMenu();
            BucketMenu.instance.ToggleBucketMenu();
        }

        public void ConvertToBait(FishData _itemReference, GameObject _modelReference, GameObject _menuItem)
        {
            if (UIManager.instance.overflowItem.activeSelf)
            {
                bucket.bucketList.Remove(_itemReference);

                if (_menuItem != UIManager.instance.overflowItem)
                {
                    rodManager.equippedRod.GetHook().AddToBucket();
                    Destroy(_menuItem);
                }

                rodManager.equippedRod.GetHook().DespawnHookedObject();
                _menuItem.SetActive(false);
                UIManager.instance.overflowItem.SetActive(false);
            }

            PlayerData.instance.AddBait(_itemReference.itemName);
            TooltipSystem.instance.NewTooltip(5f, "Converted the " + _itemReference.itemName + " into bait");
            if (_modelReference) Destroy(_modelReference);
            AudioManager.instance.PlaySound("Throwaway Fish");
            UIManager.instance.itemInfoMenu.SetActive(false);
            BucketMenu.instance.RefreshMenu();
            BucketMenu.instance.ToggleBucketMenu();
        }
    }
}
