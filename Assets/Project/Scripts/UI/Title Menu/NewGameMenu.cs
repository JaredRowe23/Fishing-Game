using Fishing.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Fishing.Util;

namespace Fishing.UI {
    public class NewGameMenu : InactiveSingleton {
        [SerializeField, Tooltip("InputField UI that receive's the player's name for the save file.")] private InputField _nameInput;
        [SerializeField, Tooltip("Button UI for starting the new game.")] private Button _startNewGameButton;
        [SerializeField, Tooltip("Button UI for cancelling starting a new game")] private Button _cancelButton;

        private static NewGameMenu _instance;
        public static NewGameMenu Instance { get => _instance; set => _instance = value; }

        private void Start() {
            _startNewGameButton.onClick.AddListener(delegate { StartNewGame(); });
            _cancelButton.onClick.AddListener(delegate { Utilities.SwapActive(MainMenu.Instance.gameObject, gameObject); });
        }

        private void StartNewGame() {
            PlayerData _newPlayerData = new PlayerData();
            _newPlayerData.NewGame();
            _newPlayerData.SaveFileData.PlayerName = _nameInput.text;
            SaveManager.Instance.LoadedPlayerData = _newPlayerData;
            SceneManager.LoadScene(_newPlayerData.SaveFileData.CurrentSceneName);
        }

        public override void SetInstanceReference() {
            Instance = this;
        }

        public override void SetDepenencyReferences() {
            // No required dependencies
        }
    }
}
