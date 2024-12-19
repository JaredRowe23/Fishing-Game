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

            nameText.text = currentRodScriptable.RodName;
            descriptionText.text = currentRodScriptable.Description;
            costText.text = currentRodScriptable.Cost.ToString("C");
            reelSpeedText.text = currentRodScriptable.ReelForce.ToString();
            castStrengthText.text = $"{currentRodScriptable.MinCastStrength} / {currentRodScriptable.MaxCastStrength}";
            castAngleText.text = currentRodScriptable.MaxCastAngle.ToString();
            lineLengthText.text = currentRodScriptable.LineLength.ToString();
            strengthFrequencyText.text = currentRodScriptable.ChargeFrequency.ToString();
            angleFrequencyText.text = currentRodScriptable.AngleFrequency.ToString();
        }

        public void BuyRod()
        {
            if (SaveManager.Instance.LoadedPlayerData.SaveFileData.Money < currentRodScriptable.Cost)
            {
                TooltipSystem.instance.NewTooltip(5f, "You don't have enough money to buy this fishing rod");
                return;
            }

            TooltipSystem.instance.NewTooltip(5f, $"You bought the {nameText.text} for {currentRodScriptable.Cost.ToString("C")}");
            SaveManager.Instance.LoadedPlayerData.SaveFileData.Money -= currentRodScriptable.Cost;
            SaveManager.Instance.LoadedPlayerData.FishingRodSaveData.Add(new FishingRodSaveData(nameText.text, "", "", null));
            RodsStoreMenu.instance.RefreshStore();
            gameObject.SetActive(false);
        }
    }
}
