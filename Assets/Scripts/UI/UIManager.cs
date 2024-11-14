using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fishing.UI;

namespace Fishing
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Button bucketMenuButton;
        public GameObject mouseOverUI;
        public GameObject itemInfoMenu;
        public GameObject overflowItem;
        public GameObject itemViewer;
        public Canvas rodCanvas;
        [SerializeField] private GameObject inventoryMenuButton;
        public GameObject rodMenuButton;
        public GameObject baitMenuButton;
        public GameObject gearMenuButton;
        public GameObject rodsMenu;
        [SerializeField] private Button recordMenuButton;

        [SerializeField] public List<GameObject> interuptableUI;

        public static UIManager instance;

        private UIManager() => instance = this;

        public bool IsActiveUI() {
            foreach (GameObject _ui in interuptableUI) {
                if (_ui.activeSelf) {
                    return true;
                }
            }

            if (PauseMenu.instance.pauseMenu.gameObject.activeSelf) return true;

            return false;
        }

        public void ShowHUDButtons() {
            bucketMenuButton.gameObject.SetActive(true);
            inventoryMenuButton.SetActive(true);
            recordMenuButton.gameObject.SetActive(true);
        }
        public void HideHUDButtons() {
            bucketMenuButton.gameObject.SetActive(false);
            inventoryMenuButton.SetActive(false);
            recordMenuButton.gameObject.SetActive(false);
        }
    }

}