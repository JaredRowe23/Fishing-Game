using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.Fishables;

namespace Fishing.Inventory
{
    public class BucketBehaviour : MonoBehaviour
    {
        public List<FishData> bucketList = new List<FishData>();
        public int maxItems;

        public static BucketBehaviour instance;

        private void Awake() => instance = this;

        private void Start() => bucketList = new List<FishData>();

        public void AddToBucket(FishableItem item)
        {
            FishData newItem = new FishData();
            {
                newItem.itemName = item.GetName();
                newItem.itemDescription = item.GetDescription();
                newItem.itemWeight = item.GetWeight();
                newItem.itemLength = item.GetLength();
            }
            item.DisableMinimapIndicator();
            bucketList.Add(newItem);
        }
    }

}