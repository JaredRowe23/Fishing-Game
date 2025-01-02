using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.IO;
using Fishing.UI;

namespace Fishing.FishingMechanics {
    public class BaitManager : MonoBehaviour {
        private RodManager _rodManager;
        private PlayerData _playerData;
        private TooltipSystem _tooltipSystem;

        private static BaitManager _instance;
        public static BaitManager Instance { get => _instance; private set { _instance = value; } }

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            _playerData = SaveManager.Instance.LoadedPlayerData;
            _rodManager = RodManager.Instance;
            _tooltipSystem = TooltipSystem.Instance;
        }

        public void SpawnBait() {
            if (_playerData.EquippedRod.EquippedBait == null) {
                return;
            }
            if (string.IsNullOrEmpty(_playerData.EquippedRod.EquippedBait.BaitName)) {
                return;
            }

            if (_playerData.EquippedRod.EquippedBait.Amount <= 0) {
                _tooltipSystem.NewTooltip("Out of bait: " + _playerData.EquippedRod.EquippedBait.BaitName);
                _playerData.BaitSaveData.Remove(_playerData.EquippedRod.EquippedBait);
                _playerData.EquippedRod.EquippedBait = null;
                return;
            }

            BaitBehaviour _newBait = Instantiate(ItemLookupTable.Instance.StringToBaitScriptable(_playerData.EquippedRod.EquippedBait.BaitName).Prefab, _rodManager.EquippedRod.Hook.transform).GetComponent<BaitBehaviour>();
            _rodManager.EquippedRod.EquippedBait = _newBait;
            _rodManager.EquippedRod.Hook.HookedObject = _newBait.gameObject;
            _newBait.transform.localPosition = _newBait.AnchorPoint;
            _newBait.transform.localRotation = Quaternion.Euler(0f, 0f, _newBait.AnchorRotation);

            _playerData.EquippedRod.EquippedBait.Amount--;
        }
    }
}