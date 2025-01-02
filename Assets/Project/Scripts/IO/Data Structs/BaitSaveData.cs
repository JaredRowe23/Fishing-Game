using UnityEngine;

namespace Fishing.IO {
    [System.Serializable]
    public class BaitSaveData {
        [SerializeField] private string _baitName;
        public string BaitName { get => _baitName; private set => _baitName = value; }

        [SerializeField] private int _amount;
        public int Amount { get => _amount; set => _amount = value; }

        public BaitSaveData(string baitName, int amount) {
            BaitName = baitName;
            Amount = amount;
        }
    }
}
