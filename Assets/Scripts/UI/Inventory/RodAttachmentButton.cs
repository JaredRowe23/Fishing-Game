using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing
{
    public class RodAttachmentButton : MonoBehaviour
    {
        [SerializeField] private ScrollRect scrollView;

        public void ToggleScrollView() => scrollView.gameObject.SetActive(!scrollView.gameObject.activeSelf);
    }

}