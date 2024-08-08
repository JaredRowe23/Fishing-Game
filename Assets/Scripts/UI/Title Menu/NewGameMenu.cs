using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fishing.IO;
using UnityEngine.SceneManagement;

namespace Fishing.UI
{
    public class NewGameMenu : MonoBehaviour
    {
        [SerializeField] private InputField nameInput;

        public static NewGameMenu instance;

        private NewGameMenu() => instance = this;

        public void StartNewGame()
        {
            PlayerData.instance.NewGame();
            PlayerData.instance.saveFileData.playerName = nameInput.text;
            SceneManager.LoadScene("World Map");
        }
    }
}
