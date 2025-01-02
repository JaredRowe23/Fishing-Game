using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI {
    public class BaitEffectsListing : MonoBehaviour {
        [SerializeField, Tooltip("Text UI displaying the name of the effect.")] private Text _effectName;
        [SerializeField, Tooltip("Image UI displaying the sprite of the effect.")] private Image _effectSprite;

        public void DisableListing() {
            _effectName.text = "";
            _effectSprite.sprite = null;
            gameObject.SetActive(false);
        }

        public void UpdateEffect(string effectName, Sprite effectSprite)
        {
            if (string.IsNullOrEmpty(effectName) || effectSprite == null) {
                DisableListing();
                return;
            }

            gameObject.SetActive(true);
           _effectName.text = effectName;
           _effectSprite.sprite = effectSprite;
        }

        private void OnDisable() {
            DisableListing();
        }
    }
}