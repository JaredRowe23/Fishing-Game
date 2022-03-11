using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.Fishables;
using Fishing.UI;

namespace Fishing.Inventory
{
    public class BucketBehaviour : MonoBehaviour
    {
        public List<FishData> bucketList = new List<FishData>();
        public int maxItems;

        public static BucketBehaviour instance;

        private void Awake() => instance = this;

        private void Start() => bucketList = new List<FishData>();

        public void AddToBucket(FishableItem _item)
        {
            AudioManager.instance.PlaySound("Catch Fish");

            FishData _newItem = new FishData();
            {
                _newItem.itemName = _item.GetName();
                _newItem.itemDescription = _item.GetDescription();
                _newItem.itemWeight = _item.GetWeight();
                _newItem.itemLength = _item.GetLength();
            }

            if (bucketList.Count >= maxItems)
            {
                BucketMenu.instance.ShowBucketMenu();
                GameController.instance.overflowItem.SetActive(true);
                BucketMenuItem _overflowMenu = GameController.instance.overflowItem.GetComponent<BucketMenuItem>();

                _overflowMenu.UpdateName(_item.GetName());
                _overflowMenu.UpdateLength(_newItem.itemLength);
                _overflowMenu.UpdateWeight(_newItem.itemWeight);

                _overflowMenu.UpdateReference(_newItem);

                return;
            }

            AudioManager.instance.PlaySound("Add To Bucket");

            _item.DisableMinimapIndicator();
            bucketList.Add(_newItem);

            _item.GetComponent<IEdible>().Despawn();
        }
    }

}