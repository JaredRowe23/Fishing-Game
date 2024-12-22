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

        private BaitManager _baitManager;
        private PlayerData _playerData;
        private AudioManager _audioManager;

        private static RodManager _instance;
        public static RodManager Instance { get => _instance; private set => _instance = value; }

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            _baitManager = BaitManager.Instance;
            _playerData = SaveManager.Instance.LoadedPlayerData;
            _audioManager = AudioManager.instance;
            EquipRod(_playerData.EquippedRod.RodName, false);
        }

        public void EquipRod(string rodName, bool playSound) {
            Debug.Assert(RodPrefabs.Count > 0, "Rod prefabs in rod manager not assigned!", this);

            if (string.IsNullOrEmpty(rodName)) {
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

            if (playSound) {
                _audioManager.PlaySound("Equip Rod");
            }

            _baitManager.SpawnBait();
        }
    }
}