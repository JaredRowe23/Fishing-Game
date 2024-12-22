using Fishing.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing {
    public class UIManager : MonoBehaviour {
        private List<InactiveSingleton> _inactiveSingletons;
        [SerializeField] private Button bucketMenuButton;
        public GameObject mouseOverUI;
        public GameObject itemInfoMenu;
        public BucketMenuItem overflowItem;
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

        private void Awake() {
            instance = this;
        }

        private void OnEnable() { // Using OnOnEnable for this instead of Awake as this shouldn't be set to inactive at any point, and this allows the scene to instantiate everything in Awake first, before their depenencies are required in Start
            _inactiveSingletons = new List<InactiveSingleton>(FindObjectsOfType<InactiveSingleton>(true));
            for (int i = 0; i < _inactiveSingletons.Count; i++) {
                _inactiveSingletons[i].SetInstanceReference();
            }
        }
        private void Start() {
            for (int i = 0; i < _inactiveSingletons.Count; i++) {
                _inactiveSingletons[i].SetDepenencyReferences();
            }
        }

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