using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fishing.FishingMechanics;

namespace Fishing.UI
{
    public class RodInfoMenu : MonoBehaviour
    {
        [SerializeField] private Image rodSprite;
        [SerializeField] private Text rodName;
        [SerializeField] private Text rodDescription;

        public void UpdateRodInfo(RodBehaviour rod)
        {
            rodSprite.sprite = rod.inventorySprite;
            rodName.text = rod.gameObject.name;
            rodDescription.text = rod.GetDescription();
        }

        public void UpdateBaitOptions()
        {

        }

        public void UpdateHookOptions()
        {

        }
        public void UpdateLineOptions()
        {

        }
    }

}