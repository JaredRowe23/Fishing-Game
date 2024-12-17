using UnityEngine;

namespace Fishing.IO {
    [System.Serializable]
    public class BucketItemSaveData {
        [SerializeField] private string _itemName;
        public string ItemName { get => _itemName; private set => _itemName = value; }

        [SerializeField] private string _description;
        public string Description { get => _description; private set => _description = value; }

        [SerializeField] private float _weight;
        public float Weight { get => _weight; private set => _weight = value; }

        [SerializeField] private float _length;
        public float Length { get => _length; private set => _length = value; }

        [SerializeField] private float _value;
        public float Value { get => _value; private set => _value = value; }

        public BucketItemSaveData(string itemName, string description, float weight, float length, float value) {
            ItemName = itemName;
            Description = description;
            Weight = weight;
            Length = length;
            Value = value;
        }

        public bool CompareTo(BucketItemSaveData other) {
            return other != null && 
                ItemName == other.ItemName && 
                Description == other.Description && 
                Weight == other.Weight && 
                Length == other.Length && 
                Value == other.Value;
        }
    }
}
