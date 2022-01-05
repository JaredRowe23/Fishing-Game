// This holds and adds objects we fish to our bucket

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketBehaviour : MonoBehaviour
{
    public List<FishData> bucketList = new List<FishData>();
    public int maxItems;

    private void Start()
    {
        bucketList = new List<FishData>();
    }

    public void AddToBucket(FishableItem item)
    {
        FishData newItem = new FishData();
        newItem.itemName = item.GetName();
        newItem.itemDescription = item.GetDescription();
        newItem.itemWeight = item.GetWeight();
        newItem.itemLength = item.GetLength();
        bucketList.Add(newItem);
    }
}
