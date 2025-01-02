using Fishing.FishingMechanics;
using Fishing.IO;
using UnityEngine.UI;
using UnityEngine;

namespace Fishing.UI {
    public class RodStoreMenu : StoreMenu {
        [SerializeField, Tooltip("Reference to the info panel for this store.")] private RodStoreInfo _rodStoreInfo;

        private PlayerData _playerData;
        private ItemLookupTable _itemLookupTable;

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            _playerData = SaveManager.Instance.LoadedPlayerData;
            _itemLookupTable = ItemLookupTable.Instance;

            GenerateListings();
        }

        public override void GenerateListings() {
            foreach (RodScriptable rod in _itemLookupTable.RodScriptables) {
                RodStoreListing listing = Instantiate(_itemListingPrefab, _itemListings.content.transform).GetComponent<RodStoreListing>();
                listing.UpdateInfo(rod);

                listing.Availability = StoreItem.ItemAvailablility.Available;
                for (int i = 0; i < _playerData.FishingRodSaveData.Count; i++) {
                    if (rod.RodName == _playerData.FishingRodSaveData[i].RodName) {
                        listing.Availability = StoreItem.ItemAvailablility.Purchased;
                        break;
                    }
                }

                listing.UpdateColor();

                listing.GetComponent<Button>().onClick.AddListener(delegate { _rodStoreInfo.UpdateInfo(listing.ReferenceScriptable); });
            }
        }
    }
}
