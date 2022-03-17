using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Fishing.WorldMap
{
    public class TravelManager : MonoBehaviour
    {
        [SerializeField] private GameObject confirmationWindow;
        [SerializeField] private Text travelText;
        private string potentialSceneName;

        public void ShowConfirmationWindow(string _potentialSceneName)
        {
            potentialSceneName = _potentialSceneName;
            travelText.text = "Travel to " + potentialSceneName + "?";
            confirmationWindow.SetActive(true);
        }
        public void HideConfirmationWindow() => confirmationWindow.SetActive(false);

        public void Travel() => SceneManager.LoadScene(potentialSceneName);
    }
}
