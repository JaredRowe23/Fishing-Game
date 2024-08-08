using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.IO
{
    [System.Serializable]
    public struct BucketItemSaveData
    {
        public string itemName;
        public string description;
        public float weight;
        public float length;
        public float value;

        public BucketItemSaveData(string _itemName, string _description, float _weight, float _length, float _value)
        {
            itemName = _itemName;
            description = _description;
            weight = _weight;
            length = _length;
            value = _value;
        }
    }
}
