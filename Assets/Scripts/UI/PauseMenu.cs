using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.IO;
using UnityEngine.SceneManagement;
using Fishing.PlayerInput;

namespace Fishing.UI
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject pauseMenu;

        private PlayerData playerData;

        public static PauseMenu instance;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            playerData = SaveManager.Instance.LoadedPlayerData;

            InputManager.OnPauseMenu += ToggleMenu;
        }

        public void ToggleMenu()
        {
            if (SceneManager.GetActiveScene().handle == 2)
            {
                if (BucketMenu.instance.gameObject.activeSelf) return;
                if (InventoryMenu.instance.gameObject.activeSelf) return;
            }

            if (pauseMenu.activeSelf) {
                UnpauseGame();
            }
            else {
                PauseGame();
            }
        }

        private void PauseGame() {
            pauseMenu.SetActive(true);
            if (SceneManager.GetActiveScene().handle == 2) {
                UIManager.instance.HideHUDButtons();
            }
            AudioManager.instance.PlaySound("Pause");
            Time.timeScale = 0f;
        }

        private void UnpauseGame() {
            pauseMenu.SetActive(false);
            if (SceneManager.GetActiveScene().handle == 2) {
                UIManager.instance.ShowHUDButtons();
            }
            AudioManager.instance.PlaySound("Unpause");
            Time.timeScale = 1f;
        }

        public void SaveGame() {
            playerData.SavePlayer();
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

        public void QuitGame() => Application.Quit();
    }

}