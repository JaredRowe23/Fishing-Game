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

        [SerializeField] private ScrollRect baitsScrollRect;
        [SerializeField] private ScrollRect hooksScrollRect;
        [SerializeField] private ScrollRect linesScrollRect;

        [SerializeField] private GameObject baitOptionPrefab;
        [SerializeField] private GameObject hookOptionPrefab;
        [SerializeField] private GameObject lineOptionPrefab;

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

            if (_rod.equippedBait != null) {
                baitButton.UpdateButton(_rod.equippedBait.Scriptable.baitName, _rod.equippedBait.Scriptable.inventorySprite);
            }
            else {
                baitButton.UpdateButton("No Bait", null);
            }

            UpdateBaitOptions();
            UpdateHookOptions();
            UpdateLineOptions();
        }

        public void UpdateBaitOptions()
        {
            DestroyBaitOptions();
            GenerateBaitOptions();
        }

        private void DestroyBaitOptions() {
            BaitAttachmentSlot[] _baitOptions = baitsScrollRect.content.transform.GetComponentsInChildren<BaitAttachmentSlot>();
            for (int i = 0; i < _baitOptions.Length; i++) {
                if (_baitOptions[i].baitScriptable == null) continue;
                Destroy(_baitOptions[i].gameObject);
            }
        }

        private void GenerateBaitOptions() {
            for (int i = 0; i < PlayerData.instance.baitSaveData.Count; i++) {
                BaitAttachmentSlot _newSlot = Instantiate(baitOptionPrefab, baitsScrollRect.content.transform).GetComponent<BaitAttachmentSlot>();
                _newSlot.baitSaveData = PlayerData.instance.baitSaveData[i];
                _newSlot.UpdateSlot();
            }
        }

        public void UpdateHookOptions()
        {

        }
        public void UpdateLineOptions()
        {

        }

        public void HideAttachmentScrollRects()
        {
            baitButton.HideScrollRect();
            hookButton.HideScrollRect();
            lineButton.HideScrollRect();
        }
    }

}