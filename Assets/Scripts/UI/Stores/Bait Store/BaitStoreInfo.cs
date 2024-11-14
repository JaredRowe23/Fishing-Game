using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fishing.FishingMechanics;
using Fishing.IO;

namespace Fishing.UI
{
    public class BaitStoreInfo : MonoBehaviour
    {
        [SerializeField] private Text nameText;
        [SerializeField] private Text descriptionText;
        [SerializeField] private Text costText;
        [SerializeField] private Text attractsText;
        [SerializeField] private List<BaitEffectsListing> effects;

        private BaitScriptable currentBait;

        public static BaitStoreInfo instance;

        private BaitStoreInfo() => instance = this;

        public void UpdateInfo(BaitScriptable _bait)
        {
            currentBait = _bait;

            nameText.text = currentBait.baitName;
            descriptionText.text = currentBait.description;
            costText.text = currentBait.cost.ToString("C");

            attractsText.text = "";
            List<string> _foodTypes = currentBait.GetFoodTypesAsString();
            for (int i = 0; i < _foodTypes.Count; i++) {
                attractsText.text += _foodTypes[i];
                if (i == _foodTypes.Count - 1) {
                    attractsText.text += ".";
                }
                else {
                    attractsText.text += ", ";
                }
            }

            for (int i = 0; i < effects.Count; i++) {
                effects[i].DisableListing();
            }
            for (int i = 0; i < currentBait.effects.Count; i++) {
                effects[i].UpdateEffect(currentBait.effects[i], currentBait.effectsSprites[i]);
            }
        }

        public void BuyBait() {
            if (PlayerData.instance.saveFileData.money < currentBait.cost) {
                TooltipSystem.instance.NewTooltip(5f, "You don't have enough money to buy this bait");
                return;
            }

            TooltipSystem.instance.NewTooltip(5f, $"You bought the {nameText.text} for {currentBait.cost.ToString("C")}");
            PlayerData.instance.saveFileData.money -= currentBait.cost;
            PlayerData.instance.AddBait(nameText.text);
            BaitStoreMenu.instance.RefreshStore();
            gameObject.SetActive(false);
        }
    }
}
