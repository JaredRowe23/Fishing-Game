using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Fishing.IO;

namespace Fishing.UI
{
    public class BucketMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Controls _controls;

        public void OnPointerEnter(PointerEventData eventData) => UIManager.instance.mouseOverUI = this.gameObject;
        public void OnPointerExit(PointerEventData eventData) => UIManager.instance.mouseOverUI = null;

        private void Awake()
        {
            _controls = new Controls();
            _controls.FishingLevelInputs.Enable();
            _controls.FishingLevelInputs.BucketMenu.performed += BucketMenu.instance.BucketMenuAction;
        }

        private void OnEnable()
        {
            _controls.FishingLevelInputs.BucketMenu.performed += BucketMenu.instance.BucketMenuAction;
        }

        private void OnDisable()
        {
            _controls.FishingLevelInputs.BucketMenu.performed -= BucketMenu.instance.BucketMenuAction;
        }
    }

}