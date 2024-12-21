using Fishing.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing {
    public class UIManager : MonoBehaviour {
        [SerializeField, Tooltip("List of singleton objects that are inactive at the start of the scene but need to have their references set at the beginning.")] private List<IInactiveSingleton> _inactiveSingletons;
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