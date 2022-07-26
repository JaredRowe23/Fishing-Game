using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.IO;
using UnityEngine.SceneManagement;

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

            playerData = PlayerData.instance;
        }

        void Update()
        {
            if (SceneManager.GetActiveScene().handle == 2)
            {
                if (BucketMenu.instance.gameObject.activeSelf) return;
                if (InventoryMenu.instance.gameObject.activeSelf) return;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleMenu();
            }
        }

        public void ToggleMenu()
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);

            if (pauseMenu.activeSelf)
            {
                AudioManager.instance.PlaySound("Pause");
                Time.timeScale = 0f;
            }
            else
            {
                AudioManager.instance.PlaySound("Unpause");
                Time.timeScale = 1f;
            }

            if (SceneManager.GetActiveScene().handle == 2)
            {
                UIManager.instance.bucketMenuButton.gameObject.SetActive(!pauseMenu.activeSelf);
                UIManager.instance.inventoryMenuButton.SetActive(!pauseMenu.activeSelf);
            }
        }

        public void SaveGame()
        {
            playerData.SavePlayer();
            ToggleMenu();
        }

        public void OpenMap()
        {
            Time.timeScale = 1f;
            ToggleMenu();
            SceneManager.LoadScene("World Map");
        }

        public void ExitToTitle()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Title Screen");
            Destroy(gameObject);
        }

        public void QuitGame() => Application.Quit();
    }

}