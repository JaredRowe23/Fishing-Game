using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fishing.IO;
using UnityEngine.SceneManagement;
using System.IO;

namespace Fishing.UI
{
    public class NewGameMenu : MonoBehaviour
    {
        [SerializeField] private InputField nameInput;

        public static NewGameMenu instance;

        private NewGameMenu() => instance = this;

        public void StartNewGame() {
            SaveManager.Instance.LoadedPlayerData = new PlayerData();
            SaveManager.Instance.LoadedPlayerData.NewGame();
            SaveManager.Instance.LoadedPlayerData.SaveFileData.PlayerName = nameInput.text;
            SceneManager.LoadScene(SaveManager.Instance.LoadedPlayerData.SaveFileData.CurrentSceneName);
        }
    }
}
