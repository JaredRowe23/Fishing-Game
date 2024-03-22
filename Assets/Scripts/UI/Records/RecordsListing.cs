using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI
{
    public class RecordsListing : MonoBehaviour
    {
        [SerializeField] private string fishableName;
        public string showName;
        public int catchAmount;
        public float length;
        public float weight;

        [SerializeField] private Image listingImage;

        public void UpdateListing(string _name, int _catchAmount, float _length, float _weight)
        {
            showName = _name;
            catchAmount = _catchAmount;
            length = _length;
            weight = _weight;

            if (_name == "") listingImage.color = Color.black;
            else listingImage.color = Color.white;
        }

        public string GetFishableName() => fishableName;

        public void ShowListingInfo()
        {
            RecordsMenu.instance.ShowRecordInfoPanel(showName, catchAmount, length, weight, listingImage.sprite);
        }
    }
}
