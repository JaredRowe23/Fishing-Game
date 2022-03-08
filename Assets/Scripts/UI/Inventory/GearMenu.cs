using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.UI
{
    public class GearMenu : MonoBehaviour
    {
        public void ShowRodMenu()
        {
            gameObject.SetActive(!gameObject.activeSelf);
            InventoryMenu.instance.UpdateActiveMenu(2);
        }
    }

}