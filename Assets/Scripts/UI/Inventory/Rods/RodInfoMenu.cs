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
            UpdateRodInfo(RodManager.Instance.EquippedRod);
        }

        public void UpdateRodInfo(RodBehaviour _rod)
        {
            reference = _rod;
            rodSprite.sprite = _rod.InventorySprite;
            rodName.text = _rod.Scriptable.rodName;
            rodDescription.text = _rod.Scriptable.description;

            if (_rod.EquippedBait != null) {
                baitButton.UpdateButton(_rod.EquippedBait.Scriptable.baitName, _rod.EquippedBait.Scriptable.inventorySprite);
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
            for (int i = 0; i < SaveManager.Instance.LoadedPlayerData.BaitSaveData.Count; i++) {
                BaitAttachmentSlot _newSlot = Instantiate(baitOptionPrefab, baitsScrollRect.content.transform).GetComponent<BaitAttachmentSlot>();
                _newSlot.baitSaveData = SaveManager.Instance.LoadedPlayerData.BaitSaveData[i];
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