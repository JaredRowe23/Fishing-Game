using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.IO;

namespace Fishing.UI
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject pauseMenu;

        public static PauseMenu instance;

        private PauseMenu() => instance = this;

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

            GameController.instance.bucketMenuButton.gameObject.SetActive(!pauseMenu.activeSelf);
            GameController.instance.inventoryMenuButton.SetActive(!pauseMenu.activeSelf);
        }

        public void NewGame() => GameController.instance.GetComponent<PlayerData>().NewGame();

        public void SaveGame() => GameController.instance.GetComponent<PlayerData>().SavePlayer();

        public void LoadGame() => GameController.instance.GetComponent<PlayerData>().LoadPlayer();

        public void LoadStore()
        {
            //Application.LoadLevel(1);
        }

        public void QuitGame() => Application.Quit();
    }

}