using Fishing.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI {
    public class RecordsListing : MonoBehaviour {
        private RecordSaveData _recordData;
        public RecordSaveData RecordData { get => _recordData; private set { _recordData = value; } }

        [SerializeField] private Image _listingImage;
        public Image ListingImage { get => _listingImage; private set { } }

        public void UpdateListing(RecordSaveData data) {
            Debug.Assert(data != null, "Record data reference set to null when generating RecordListing", this);

            _recordData = data;
            _listingImage.sprite = ItemLookupTable.Instance.StringToFishScriptable(data.ItemName).InventorySprite;
            _listingImage.color = data.AmountCaught == 0 ? Color.black : Color.white;
        }
    }
}
