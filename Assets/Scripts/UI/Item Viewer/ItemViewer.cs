using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemViewer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData) => GameController.instance.mouseOverUI = this.gameObject;
    public void OnPointerExit(PointerEventData eventData) => GameController.instance.mouseOverUI = null;
}
