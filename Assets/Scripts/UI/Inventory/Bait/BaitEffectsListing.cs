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

        public void UpdateEffect(string _effectName, Sprite _effectSprite)
        {
            if (_effectName == "")
            {
                effectName.text = "";
                effectSprite.sprite = null;
            }
            effectName.text = _effectName;
            effectSprite.sprite = _effectSprite;
        }
    }
}
