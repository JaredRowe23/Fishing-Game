using Fishing.FishingMechanics;
using Fishing.IO;
using Fishing.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing {
    public class RodManager : MonoBehaviour {
        [SerializeField, Tooltip("Prefabs of the fishing rods to spawn in the world when equipped.")] private List<GameObject> _rodPrefabs;
        public List<GameObject> RodPrefabs { get => _rodPrefabs; private set => _rodPrefabs = value; }

        private RodBehaviour _equippedRod;
        public RodBehaviour EquippedRod { get => _equippedRod; private set => _equippedRod = value; }

        private PlayerData _playerData;
        private RodsMenu _rodsMenu;
        private TooltipSystem _tooltipSystem;
        private AudioManager _audioManager;
        private ItemLookupTable _itemLookupTable;

        private static RodManager _instance;
        public static RodManager Instance { get => _instance; private set => _instance = value; }

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            _rodsMenu = RodsMenu.instance;
            _playerData = PlayerData.instance;
            _tooltipSystem = TooltipSystem.instance;
            _audioManager = AudioManager.instance;
            _itemLookupTable = ItemLookupTable.instance;
            EquipRod(PlayerData.instance.equippedRod.rodName, false);
        }

        public void EquipRod(string rodName, bool playSound) {
            Debug.Assert(RodPrefabs.Count > 0, "Rod prefabs in rod manager not assigned!", this);

            if (rodName == "") {
                rodName = RodPrefabs[0].name;
            }

            if (EquippedRod != null) {
                DestroyImmediate(EquippedRod.gameObject);
                EquippedRod = null;
            }

            foreach (GameObject _prefab in RodPrefabs) {
                if (_prefab.name != rodName) {
                    continue;
                }

                EquippedRod = Instantiate(_prefab).GetComponent<RodBehaviour>();
            }

            if (EquippedRod == null) {
                EquippedRod = Instantiate(RodPrefabs[0].GetComponent<RodBehaviour>());
            }

            for (int i = 0; i < _playerData.fishingRodSaveData.Count; i++) {
                if (_playerData.fishingRodSaveData[i].rodName != rodName) {
                    continue;
                }

                _playerData.equippedRod = _playerData.fishingRodSaveData[i];
            }

            _tooltipSystem.NewTooltip(5f, "Equipped the " + rodName);
            _rodsMenu.UpdateEquippedCheckmark();
            if (playSound) {
                _audioManager.PlaySound("Equip Rod");
            }

            SpawnBait();
        }

        public void SpawnBait() {
            if (_playerData.equippedRod.equippedBait == null) {
                return;
            }
            if (string.IsNullOrEmpty(_playerData.equippedRod.equippedBait.baitName)) {
                return;
            }

            BaitBehaviour _newBait = Instantiate(_itemLookupTable.StringToBait(_playerData.equippedRod.equippedBait.baitName).prefab, EquippedRod.Hook.transform).GetComponent<BaitBehaviour>();
            EquippedRod.EquippedBait = _newBait;
            EquippedRod.Hook.HookedObject = _newBait.gameObject;
            _newBait.transform.localPosition = _newBait.AnchorPoint;
            _newBait.transform.localRotation = Quaternion.Euler(0f, 0f, _newBait.AnchorRotation);
        }
    }

}