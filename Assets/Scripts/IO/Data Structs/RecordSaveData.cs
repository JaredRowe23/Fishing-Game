using UnityEngine;

namespace Fishing.IO {
    [System.Serializable]
    public class RecordSaveData {
        [SerializeField] private string _itemName;
        public string ItemName { get => _itemName; private set => _itemName = value; }

        [SerializeField] private int _amountCaught;
        public int AmountCaught { get => _amountCaught; set => _amountCaught = value; }

        [SerializeField] private float _lengthRecord;
        public float LengthRecord { get => _lengthRecord; set => _lengthRecord = value; }

        [SerializeField] private float _weightRecord;
        public float WeightRecord { get => _weightRecord; set => _weightRecord = value; }

        public RecordSaveData(string itemName, int amountCaught, float lengthRecord, float weightRecord) {
            ItemName = itemName;
            AmountCaught = amountCaught;
            LengthRecord = lengthRecord;
            WeightRecord = weightRecord;
        }
    }
}
