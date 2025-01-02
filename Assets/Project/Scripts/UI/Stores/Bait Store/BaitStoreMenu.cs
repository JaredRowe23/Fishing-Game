using Fishing.FishingMechanics;
using Fishing.IO;
using UnityEngine.UI;
using UnityEngine;

namespace Fishing.UI {
    public class BaitStoreMenu : StoreMenu {
        [SerializeField, Tooltip("Reference to the bait store info panel.")] private BaitStoreInfo _baitStoreInfo;

        private ItemLookupTable _itemLookupTable;

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            _itemLookupTable = ItemLookupTable.Instance;
            GenerateListings();
        }

        public override void GenerateListings() {
            foreach (BaitScriptable bait in _itemLookupTable.BaitScriptables) {
                BaitStoreListing listing = Instantiate(_itemListingPrefab, _itemListings.content.transform).GetComponent<BaitStoreListing>();
                listing.UpdateInfo(bait);

                listing.UpdateColor();

                listing.GetComponent<Button>().onClick.AddListener(delegate { _baitStoreInfo.UpdateInfo(listing.ReferenceScriptable); } );
            }
        }
    }
}
