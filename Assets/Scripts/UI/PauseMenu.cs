using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!GameController.instance.bucketMenu.gameObject.activeSelf && !GameController.instance.inventoryMenu.gameObject.activeSelf)
            {
                ToggleMenu();
            }
        }
    }

    public void ToggleMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);

        if (pauseMenu.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }

        GameController.instance.bucketMenuButton.gameObject.SetActive(!pauseMenu.gameObject.activeSelf);
        GameController.instance.inventoryMenuButton.gameObject.SetActive(!pauseMenu.gameObject.activeSelf);
    }

    public void NewGame()
    {
        GameController.instance.GetComponent<PlayerData>().NewGame();
    }

    public void SaveGame()
    {
        GameController.instance.GetComponent<PlayerData>().SavePlayer();
    }

    public void LoadGame()
    {
        GameController.instance.GetComponent<PlayerData>().LoadPlayer();
    }

    public void LoadStore()
    {
        //Application.LoadLevel(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
