using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI
{
    public class BaitEffectsListing : MonoBehaviour
    {
        [SerializeField] private Text effectName;
        [SerializeField] private Image effectSprite;

        public void DisableListing() {
            effectName.text = "";
            effectSprite.sprite = null;
            gameObject.SetActive(false);
        }

        public void UpdateEffect(string _effectName, Sprite _effectSprite)
        {
            if (string.IsNullOrEmpty(_effectName) || _effectSprite == null) {
                DisableListing();
                return;
            }
            gameObject.SetActive(true);
            effectName.text = _effectName;
            effectSprite.sprite = _effectSprite;
        }
    }
}
