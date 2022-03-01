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
        public GameObject equippedRod;

        [SerializeField]
        public List<Transform> foodTransforms;

        public static GameController instance;

        private GameController() => instance = this;
    }

}