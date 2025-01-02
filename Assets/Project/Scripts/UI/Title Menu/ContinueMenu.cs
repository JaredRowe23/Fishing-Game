using Fishing.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Fishing.Util;

namespace Fishing.UI {
    public class ContinueMenu : InactiveSingleton {
        [SerializeField, Tooltip("Reference to the SaveSlotDetails that displays the details of the most recently saved game to continue from.")] private SaveSlotDetails _slotDetails;
        [SerializeField, Tooltip("Reference to the button that loads the most recently saved game.")] private Button _loadButton;
        [SerializeField, Tooltip("Button UI for cancelling continuing the most recently saved game.")] private Button _cancelButton;
        [SerializeField, Tooltip("GameObject that displays to the player that there are no saves to load.")] private GameObject _noSavesText;

        private SaveManager _saveManager;

        private static ContinueMenu _instance;
        public static ContinueMenu Instance { get => _instance; set => _instance = value; }

        private void Start() {
            _loadButton.onClick.AddListener(delegate { ContinueGame(); });
            _cancelButton.onClick.AddListener(delegate { Utilities.SwapActive(MainMenu.Instance.gameObject, gameObject); });
        }

        private void ContinueGame() {
            _saveManager.LoadGame($"{Application.persistentDataPath}/{_saveManager.SaveFiles[0].Name}.fish");
            SceneManager.LoadScene(_saveManager.LoadedPlayerData.SaveFileData.CurrentSceneName);
        }

        private void LoadContinueSlotDetails() {
            _saveManager.LoadSaveSlots();
            if (_saveManager.SaveFiles.Count == 0) {
                DisplayNoSaves();
            }
            else {
                DisplaySave();
            }
        }

        private void DisplayNoSaves() {
            _slotDetails.gameObject.SetActive(false);
            _noSavesText.SetActive(true);
            _loadButton.interactable = false;
        }

        private void DisplaySave() {
            _slotDetails.gameObject.SetActive(true);
            _noSavesText.SetActive(false);
            _loadButton.interactable = true;

            _slotDetails.UpdateInfo(_saveManager.SaveFiles[0]);
        }

        private void OnEnable() {
            LoadContinueSlotDetails();
        }

        public override void SetInstanceReference() {
            Instance = this;
        }

        public override void SetDepenencyReferences() {
            _saveManager = SaveManager.Instance;
        }
    }
}
