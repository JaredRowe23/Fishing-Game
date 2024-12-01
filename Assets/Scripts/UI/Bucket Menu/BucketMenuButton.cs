using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Fishing.PlayerInput;

namespace Fishing.UI
{
    public class BucketMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {

        public void OnPointerEnter(PointerEventData eventData) => UIManager.instance.mouseOverUI = this.gameObject;
        public void OnPointerExit(PointerEventData eventData) => UIManager.instance.mouseOverUI = null;

        private void Awake()
        {

            InputManager.onBucketMenu += BucketMenu.instance.ToggleBucketMenu;
        }
    }

}