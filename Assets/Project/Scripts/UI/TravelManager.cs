using Fishing.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Fishing.WorldMap {
    public class TravelManager : MonoBehaviour {
        [SerializeField, Tooltip("Reference to the object that displays buttons to confirm travelling to the selected area.")] private GameObject _confirmationWindow;
        [SerializeField, Tooltip("Text UI that asks the player if they want  to.")] private Text _travelText;
        private string _potentialSceneName;

        private PlayerData _playerData;

        private void Awake() {
            _playerData = SaveManager.Instance.LoadedPlayerData;
        }

        public void ShowConfirmationWindow(string potentialSceneName) {
            _potentialSceneName = potentialSceneName;
            _travelText.text = $"Travel to {_potentialSceneName}?";
            _confirmationWindow.SetActive(true);
        }
        public void HideConfirmationWindow() {
            _confirmationWindow.SetActive(false);
        }

        public void Travel() {
            _playerData.SaveFileData.CurrentSceneName = _potentialSceneName;
            SceneManager.LoadScene(_potentialSceneName);
        }
    }
}
