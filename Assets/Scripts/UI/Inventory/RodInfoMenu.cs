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

        public void UpdateRodInfo(RodBehaviour _rod)
        {
            rodSprite.sprite = _rod.inventorySprite;
            rodName.text = _rod.gameObject.name;
            rodDescription.text = _rod.GetDescription();
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