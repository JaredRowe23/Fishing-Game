using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fishing.FishingMechanics;

namespace Fishing.UI
{
    public class BaitInfoMenu : MonoBehaviour
    {
        [SerializeField] private Text baitName;
        [SerializeField] private Image baitSprite;
        [SerializeField] private Text baitDescription;
        [SerializeField] private Text baitAttractionList;
        [SerializeField] public List<BaitEffectsListing> baitEffectsListings;

        public static BaitInfoMenu instance;

        private BaitInfoMenu() => instance = this;

        public void UpdateBaitInfoMenu(BaitScriptable _bait)
        {
            baitName.text = _bait.baitName;
            baitSprite.sprite = _bait.inventorySprite;
            baitDescription.text = _bait.description;

            baitAttractionList.text = "";
            if (_bait.GetFoodTypesAsString() != null)
            {
                foreach (string _str in _bait.GetFoodTypesAsString())
                {
                    baitAttractionList.text += _str + ", ";
                }
                baitAttractionList.text = baitAttractionList.text.Substring(0, baitAttractionList.text.Length - 2);
                baitAttractionList.text += '.';
            }

            foreach (BaitEffectsListing _effectListing in baitEffectsListings)
            {
                _effectListing.UpdateEffect("", null);
                _effectListing.gameObject.SetActive(false);
            }
            for (int i = 0; i < _bait.effects.Count; i++)
            {
                baitEffectsListings[i].UpdateEffect(_bait.effects[i], _bait.effectsSprites[i]);
                baitEffectsListings[i].gameObject.SetActive(true);
            }
        }
    }
}
