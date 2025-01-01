using Fishing.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing {
    public class UIManager : MonoBehaviour {
        [SerializeField, Tooltip("Button UI that shows the bucket menu when pressed.")] private Button _bucketMenuButton;
        [SerializeField, Tooltip("Button UI that shows the inventory menu when pressed.")] private Button _inventoryMenuButton;
        [SerializeField, Tooltip("Button UI that shows the record menu when pressed.")] private Button _recordMenuButton;

        [SerializeField, Tooltip("Bucket Menu Item that displays when catching a fishable item while your bucket is already full.")] private BucketMenuItem _overflowItem;
        public BucketMenuItem OverflowItem { get => _overflowItem; set => _overflowItem = value; }

        [SerializeField] private List<GameObject> _interuptableUI;

        private static UIManager _instance;
        public static UIManager Instance { get => _instance; set => _instance = value; }

        private void Awake() {
            Instance = this;
        }

        public bool IsActiveUI() {
            foreach (GameObject ui in _interuptableUI) {
                if (ui.activeSelf) {
                    return true;
                }
            }

            if (PauseMenu.Instance.PauseUI.activeSelf) {
                return true;
            }

            return false;
        }

        public void ShowHUDButtons() {
            _bucketMenuButton.gameObject.SetActive(true);
            _inventoryMenuButton.gameObject.SetActive(true);
            _recordMenuButton.gameObject.SetActive(true);
        }
        public void HideHUDButtons() {
            _bucketMenuButton.gameObject.SetActive(false);
            _inventoryMenuButton.gameObject.SetActive(false);
            _recordMenuButton.gameObject.SetActive(false);
        }
    }

}