using UnityEngine;
using UnityEngine.UI;
using Fishing.Util;

namespace Fishing.UI {
    public class QuitMenu : InactiveSingleton {
        [SerializeField, Tooltip("Button UI for confirming quiting the game.")] private Button _quitButton;
        [SerializeField, Tooltip("Button UI for cancelling quiting the game.")] private Button _cancelButton;

        private static QuitMenu _instance;
        public static QuitMenu Instance { get => _instance; private set { _instance = value; } }

        private void Start() {
            _quitButton.onClick.AddListener(delegate { QuitGame(); });
            _cancelButton.onClick.AddListener(delegate { Utilities.SwapActive(MainMenu.Instance.gameObject, gameObject); });
        }

        private void QuitGame() {
            Application.Quit();
        }

        public override void SetInstanceReference() {
            Instance = this;
        }

        public override void SetDepenencyReferences() {
            // No required dependencies
        }
    }
}