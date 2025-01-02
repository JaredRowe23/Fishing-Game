using UnityEngine;

namespace Fishing.UI {
    public class GearMenu : MonoBehaviour {
        public void ShowGearMenu() {
            InventoryMenu.Instance.UpdateActiveMenu(gameObject);
        }
    }
}