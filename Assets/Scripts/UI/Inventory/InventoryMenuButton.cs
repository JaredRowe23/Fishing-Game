using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Fishing.IO;

namespace Fishing.UI
{
    public class InventoryMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData) => UIManager.instance.mouseOverUI = this.gameObject;
        public void OnPointerExit(PointerEventData eventData) => UIManager.instance.mouseOverUI = null;

        private Controls _controls;

        private void Awake()
        {
            _controls = new Controls();
            _controls.FishingLevelInputs.Enable();
            _controls.FishingLevelInputs.BackpackMenu.performed += InventoryMenu.instance.InventoryMenuAction;
        }

        private void OnEnable()
        {
            _controls.FishingLevelInputs.BackpackMenu.performed += InventoryMenu.instance.InventoryMenuAction;
        }

        private void OnDisable()
        {
            _controls.FishingLevelInputs.BackpackMenu.performed -= InventoryMenu.instance.InventoryMenuAction;
        }
    }

}