using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI
{
    public class RodAttachmentButton : MonoBehaviour
    {
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private Text baitName;
        [SerializeField] private Image baitSprite;

        public void UpdateButton(string _name, Sprite _sprite)
        {
            baitName.text = _name;
            baitSprite.sprite = _sprite;
        }

        public void ToggleScrollRect() {
            if (scrollRect.gameObject.activeSelf) {
                HideScrollRect();
            }
            else {
                ShowScrollRect();
            }
        }
        public void ShowScrollRect() {
            scrollRect.gameObject.SetActive(true);
        }
        public void HideScrollRect() {
            scrollRect.gameObject.SetActive(false);
        }
    }

}