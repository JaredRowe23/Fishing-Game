using Fishing.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI {
    public class RecordsMenu : InactiveSingleton {
        [SerializeField, Tooltip("GameObject for the panel that holds more detailed information on the selected fishable's records.")] private GameObject _recordInfoPanel;
        [SerializeField, Tooltip("ScrollRect for where the listings of records will populate.")] private ScrollRect _recordListings;
        [SerializeField, Tooltip("Prefab object for the record listing of each fishable type.")] private GameObject _recordListingPrefab;

        [SerializeField, Tooltip("Image UI that displays the selected fishable record's sprite.")] private Image _recordInfoImage;
        [SerializeField, Tooltip("Text UI that displays the selected fishable record's name.")] private Text _recordInfoName;
        [SerializeField, Tooltip("Text UI that displays the selected fishable record's total amount caught on this save file.")] private Text _recordCatchAmount;
        [SerializeField, Tooltip("Text UI that displays the selected fishable record's longest length.")] private Text _recordInfoLength;
        [SerializeField, Tooltip("Text UI that displays the selected fishable record's highest weight.")] private Text _recordInfoWeight;

        private PlayerData _playerData;
        private ItemLookupTable _itemLookupTable;
        private UIManager _UIManager;

        private static RecordsMenu _instance;
        public static RecordsMenu Instance { get => _instance; set => _instance = value; }

        public void ToggleRecordsMenu() {
            if (gameObject.activeSelf) {
                HideRecordsMenu();
            }
            else {
                ShowRecordsMenu();
            }
        }

        private void ShowRecordsMenu() {
            gameObject.SetActive(true);
            _UIManager.HideHUDButtons();
            UpdateRecords();
        }
        private void HideRecordsMenu() {
            gameObject.SetActive(false);
            _UIManager.ShowHUDButtons();
            _recordInfoPanel.SetActive(false);
        }

        private void UpdateRecords() {
            for (int i = 0; i < _itemLookupTable.FishableScriptables.Count; i++) {
                RecordsListing _newListing = Instantiate(_recordListingPrefab, _recordListings.content.transform).GetComponent<RecordsListing>();
                _newListing.UpdateListing(_playerData.StringToFishRecordData(_itemLookupTable.FishableScriptables[i].ItemName));
                _newListing.GetComponent<Button>().onClick.AddListener(delegate { ShowRecordInfoPanel(_newListing); });
            }
        }

        private void DestroyRecordsListings() {
            foreach (Transform child in _recordListings.content.transform) {
                Destroy(child.gameObject);
            }
        }

        public void ShowRecordInfoPanel(RecordsListing listing) {
            _recordInfoPanel.SetActive(true);

            _recordInfoImage.sprite = listing.ListingImage.sprite;

            if (listing.RecordData.AmountCaught == 0) {
                _recordInfoName.text = "???";
                _recordCatchAmount.text = "0";
                _recordInfoLength.text = "Catch this fish to track your record length.";
                _recordInfoWeight.text = "Catch this fish to track your record weight.";
                _recordInfoImage.color = Color.black;
            }
            else {
                _recordInfoName.text = listing.RecordData.ItemName;
                _recordCatchAmount.text = $"Caught: {listing.RecordData.AmountCaught}";
                _recordInfoLength.text = $"Length Record: \n{listing.RecordData.LengthRecord.ToString("F2")}cm";
                _recordInfoWeight.text = $"Weight Record: \n{listing.RecordData.WeightRecord.ToString("F2")}kg";
                _recordInfoImage.color = Color.white;
            }
        }

        public override void SetInstanceReference() {
            Instance = this;
        }

        public override void SetDepenencyReferences() {
            _playerData = SaveManager.Instance.LoadedPlayerData;
            _itemLookupTable = ItemLookupTable.Instance;
            _UIManager = UIManager.instance;
        }

        private void OnDisable() {
            DestroyRecordsListings();
        }
    }
}
