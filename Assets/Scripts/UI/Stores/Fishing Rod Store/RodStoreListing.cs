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

        [SerializeField] private Image overlayColor;
        [SerializeField] private Text nameText;
        [SerializeField] private Text costText;
        [SerializeField] private Image rodSprite;

        private RodScriptable referenceScriptable;

        private RodStoreInfo infoPanel;

        private void Awake()
        {
            infoPanel = RodStoreInfo.instance;
        }

        public void UpdateInfo (RodScriptable _rod)
        {
            referenceScriptable = _rod;
            nameText.text = _rod.RodName;
            costText.text = _rod.Cost.ToString("C");
            rodSprite.sprite = _rod.InventorySprite;
        }

        public void UpdateColor(ItemStatus status)
        {
            switch (status) {
                case ItemStatus.Available:
                    overlayColor.color = availableColor;
                    break;
                case ItemStatus.Restricted:
                    overlayColor.color = restrictedColor;
                    break;
                case ItemStatus.Purchased:
                    overlayColor.color = purchasedColor;
                    break;
            }
        }
        public void UpdateInfoPanel()
        {
            infoPanel.gameObject.SetActive(true);
            infoPanel.UpdateInfo(referenceScriptable);
        }
    }
}
