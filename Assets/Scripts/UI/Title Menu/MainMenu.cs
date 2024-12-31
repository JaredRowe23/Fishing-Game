using Fishing.UI;
using UnityEngine;
using UnityEngine.UI;
using Fishing.Util;

namespace Fishing.UI {
    public class MainMenu : MonoBehaviour {
        [SerializeField, Tooltip("Button that shows the new game menu.")] private Button _newGameMenuButton;
        [SerializeField, Tooltip("Button that shows the continue menu.")] private Button _continueMenuButton;
        [SerializeField, Tooltip("Button that shows the load game menu.")] private Button _loadMenuButton;
        [SerializeField, Tooltip("Button that shows the settings menu.")] private Button _settingsMenuButton;
        [SerializeField, Tooltip("Button that shows the quit menu.")] private Button _quitMenuButton;

        private static MainMenu _instance;
        public static MainMenu Instance { get => _instance; private set { _instance = value; } }

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            _newGameMenuButton.onClick.AddListener(delegate { Utilities.SwapActive(NewGameMenu.Instance.gameObject, gameObject); });
            _continueMenuButton.onClick.AddListener(delegate { Utilities.SwapActive(ContinueMenu.Instance.gameObject, gameObject); });
            _loadMenuButton.onClick.AddListener(delegate { Utilities.SwapActive(LoadMenu.Instance.gameObject, gameObject); });
            _settingsMenuButton.onClick.AddListener(delegate { Utilities.SwapActive(SettingsMenu.Instance.gameObject, gameObject); });
            _quitMenuButton.onClick.AddListener(delegate { Utilities.SwapActive(QuitMenu.Instance.gameObject, gameObject); });
        }
    }
}