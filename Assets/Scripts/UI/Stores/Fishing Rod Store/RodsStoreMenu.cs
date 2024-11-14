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
        [SerializeField] private ScrollRect rodListings;

        public static RodsStoreMenu instance;

        private RodsStoreMenu() => instance = this;

        public void DestroyListings()
        {
            RodStoreListing[] _listings = rodListings.content.transform.GetComponentsInChildren<RodStoreListing>();
            for (int i = 0; i < _listings.Length; i++) {
                Destroy(_listings[i].gameObject);
            }
        }

        public void GenerateListings()
        {
            foreach (RodScriptable _rod in ItemLookupTable.instance.rodScriptables)
            {
                RodStoreListing _listing = Instantiate(rodListingPrefab, rodListings.content.transform).GetComponent<RodStoreListing>();
                _listing.UpdateInfo(_rod);

                _listing.UpdateColor(RodStoreListing.ItemStatus.Available);

                for (int i = 0; i < PlayerData.instance.fishingRodSaveData.Count; i++)
                {
                    if (_rod.rodName == PlayerData.instance.fishingRodSaveData[i].rodName)
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
