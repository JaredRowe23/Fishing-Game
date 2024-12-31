using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI {
    public abstract class StoreInfoPanel : MonoBehaviour {
        [SerializeField, Tooltip("Text UI that displays the selected item's name.")] protected Text _nameText;
        [SerializeField, Tooltip("Image UI that displays the selected item's sprite.")] protected Image _itemImage;
        [SerializeField, Tooltip("Text UI that displays the selected item's description.")] protected Text _descriptionText;
        [SerializeField, Tooltip("Text UI that displays the selected item's cost.")] protected Text _costText;

        public abstract void PurchaseItem();
    }
}