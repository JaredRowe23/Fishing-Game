using Fishing.FishingMechanics;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI {
    public class BaitInfoMenu : InactiveSingleton {
        [SerializeField, Tooltip("Text UI displaying the name of the selected bait.")] private Text _baitName;
        [SerializeField, Tooltip("Image UI displaying the sprite of the selected bait.")] private Image _baitSprite;
        [SerializeField, Tooltip("Text UI displaying the description of the selected bait.")] private Text _baitDescription;
        [SerializeField, Tooltip("Text UI dispalying each fish type the selected bait attracts.")] private Text _baitAttractionList;
        [SerializeField, Tooltip("List of BaitEffectListing prefabs that display info on each effect of the selected bait.")] private List<BaitEffectsListing> _baitEffectsListings;

        private BaitScriptable _currentBait;

        private static BaitInfoMenu _instance;
        public static BaitInfoMenu Instance { get => _instance; private set => _instance = value; }

        public void UpdateBaitInfoMenu(BaitScriptable bait) {
            _currentBait = bait;
            _baitName.text = _currentBait.BaitName;
            _baitSprite.sprite = _currentBait.InventorySprite;
            _baitDescription.text = _currentBait.Description;
            _baitAttractionList.text = $"{string.Join(", ", _currentBait.GetFoodTypesAsString())}.";
            UpdateEffectsListings();
        }

        private void UpdateEffectsListings() {
            for (int i = 0; i < _baitEffectsListings.Count; i++) {
                if (i < _currentBait.Effects.Count) {
                    _baitEffectsListings[i].UpdateEffect(_currentBait.Effects[i], _currentBait.EffectsSprites[i]);
                }
                else {
                    _baitEffectsListings[i].DisableListing();
                }
            }
        }

        public override void SetInstanceReference() {
            Instance = this;
        }

        public override void SetDepenencyReferences() {
            // No required dependencies
        }

        private void OnDisable() {
            gameObject.SetActive(false);
        }
    }
}
