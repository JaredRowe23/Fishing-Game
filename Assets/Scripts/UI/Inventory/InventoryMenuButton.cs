using Fishing.PlayerInput;
using UnityEngine;

namespace Fishing.UI {
    public class InventoryMenuButton : MonoBehaviour {
        private void OnEnable() {
            InputManager.OnInventoryMenu += InventoryMenu.Instance.ToggleInventoryMenu;
        }

        private void OnDisable() {
            InputManager.OnInventoryMenu -= InventoryMenu.Instance.ToggleInventoryMenu;
        }
    }

}