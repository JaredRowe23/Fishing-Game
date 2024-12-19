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

        private BaitScriptable currentBait;

        public static BaitInfoMenu instance;

        private BaitInfoMenu() => instance = this;

        public void UpdateBaitInfoMenu(BaitScriptable _bait)
        {
            currentBait = _bait;
            baitName.text = currentBait.BaitName;
            baitSprite.sprite = currentBait.InventorySprite;
            baitDescription.text = currentBait.Description;
            baitAttractionList.text = GetAttractionListText();
            UpdateEffectsListings();
        }

        private string GetAttractionListText() {
            string _attractionListText = "";
            List<string> _foodTypes = currentBait.GetFoodTypesAsString();
            for (int i = 0; i < _foodTypes.Count; i++) {
                _attractionListText += _foodTypes[i];
                if (i == _foodTypes.Count - 1) {
                    _attractionListText += ".";
                }
                else {
                    _attractionListText += ", ";
                }
            }
            return _attractionListText;
        }

        private void UpdateEffectsListings() {
            for (int i = 0; i < baitEffectsListings.Count; i++) {
                baitEffectsListings[i].DisableListing();
            }
            for (int i = 0; i < currentBait.Effects.Count; i++) {
                baitEffectsListings[i].UpdateEffect(currentBait.Effects[i], currentBait.EffectsSprites[i]);
            }
        }
    }
}
