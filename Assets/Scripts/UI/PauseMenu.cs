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

        private PauseMenu() => instance = this;

        private void Start()
        {
            playerData = UIManager.instance.GetComponent<PlayerData>();
        }

        void Update()
        {
            if (BucketMenu.instance.gameObject.activeSelf) return;
            if (InventoryMenu.instance.gameObject.activeSelf) return;

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

            UIManager.instance.bucketMenuButton.gameObject.SetActive(!pauseMenu.activeSelf);
            UIManager.instance.inventoryMenuButton.SetActive(!pauseMenu.activeSelf);
        }

        public void NewGame() => playerData.NewGame();

        public void SaveGame() => playerData.SavePlayer();

        public void LoadGame() => playerData.LoadPlayer();

        public void LoadStore()
        {
            //Application.LoadLevel(1);
        }

        public void ExitToTitle()
        {
            SceneManager.LoadScene("Title Screen");
        }

        public void QuitGame() => Application.Quit();
    }

}