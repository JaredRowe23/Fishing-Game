using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI {
    public abstract class StoreMenu : MonoBehaviour {
        [SerializeField, Tooltip("Prefab to spawn for each item listing.")] protected GameObject _itemListingPrefab;
        [SerializeField, Tooltip("ScrollRect UI that holds every item listing.")] protected ScrollRect _itemListings;

        protected static StoreMenu _instance;
        public static StoreMenu Instance { get => _instance; protected set { _instance = value; } }
        public abstract void GenerateListings();

        public void DestroyListings() {
            foreach (Transform child in _itemListings.content.transform) {
                Destroy(child.gameObject);
            }
        }

        public void RefreshStore() {
            DestroyListings();
            GenerateListings();
        }
    }
}