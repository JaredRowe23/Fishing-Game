using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Fishing
{
    public class BlockCastUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData) => UIManager.instance.mouseOverUI = this.gameObject;
        public void OnPointerExit(PointerEventData eventData) => UIManager.instance.mouseOverUI = null;
    }
}
