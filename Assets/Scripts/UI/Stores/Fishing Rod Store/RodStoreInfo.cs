using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fishing.FishingMechanics;
using Fishing.IO;

namespace Fishing.UI
{
    public class RodStoreInfo : MonoBehaviour
    {
        [SerializeField] private Text nameText;
        [SerializeField] private Text descriptionText;
        [SerializeField] private Text costText;
        [SerializeField] private Text reelSpeedText;
        [SerializeField] private Text castStrengthText;
        [SerializeField] private Text castAngleText;
        [SerializeField] private Text lineLengthText;
        [SerializeField] private Text strengthFrequencyText;
        [SerializeField] private Text angleFrequencyText;

        public static RodStoreInfo instance;

        private RodStoreInfo() => instance = this;

        public void UpdateInfo(RodScriptable _rod)
        {
            nameText.text = _rod.rodName;
            descriptionText.text = _rod.description;
            costText.text = "$" + _rod.cost.ToString();
            reelSpeedText.text = _rod.reelSpeed.ToString();
            castStrengthText.text = _rod.minCastStrength + "/" + _rod.maxCastStrength;
            castAngleText.text = _rod.maxCastAngle.ToString();
            lineLengthText.text = _rod.lineLength.ToString();
            strengthFrequencyText.text = _rod.chargeFrequency.ToString();
            angleFrequencyText.text = _rod.angleFrequency.ToString();
        }

        public void BuyRod()
        {
            if (PlayerData.instance.money < float.Parse(costText.text))
            {
                TooltipSystem.instance.NewTooltip(5f, "You don't have enough money to buy this fishing rod");
                return;
            }

            TooltipSystem.instance.NewTooltip(5f, "You bought the " + nameText.text + " for $" + costText.text);
            PlayerData.instance.money -= float.Parse(costText.text);
            PlayerData.instance.AddRod(nameText.text);
            RodsStoreMenu.instance.RefreshStore();
            gameObject.SetActive(false);
        }
    }
}
