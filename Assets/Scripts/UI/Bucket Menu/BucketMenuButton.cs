// The sole purpose for this script is to tell whether
// the mouse is over the bucket button's UI element in order
// to stop the rod from casting when trying to open the menu

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BucketMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
