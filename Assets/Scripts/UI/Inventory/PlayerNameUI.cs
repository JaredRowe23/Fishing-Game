using Fishing.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing {
    public class PlayerNameUI : MonoBehaviour {
        [SerializeField, Tooltip("Text UI that displays the player's name (which is also the save file name)")] private Text _playerNameText;

        private void Awake() {
            _playerNameText.text = SaveManager.Instance.LoadedPlayerData.SaveFileData.PlayerName;
        }
    }
}
