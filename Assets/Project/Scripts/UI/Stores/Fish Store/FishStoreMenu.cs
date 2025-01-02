using Fishing.IO;
using UnityEngine.UI;
using UnityEngine;

namespace Fishing.UI {
    public class FishStoreMenu : StoreMenu {
        [SerializeField, Tooltip("Reference to fish store info panel")] private FishStoreInfo _fishStoreInfo;

        private PlayerData _playerData;

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            _playerData = SaveManager.Instance.LoadedPlayerData;

            GenerateListings();
        }

        public override void GenerateListings() {
            foreach (BucketItemSaveData data in _playerData.BucketItemSaveData) {
                FishStoreListing _newListing = Instantiate(_itemListingPrefab, _itemListings.content.transform).GetComponent<FishStoreListing>();
                _newListing.UpdateInfo(data);
                _newListing.Availability = StoreItem.ItemAvailablility.Available;
                _newListing.UpdateColor();

                _newListing.GetComponent<Button>().onClick.AddListener(delegate { _fishStoreInfo.UpdateInfo(data); });
            }
        }
    }
}