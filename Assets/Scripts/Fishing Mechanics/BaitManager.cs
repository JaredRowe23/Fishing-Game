using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.IO;

namespace Fishing.FishingMechanics {
    public class BaitManager : MonoBehaviour {
        private RodManager _rodManager;
        private PlayerData _playerData;

        private static BaitManager _instance;
        public static BaitManager Instance { get => _instance; private set { _instance = value; } }

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            _playerData = SaveManager.Instance.LoadedPlayerData;
            _rodManager = RodManager.Instance;
        }

        public void SpawnBait() {
            if (_playerData.EquippedRod.EquippedBait == null) {
                return;
            }
            if (string.IsNullOrEmpty(_playerData.EquippedRod.EquippedBait.BaitName)) {
                return;
            }

            BaitBehaviour _newBait = Instantiate(ItemLookupTable.Instance.StringToBaitScriptable(_playerData.EquippedRod.EquippedBait.BaitName).Prefab, _rodManager.EquippedRod.Hook.transform).GetComponent<BaitBehaviour>();
            _rodManager.EquippedRod.EquippedBait = _newBait;
            _rodManager.EquippedRod.Hook.HookedObject = _newBait.gameObject;
            _newBait.transform.localPosition = _newBait.AnchorPoint;
            _newBait.transform.localRotation = Quaternion.Euler(0f, 0f, _newBait.AnchorRotation);
        }
    }
}