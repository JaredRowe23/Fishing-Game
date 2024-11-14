using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fishing.FishingMechanics;

namespace Fishing.UI
{
    public class BaitStoreListing : MonoBehaviour
    {
        public enum ItemStatus { Available, Restricted };
        public ItemStatus status;

        public Color availableColor;
        public Color restrictedColor;

        public BaitScriptable referenceScriptable;

        [SerializeField] private Image overlayColor;
        [SerializeField] private Text nameText;
        [SerializeField] private Text costText;
        [SerializeField] private Text attractsText;
        [SerializeField] private Text effectsText;
        [SerializeField] private Image baitSprite;

        private BaitStoreInfo infoPanel;

        private void Awake()
        {
            infoPanel = BaitStoreInfo.instance;
        }

        public void UpdateInfo(BaitScriptable _bait)
        {
            referenceScriptable = _bait;

            nameText.text = referenceScriptable.baitName;
            costText.text = referenceScriptable.cost.ToString("C");
            if (referenceScriptable.GetFoodTypesAsString() != null) attractsText.text = $"Attracts: x{_bait.GetFoodTypesAsString().Count}";
            effectsText.text = $"Effects: x{referenceScriptable.effects.Count}";
            baitSprite.sprite = referenceScriptable.inventorySprite;
        }

        public void UpdateColor(ItemStatus status) {
            switch (status) {
                case ItemStatus.Available:
                    overlayColor.color = availableColor;
                    break;
                case ItemStatus.Restricted:
                    overlayColor.color = restrictedColor;
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
