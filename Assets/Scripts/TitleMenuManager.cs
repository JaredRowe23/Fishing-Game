using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.IO;
using UnityEngine.SceneManagement;

namespace Fishing.UI
{
    public class TitleMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject continueMenu;
        [SerializeField] private GameObject loadMenu;
        [SerializeField] private GameObject settingsMenu;
        [SerializeField] private GameObject quitMenu;

        public void NewGame()
        {
            //Wipe the current session's player data
            SceneManager.LoadScene("World Map");
        }

        public void ShowLoadMenu() => SwapActive(loadMenu, mainMenu);
        public void HideLoadMenu() => SwapActive(mainMenu, loadMenu);
        public void LoadGame(int _saveSlot)
        {
            //Set the current session's data to match with the selected slot, then call SceneManager.LoadScene("World Map")
        }

        public void Continue()
        {
            //Check for the most recent saved slot, and pass that into LoadGame
        }
        public void ShowContinueMenu()
        {
            //Check and cache the most recent saved slot
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
