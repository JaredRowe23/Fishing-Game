using Fishing.IO;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Fishing.Util;

namespace Fishing.UI {
    public class LoadMenu : InactiveSingleton {
        [SerializeField, Tooltip("Prefab for displaying information for each save game file found.")] private GameObject _saveFileListingPrefab;
        [SerializeField, Tooltip("ScrollRect UI that lists every save file listing.")] private ScrollRect _saveFileListings;

        [SerializeField, Tooltip("Button UI for loading the selected save file.")] private Button _loadButton;
        [SerializeField, Tooltip("Button UI for deleting the selected save file.")] private Button _deleteButton;
        [SerializeField, Tooltip("Button UI for cancelling loading the selected save file.")] private Button _cancelButton
            ;
        [SerializeField, Tooltip("UI object for displaying the selected save slot's details.")] private SaveSlotDetails _slotDetails;
        private int _selectedSlotIndex;

        private SaveManager _saveManager;

        private static LoadMenu _instance;
        public static LoadMenu Instance { get => _instance; set => _instance = value; }

        private void Start() {
            _loadButton.onClick.AddListener(delegate { LoadSaveSlot(); });
            _deleteButton.onClick.AddListener(delegate { DeleteSaveSlot(); });
            _cancelButton.onClick.AddListener(delegate { Utilities.SwapActive(MainMenu.Instance.gameObject, gameObject); });
        }

        private void GenerateSaveListings() {
            _saveManager.LoadSaveSlots();

            for (int i = 0; i < _saveManager.SaveFiles.Count; i++) {
                SaveSlotListing _newSlot = Instantiate(_saveFileListingPrefab, _saveFileListings.content).GetComponent<SaveSlotListing>();
                _newSlot.SetInfo(_saveManager.SaveFiles[i].Name, _saveManager.SaveFiles[i].DateTime, i);
                _newSlot.GetComponent<Button>().onClick.AddListener(delegate { SelectSlot(_newSlot.SaveIndex); });
            }
        }

        private void SelectSlot(int slotIndex) {
            _slotDetails.transform.parent.gameObject.SetActive(true);
            _slotDetails.UpdateInfo(_saveManager.SaveFiles[slotIndex]);
            _selectedSlotIndex = slotIndex;
        }

        private void LoadSaveSlot() {
            _saveManager.LoadGame($"{Application.persistentDataPath}/{_saveManager.SaveFiles[_selectedSlotIndex].Name}.fish");
            SceneManager.LoadScene(_saveManager.LoadedPlayerData.SaveFileData.CurrentSceneName);
        }

        private void DestroySaveListings() {
            SaveSlotListing[] _saveSlots = _saveFileListings.content.transform.GetComponentsInChildren<SaveSlotListing>();
            for (int i = 0; i < _saveSlots.Length; i++) {
                Destroy(_saveSlots[i].gameObject);
            }
        }

        private void DeleteSaveSlot() {
            string _path = $"{Application.persistentDataPath}/{_saveManager.SaveFiles[_selectedSlotIndex].Name}.fish";
            if (File.Exists(_path)) {
                File.Delete(_path);
                RefreshSaveSlotListings();
                _slotDetails.transform.parent.gameObject.SetActive(false);
            }
            else {
                Debug.LogError($"Save file not found in {Application.persistentDataPath}/{_saveManager.SaveFiles[_selectedSlotIndex].Name}.fish");
            }
        }

        private void RefreshSaveSlotListings() {
            DestroySaveListings();
            GenerateSaveListings();
        }

        private void OnEnable() {
            GenerateSaveListings();
        }

        private void OnDisable() {
            DestroySaveListings();
        }

        public override void SetInstanceReference() {
            Instance = this;
        }

        public override void SetDepenencyReferences() {
            _saveManager = SaveManager.Instance;
        }
    }
}
