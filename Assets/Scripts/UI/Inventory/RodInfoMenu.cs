using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing
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
            rodDescription.text = rod.description;
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