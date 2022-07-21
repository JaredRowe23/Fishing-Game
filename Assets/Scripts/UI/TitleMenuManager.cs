using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fishing.IO;

namespace Fishing.UI
{
    public class TitleMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject newGameMenu;
        [SerializeField] private GameObject continueMenu;
        [SerializeField] private GameObject loadMenu;
        [SerializeField] private GameObject settingsMenu;
        [SerializeField] private GameObject quitMenu;

        public static TitleMenuManager instance;

        private TitleMenuManager() => instance = this;

        private void Awake()
        {
            loadMenu = LoadMenu.instance.gameObject;
        }

        public void NewGame()
        {

        }

        public void ShowNewGameMenu()
        {
            SwapActive(newGameMenu, mainMenu);
        }

        public void HideNewGameMenu()
        {
            SwapActive(mainMenu, newGameMenu);
        }

        public void ShowLoadMenu()
        {
            SwapActive(loadMenu, mainMenu);
            loadMenu.GetComponent<LoadMenu>().GenerateSaveListings();
        }
        public void HideLoadMenu()
        {
            LoadMenu.instance.DestroySaveListings();
            LoadMenu.instance.slotDetails.gameObject.SetActive(false);
            SwapActive(mainMenu, loadMenu);
        }
        public void LoadGame(int _saveSlot)
        {
            SaveManager.LoadGame(Application.persistentDataPath + "/" + SaveManager.saveFiles[_saveSlot].name + ".fish");
            SceneManager.LoadScene(PlayerData.instance.currentSceneName);
        }

        public void Continue()
        {
            LoadGame(0);
        }

        public void ShowContinueMenu()
        {
            SaveManager.LoadSaveSlots();
            ContinueMenu.instance.LoadContinueSlotDetails();
            SwapActive(continueMenu, mainMenu);
        }
        public void HideContinueMenu() => SwapActive(mainMenu, continueMenu);

        public void ShowSettingsMenu() => SwapActive(settingsMenu, mainMenu);
        public void HideSettingsMenu() => SwapActive(mainMenu, settingsMenu);

        public void ShowQuitMenu() => SwapActive(quitMenu, mainMenu);
        public void HideQuitMenu() => SwapActive(mainMenu, quitMenu);
        public void QuitGame() => Application.Quit();

        private void SwapActive(GameObject _setActive, GameObject _setInactive)
        {
            _setActive.SetActive(true);
            _setInactive.SetActive(false);
        }
    }

}
