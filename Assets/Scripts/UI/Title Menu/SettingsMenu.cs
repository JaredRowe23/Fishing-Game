using UnityEngine;
using UnityEngine.UI;
using Fishing.Util;

namespace Fishing.UI {
    public class SettingsMenu : InactiveSingleton {
        [SerializeField, Tooltip("Button UI for exiting the settings menu.")] private Button _exitSettingsButton;

        private static SettingsMenu _instance;
        public static SettingsMenu Instance { get => _instance; private set { _instance = value; } }

        private void Start() {
            _exitSettingsButton.onClick.AddListener(delegate { Utilities.SwapActive(MainMenu.Instance.gameObject, gameObject); });
        }

        public override void SetInstanceReference() {
            Instance = this;
        }

        public override void SetDepenencyReferences() {
            // No required dependencies
        }
    }
}