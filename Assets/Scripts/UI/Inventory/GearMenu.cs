using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.UI
{
    public class GearMenu : MonoBehaviour, IInventoryTab
    {
        public void ShowGearMenu() {
            InventoryMenu.instance.UpdateActiveMenu(gameObject);
        }

        public void ShowTab() {
        }

        public void HideTab() {
        }
    }

}