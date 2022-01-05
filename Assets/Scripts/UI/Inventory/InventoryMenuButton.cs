using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameController.instance.mouseOverUI = this.gameObject;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        GameController.instance.mouseOverUI = null;
    }
}
