using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing
{
    public class UIManager : MonoBehaviour
    {
        public Button bucketMenuButton;
        public GameObject mouseOverUI;
        public GameObject itemInfoMenu;
        public GameObject overflowItem;
        public GameObject itemViewer;
        public Canvas rodCanvas;
        public GameObject inventoryMenuButton;
        public GameObject rodMenuButton;
        public GameObject baitMenuButton;
        public GameObject gearMenuButton;
        public GameObject rodsMenu;

        [SerializeField] private List<GameObject> interuptableUI;

        public static UIManager instance;

        private UIManager() => instance = this;

        public bool IsActiveUI()
        {
            foreach (GameObject _ui in interuptableUI)
            {
                if (_ui.activeSelf)
                {
                    return true;
                }
            }
            return false;
        }
    }

}