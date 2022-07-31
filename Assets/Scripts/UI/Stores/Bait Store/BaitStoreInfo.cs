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

        public static BaitStoreInfo instance;

        private BaitStoreInfo() => instance = this;

        public void UpdateInfo(BaitScriptable _bait)
        {
            nameText.text = _bait.baitName;
            descriptionText.text = _bait.description;
            costText.text = "$" + _bait.cost.ToString();
            attractsText.text = "";

            foreach (string _str in _bait.GetFoodTypesAsString())
            {
                attractsText.text += _str + ", ";
            }
            attractsText.text = attractsText.text.Substring(0, attractsText.text.Length - 2);
            attractsText.text += '.';

            foreach (BaitEffectsListing _effectListing in effects)
            {
                _effectListing.UpdateEffect("", null);
                _effectListing.gameObject.SetActive(false);
            }
            for (int i = 0; i < _bait.effects.Count; i++)
            {
                effects[i].UpdateEffect(_bait.effects[i], _bait.effectsSprites[i]);
                effects[i].gameObject.SetActive(true);
            }
        }

        public void BuyBait()
        {
            if (PlayerData.instance.money < float.Parse(costText.text))
            {
                TooltipSystem.instance.NewTooltip(5f, "You don't have enough money to buy this bait");
                return;
            }

            TooltipSystem.instance.NewTooltip(5f, "You bought the " + nameText.text + " for $" + costText.text);
            PlayerData.instance.money -= float.Parse(costText.text);
            PlayerData.instance.AddBait(nameText.text);
            BaitStoreMenu.instance.RefreshStore();
            gameObject.SetActive(false);
        }
    }
}
