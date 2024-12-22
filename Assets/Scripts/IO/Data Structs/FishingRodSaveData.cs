using UnityEngine;
using UnityEngine.Events;

namespace Fishing.IO {
    [System.Serializable]
    public class FishingRodSaveData {
        [SerializeField] private string _rodName;
        public string RodName { get => _rodName; private set => _rodName = value; }

        [SerializeField] private string _equippedLine;
        public string EquippedLine { get => _equippedLine; set => _equippedLine = value; }

        [SerializeField] private string _equippedHook;
        public string EquippedHook { get => _equippedHook; set => _equippedHook = value; }

        public UnityAction ChangedEquippedBait;
        [SerializeField] private BaitSaveData _equippedBait;
        public BaitSaveData EquippedBait {
            get => _equippedBait;
            set {
                if (_equippedBait != value) {
                    ChangedEquippedBait?.Invoke();
                }
                _equippedBait = value;
            }
        }

        public FishingRodSaveData(string rodName, string equippedLine, string equippedHook, BaitSaveData equippedBait) {
            RodName = rodName;
            EquippedLine = equippedLine;
            EquippedHook = equippedHook;
            EquippedBait = equippedBait;
        }
    }
}
