using Fishing.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI
{
    public class RecordsListing : MonoBehaviour
    {
        [SerializeField] private string fishableName;
        public string FishableName { get { return fishableName; } private set { } }
        public string showName;
        public int catchAmount;
        public float length;
        public float weight;

        private RecordSaveData recordData;

        [SerializeField] private Image listingImage;

        public void ResetListing() {
            recordData = null;
            showName = "";
            catchAmount = 0;
            length = 0;
            weight = 0;
            listingImage.color = Color.black;
        }

        public void UpdateListing(RecordSaveData _recordData) {
            if (_recordData == null) {
                ResetListing();
                return;
            }

            recordData = _recordData;
            showName = recordData.ItemName;
            catchAmount = recordData.AmountCaught;
            length = recordData.LengthRecord;
            weight = recordData.WeightRecord;
            listingImage.color = Color.white;
        }

        public void ShowListingInfo() {
            RecordsMenu.instance.ShowRecordInfoPanel(recordData, listingImage.sprite);
        }
    }
}
