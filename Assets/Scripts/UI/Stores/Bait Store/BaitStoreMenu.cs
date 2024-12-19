using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.FishingMechanics;
using Fishing.IO;
using UnityEngine.UI;

namespace Fishing.UI
{
    public class BaitStoreMenu : MonoBehaviour
    {
        [SerializeField] private GameObject baitListingPrefab;
        [SerializeField] private ScrollRect baitListings;

        public static BaitStoreMenu instance;

        private BaitStoreMenu() => instance = this;

        public void DestroyListings() {
            BaitStoreListing[] _listings = baitListings.content.transform.GetComponentsInChildren<BaitStoreListing>();
            for (int i = 0; i < _listings.Length; i++) {
                Destroy(_listings[i].gameObject);
            }
        }

        public void GenerateListings() {
            foreach (BaitScriptable _bait in ItemLookupTable.Instance.BaitScriptables) {
                BaitStoreListing _listing = Instantiate(baitListingPrefab, baitListings.content.transform).GetComponent<BaitStoreListing>();
                _listing.UpdateInfo(_bait);

                _listing.UpdateColor(BaitStoreListing.ItemStatus.Available);
            }
        }

        public void RefreshStore()
        {
            DestroyListings();
            GenerateListings();
        }
    }
}
