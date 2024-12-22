using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Fishing.UI {
    public class DisableCastOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        private static DisableCastOnHover _hoveredUI = null;
        public static bool IsHoveringUI {
            get {
                return _hoveredUI != null;
            }
        }

        public void OnPointerEnter(PointerEventData eventData) {
            _hoveredUI = this;
        }

        public void OnPointerExit(PointerEventData eventData) {
            _hoveredUI = null;
        }

        private void OnDisable() {
            if (_hoveredUI == this) {
                _hoveredUI = null;
            }
        }
    }

}