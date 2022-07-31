using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.FishingMechanics;
using Fishing.IO;

namespace Fishing.UI
{
    public class BaitStoreMenu : MonoBehaviour
    {
        [SerializeField] private GameObject baitListingPrefab;
        [SerializeField] private GameObject content;

        public static BaitStoreMenu instance;

        private BaitStoreMenu() => instance = this;

        public void DestroyListings()
        {
            foreach (Transform _child in content.transform)
            {
                if (_child.GetComponent<BaitStoreListing>())
                {
                    Destroy(_child.gameObject);
                }
            }
        }

        public void GenerateListings()
        {
            foreach (BaitScriptable _bait in ItemLookupTable.instance.baitScriptables)
            {
                BaitStoreListing _listing = Instantiate(baitListingPrefab, content.transform).GetComponent<BaitStoreListing>();
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
