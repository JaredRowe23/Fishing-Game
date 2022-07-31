using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fishing.FishingMechanics;
using Fishing.IO;

namespace Fishing.UI
{
    public class RodInfoMenu : MonoBehaviour
    {
        [SerializeField] private Image rodSprite;
        [SerializeField] private Text rodName;
        [SerializeField] private Text rodDescription;

        [SerializeField] private RodAttachmentButton baitButton;
        [SerializeField] private RodAttachmentButton hookButton;
        [SerializeField] private RodAttachmentButton lineButton;

        [SerializeField] private GameObject baitsContent;
        [SerializeField] private GameObject hooksContent;
        [SerializeField] private GameObject linesContent;

        [SerializeField] private GameObject baitAttachmentSlot;
        [SerializeField] private GameObject hookAttachmentSlot;
        [SerializeField] private GameObject lineAttachmentSlot;

        private RodBehaviour reference;

        public static RodInfoMenu instance;

        private RodInfoMenu() => instance = this;

        private void Awake()
        {
            UpdateRodInfo(RodManager.instance.equippedRod);
        }

        public void UpdateRodInfo(RodBehaviour _rod)
        {
            reference = _rod;
            rodSprite.sprite = _rod.inventorySprite;
            rodName.text = _rod.scriptable.rodName;
            rodDescription.text = _rod.scriptable.description;

            if (_rod.equippedBait != null) baitButton.UpdateButton(_rod.equippedBait.scriptable.baitName, _rod.equippedBait.scriptable.inventorySprite);
            else baitButton.UpdateButton("No Bait", null);

            UpdateBaitOptions();
            UpdateHookOptions();
            UpdateLineOptions();
        }

        public void UpdateBaitOptions()
        {
            foreach(Transform _child in baitsContent.transform)
            {
                if (_child.GetComponent<BaitAttachmentSlot>().baitScriptable == null) continue;
                if (_child.GetComponent<BaitAttachmentSlot>())
                {
                    Destroy(_child.gameObject);
                }
            }

            int i = 0;
            if (PlayerData.instance.bait.Count == 0) return;
            foreach (string _bait in PlayerData.instance.bait)
            {
                BaitAttachmentSlot _newSlot = Instantiate(baitAttachmentSlot, baitsContent.transform).GetComponent<BaitAttachmentSlot>();
                _newSlot.baitScriptable = ItemLookupTable.instance.StringToBait(_bait);
                _newSlot.UpdateSlot();
                _newSlot.countText.text = "x" + PlayerData.instance.baitCounts[i].ToString();

                i++;
            }
        }

        public void UpdateHookOptions()
        {

        }
        public void UpdateLineOptions()
        {

        }

        public void HideButtonScrollViews()
        {
            baitButton.SetScrollView(false);
            hookButton.SetScrollView(false);
            lineButton.SetScrollView(false);
        }
    }

}