using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.IO;
using UnityEngine.UI;

namespace Fishing.UI
{
    public class RecordsMenu : MonoBehaviour
    {
        [SerializeField] private GameObject recordInfoPanel;
        [SerializeField] private List<RecordsListing> listings;

        [SerializeField] private Image recordInfoImage;
        [SerializeField] private Text recordInfoName;
        [SerializeField] private Text recordCatchAmount;
        [SerializeField] private Text recordInfoLength;
        [SerializeField] private Text recordInfoWeight;

        public static RecordsMenu instance;

        private RecordsMenu() => instance = this;

        public void ToggleRecordsMenu()
        {
            if (gameObject.activeSelf) {
                HideRecordsMenu();
            }
            else {
                ShowRecordsMenu();
            }
        }

        private void ShowRecordsMenu() {
            gameObject.SetActive(true);
            UIManager.instance.HideHUDButtons();
            UIManager.instance.mouseOverUI = null;
            UpdateRecords();
        }
        private void HideRecordsMenu() {
            gameObject.SetActive(false);
            UIManager.instance.mouseOverUI = null;
            UIManager.instance.ShowHUDButtons();
            recordInfoPanel.SetActive(false);
        }

        private void UpdateRecords()
        {
            for (int i = 0; i < listings.Count; i++) {
                listings[i].ResetListing();
                for (int j = 0; j < SaveManager.Instance.LoadedPlayerData.RecordSaveData.Count; j++) {
                    RecordSaveData _recordSaveData = SaveManager.Instance.LoadedPlayerData.RecordSaveData[j];
                    if (_recordSaveData.ItemName != listings[i].FishableName) continue;
                    if (_recordSaveData.AmountCaught == 0) continue;

                    listings[i].UpdateListing(_recordSaveData);
                    break;
                }
            }
        }

        public void ShowRecordInfoPanel(RecordSaveData _recordData, Sprite _sprite)
        {
            recordInfoPanel.SetActive(true);

            recordInfoImage.sprite = _sprite;

            if (_recordData.AmountCaught == 0)
            {
                recordInfoName.text = "???";
                recordCatchAmount.text = "0";
                recordInfoLength.text = "Catch this fish to track your record length.";
                recordInfoWeight.text = "Catch this fish to track your record weight.";
                recordInfoImage.color = Color.black;
            }
            else
            {
                recordInfoName.text = _recordData.ItemName;
                recordCatchAmount.text = $"Caught: {_recordData.AmountCaught}";
                recordInfoLength.text = $"Length Record: \n{_recordData.LengthRecord}cm";
                recordInfoWeight.text = $"Weight Record: \n{_recordData.WeightRecord}kg";
                recordInfoImage.color = Color.white;
            }
        }
    }
}
