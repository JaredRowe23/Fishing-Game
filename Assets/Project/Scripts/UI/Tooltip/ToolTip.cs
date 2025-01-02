using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI {
    public class ToolTip : MonoBehaviour {
        [SerializeField, Tooltip("Text UI that displays the tooltip message.")] private Text _tipText;
        private float _lifetime;

        public void InitializeToolTip(float lifetime, string tipText) {
            _lifetime = lifetime;
            _tipText.text = tipText;
            StartCoroutine(Co_ShowTooltip());
        }

        private IEnumerator Co_ShowTooltip() {
            yield return new WaitForSeconds(_lifetime);
            Destroy(gameObject);
        }
    }
}
