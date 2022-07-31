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
            nameText.text = _bait.baitName;
            costText.text = "$" + _bait.cost.ToString();
            attractsText.text = "Attracts: x" + _bait.GetFoodTypesAsString().Count.ToString();
            effectsText.text = "Effects: x" + _bait.effects.Count.ToString();
            baitSprite.sprite = _bait.inventorySprite;
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
        }
        public void UpdateInfoPanel()
        {
            infoPanel.gameObject.SetActive(true);
            infoPanel.UpdateInfo(referenceScriptable);
        }
    }
}
