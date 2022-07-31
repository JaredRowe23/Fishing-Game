using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fishing.IO;
using Fishing.FishingMechanics;

namespace Fishing.UI
{
    public class RodsStoreMenu : MonoBehaviour
    {
        [SerializeField] private GameObject rodListingPrefab;
        [SerializeField] private GameObject content;

        public static RodsStoreMenu instance;

        private RodsStoreMenu() => instance = this;

        public void DestroyListings()
        {
            foreach (Transform _child in content.transform)
            {
                if (_child.GetComponent<RodStoreListing>())
                {
                    Destroy(_child.gameObject);
                }
            }
        }

        public void GenerateListings()
        {
            foreach (RodScriptable _rod in ItemLookupTable.instance.rodScriptables)
            {
                RodStoreListing _listing = Instantiate(rodListingPrefab, content.transform).GetComponent<RodStoreListing>();
                _listing.UpdateInfo(_rod);

                _listing.UpdateColor(RodStoreListing.ItemStatus.Available);

                foreach (string _rodName in PlayerData.instance.fishingRods)
                {
                    if (_rod.rodName == _rodName)
                    {
                        _listing.UpdateColor(RodStoreListing.ItemStatus.Purchased);
                        break;
                    }
                }

            }
        }

        public void RefreshStore()
        {
            DestroyListings();
            GenerateListings();
        }
    }
}
