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
            gameObject.SetActive(!gameObject.activeSelf);
            if (gameObject.activeSelf) UpdateRecords();
            else recordInfoPanel.SetActive(false);

            UIManager.instance.bucketMenuButton.gameObject.SetActive(!gameObject.activeSelf);
            UIManager.instance.inventoryMenuButton.SetActive(!gameObject.activeSelf);
            UIManager.instance.recordMenuButton.gameObject.SetActive(!gameObject.activeSelf);

            UIManager.instance.mouseOverUI = null;
        }

        private void UpdateRecords()
        {
            foreach (RecordsListing _listing in listings)
            {
                _listing.UpdateListing("", 0, 0f, 0f);
                for (int i = 0; i < PlayerData.instance.recordSaveData.Count; i++)
                {
                    if (PlayerData.instance.recordSaveData[i].itemName != _listing.GetFishableName()) continue;

                    _listing.UpdateListing(PlayerData.instance.recordSaveData[i].itemName, PlayerData.instance.recordSaveData[i].amountCaught, PlayerData.instance.recordSaveData[i].lengthRecord, PlayerData.instance.recordSaveData[i].weightRecord);
                    break;
                }
            }
        }

        public void ShowRecordInfoPanel(string _name, int _catchAmount, float _length, float _weight, Sprite _sprite)
        {
            recordInfoPanel.SetActive(true);

            recordInfoImage.sprite = _sprite;

            if (_name == "")
            {
                recordInfoName.text = "???";
                recordCatchAmount.text = "0";
                recordInfoLength.text = "Catch this fish to track \n your record length";
                recordInfoWeight.text = "Catch this fish to track \n your record weight";
                recordInfoImage.color = Color.black;
            }
            else
            {
                recordInfoName.text = _name;
                recordCatchAmount.text = "Caught: " + _catchAmount.ToString();
                recordInfoLength.text = "Length Record: \n" + _length.ToString() + "cm";
                recordInfoWeight.text = "Weight Record: \n" + _weight.ToString() + "kg";
                recordInfoImage.color = Color.white;
            }
        }
    }
}
