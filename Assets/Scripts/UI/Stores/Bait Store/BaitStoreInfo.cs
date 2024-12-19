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

            nameText.text = currentBait.BaitName;
            descriptionText.text = currentBait.Description;
            costText.text = currentBait.Cost.ToString("C");

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
            for (int i = 0; i < currentBait.Effects.Count; i++) {
                effects[i].UpdateEffect(currentBait.Effects[i], currentBait.EffectsSprites[i]);
            }
        }

        public void BuyBait() {
            if (SaveManager.Instance.LoadedPlayerData.SaveFileData.Money < currentBait.Cost) {
                TooltipSystem.instance.NewTooltip(5f, "You don't have enough money to buy this bait");
                return;
            }

            TooltipSystem.instance.NewTooltip(5f, $"You bought the {nameText.text} for {currentBait.Cost.ToString("C")}");
            SaveManager.Instance.LoadedPlayerData.SaveFileData.Money -= currentBait.Cost;
            SaveManager.Instance.LoadedPlayerData.AddBait(nameText.text, 1);
            BaitStoreMenu.instance.RefreshStore();
            gameObject.SetActive(false);
        }
    }
}
