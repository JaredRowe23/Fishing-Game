using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI
{
    public class RodAttachmentButton : MonoBehaviour
    {
        [SerializeField] private ScrollRect scrollView;
        [SerializeField] private Text baitName;
        [SerializeField] private Image baitSprite;

        public void UpdateButton(string _name, Sprite _sprite)
        {
            baitName.text = _name;
            baitSprite.sprite = _sprite;
        }

        public void ToggleScrollView() => scrollView.gameObject.SetActive(!scrollView.gameObject.activeSelf);
        public void SetScrollView(bool _setActive) => scrollView.gameObject.SetActive(_setActive);
    }

}