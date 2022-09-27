using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Fishing.UI;
using UnityEngine.InputSystem;
using Fishing.IO;

namespace Fishing.WorldMap
{
    public class TravelManager : MonoBehaviour
    {
        [SerializeField] private GameObject confirmationWindow;
        [SerializeField] private Text travelText;
        [SerializeField] private MenuNavigation mapNavi;
        private string potentialSceneName;

        private Controls _controls;

        private void Awake()
        {
            _controls = new Controls();
        }

        public void ShowConfirmationWindow(string _potentialSceneName)
        {
            potentialSceneName = _potentialSceneName;
            travelText.text = "Travel to " + potentialSceneName + "?";
            mapNavi.enabled = false;
            confirmationWindow.SetActive(true);
        }
        public void HideConfirmationWindow()
        {
            confirmationWindow.SetActive(false);
            mapNavi.enabled = true;
        }

        public void Travel() => SceneManager.LoadScene(potentialSceneName);
    }
}
