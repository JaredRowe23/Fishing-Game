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

        private RodScriptable currentRodScriptable;

        public static RodStoreInfo instance;

        private RodStoreInfo() => instance = this;

        public void UpdateInfo(RodScriptable _rod)
        {
            currentRodScriptable = _rod;

            nameText.text = currentRodScriptable.rodName;
            descriptionText.text = currentRodScriptable.description;
            costText.text = currentRodScriptable.cost.ToString("C");
            reelSpeedText.text = currentRodScriptable.reelSpeed.ToString();
            castStrengthText.text = $"{currentRodScriptable.minCastStrength} / {currentRodScriptable.maxCastStrength}";
            castAngleText.text = currentRodScriptable.maxCastAngle.ToString();
            lineLengthText.text = currentRodScriptable.lineLength.ToString();
            strengthFrequencyText.text = currentRodScriptable.chargeFrequency.ToString();
            angleFrequencyText.text = currentRodScriptable.angleFrequency.ToString();
        }

        public void BuyRod()
        {
            if (PlayerData.instance.saveFileData.money < currentRodScriptable.cost)
            {
                TooltipSystem.instance.NewTooltip(5f, "You don't have enough money to buy this fishing rod");
                return;
            }

            TooltipSystem.instance.NewTooltip(5f, $"You bought the {nameText.text} for {currentRodScriptable.cost.ToString("C")}");
            PlayerData.instance.saveFileData.money -= currentRodScriptable.cost;
            PlayerData.instance.fishingRodSaveData.Add(new FishingRodSaveData(nameText.text, "", "", null));
            RodsStoreMenu.instance.RefreshStore();
            gameObject.SetActive(false);
        }
    }
}
