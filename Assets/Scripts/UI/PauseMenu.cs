using Fishing.IO;
using Fishing.PlayerInput;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Fishing.UI {
    public class PauseMenu : MonoBehaviour {
        [SerializeField, Tooltip("Reference to the gameobject that actually holds the pause UI.")] private GameObject _pauseUI;
        public GameObject PauseUI { get => _pauseUI; private set { _pauseUI = value; } }

        [SerializeField, Tooltip("Button UI for unpausing the game.")] private Button _unpauseButton;
        [SerializeField, Tooltip("Button UI for unpausing the game.")] private Button _saveButton;
        [SerializeField, Tooltip("Button UI for unpausing the game.")] private Button _mapButton;
        [SerializeField, Tooltip("Button UI for unpausing the game.")] private Button _titleScreenButton;
        [SerializeField, Tooltip("Button UI for unpausing the game.")] private Button _quitGameButton;

        private PlayerData _playerData;

        private static PauseMenu _instance;
        public static PauseMenu Instance { get => _instance; private set { _instance = value; } }

        private void Awake() {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start() {
            InputManager.OnPauseMenu += ToggleMenu;
            _playerData = SaveManager.Instance.LoadedPlayerData;

            _unpauseButton.onClick.AddListener(delegate { UnpauseGame(); });
            _saveButton.onClick.AddListener(delegate { SaveGame(); });
            _mapButton.onClick.AddListener(delegate { OpenMap(); });
            _titleScreenButton.onClick.AddListener(delegate { ExitToTitle(); });
            _quitGameButton.onClick.AddListener(delegate { QuitGame(); });
        }

        public void ToggleMenu() {
            if (_pauseUI.gameObject.activeSelf) {
                UnpauseGame();
            }
            else {
                PauseGame();
            }
        }

        private void PauseGame() {
            _pauseUI.gameObject.SetActive(true);
            if (SceneManager.GetActiveScene().handle == 2) {
                UIManager.Instance.HideHUDButtons();
            }

            AudioManager.instance.PlaySound("Pause");
            Time.timeScale = 0f;
        }

        private void UnpauseGame() {
            _pauseUI.gameObject.SetActive(false);
            if (SceneManager.GetActiveScene().handle == 2) {
                UIManager.Instance.ShowHUDButtons();
            }

            AudioManager.instance.PlaySound("Unpause");
            Time.timeScale = 1f;
        }

        public void SaveGame() {
            _playerData.SavePlayer();
            ToggleMenu();
        }

        public void OpenMap() {
            UnpauseGame();
            InputManager.ClearListeners();
            SceneManager.LoadScene("World Map");
        }

        public void ExitToTitle() {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Title Screen");
            Destroy(gameObject);
        }

        public void QuitGame() {
            Application.Quit();
        }

        private void OnDestroy() {
            InputManager.OnPauseMenu -= ToggleMenu;
        }
    }
}