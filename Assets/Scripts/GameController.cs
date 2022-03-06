using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing
{
    public class GameController : MonoBehaviour
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
        public RodBehaviour equippedRod;

        [SerializeField] private List<GameObject> interuptableUI;

        [SerializeField]
        public List<Transform> foodTransforms;

        public static GameController instance;

        private GameController() => instance = this;

        public bool IsActiveUI()
        {
            foreach(GameObject ui in interuptableUI)
            {
                if (ui.activeSelf)
                {
                    return true;
                }
            }
            return false;
        }
    }

}