using UnityEngine;
using UnityEngine.UI;

public abstract class StoreItem : MonoBehaviour {
    [SerializeField, Tooltip("Text UI that displays the item's name.")] protected Text _nameText;
    [SerializeField, Tooltip("Image UI that displays the item's sprite.")] protected Image _itemImage;
    [SerializeField, Tooltip("Text UI that displays the item's cost.")] protected Text _costText;
    [SerializeField, Tooltip("Image UI that overlays over the item listing and changes color depending on the item's availability.")] protected Image _overlayImage;

    public enum ItemAvailablility { Available, Purchased, Restricted };

    private ItemAvailablility _availability;
    public ItemAvailablility Availability { get => _availability; set { _availability = value; } }

    [SerializeField, Tooltip("Color that will be overlayed on top of this item's UI when it is available.")] private Color _availableColor;
    public Color AvailableColor { get => _availableColor; private set { _availableColor = value; } }
    [SerializeField, Tooltip("Color that will be overlayed on top of this item's UI when it is restricted.")] private Color _restrictedColor;
    public Color RestrictedColor { get => _restrictedColor; private set { _restrictedColor = value; } }
    [SerializeField, Tooltip("Color that will be overlayed on top of this item's UI when it is already purchased.")] private Color _purchasedColor;
    public Color PurchasedColor { get => _purchasedColor; private set { _purchasedColor = value; } }

    public void UpdateColor() {
        switch (Availability) {
            case ItemAvailablility.Available:
                _overlayImage.color = AvailableColor;
                break;
            case ItemAvailablility.Restricted:
                _overlayImage.color = RestrictedColor;
                break;
            case ItemAvailablility.Purchased:
                _overlayImage.color = PurchasedColor;
                break;
        }
    }
}
