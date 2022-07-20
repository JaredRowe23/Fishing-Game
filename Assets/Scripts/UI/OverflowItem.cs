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

            if (_modelReference) Destroy(_modelReference);
            AudioManager.instance.PlaySound("Throwaway Fish");
            UIManager.instance.itemInfoMenu.SetActive(false);
            BucketMenu.instance.RefreshMenu();
            BucketMenu.instance.ShowBucketMenu();
        }
    }
}
