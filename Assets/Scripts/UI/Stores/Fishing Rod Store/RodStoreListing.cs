using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fishing.FishingMechanics;

namespace Fishing.UI
{
    public class RodStoreListing : MonoBehaviour
    {
        public enum ItemStatus { Available, Restricted, Purchased };
        public ItemStatus status;

        public Color availableColor;
        public Color restrictedColor;
        public Color purchasedColor;

        public RodScriptable referenceScriptable;

        [SerializeField] private Image overlayColor;
        [SerializeField] private Text nameText;
        [SerializeField] private Text costText;
        [SerializeField] private Image rodSprite;

        private RodStoreInfo infoPanel;

        private void Awake()
        {
            infoPanel = RodStoreInfo.instance;
        }

        public void UpdateInfo (RodScriptable _rod)
        {
            referenceScriptable = _rod;
            nameText.text = _rod.rodName;
            costText.text = "$" + _rod.cost.ToString();
            rodSprite.sprite = _rod.inventorySprite;
        }

        public void UpdateColor(ItemStatus status)
        {
            if (status == ItemStatus.Available)
            {
                overlayColor.color = availableColor;
            }
            else if (status == ItemStatus.Restricted)
            {
                overlayColor.color = restrictedColor;
            }
            else if (status == ItemStatus.Purchased)
            {
                overlayColor.color = purchasedColor;
            }
        }
        public void UpdateInfoPanel()
        {
            infoPanel.gameObject.SetActive(true);
            infoPanel.UpdateInfo(referenceScriptable);
        }
    }
}
