using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fishing.Inventory;

namespace Fishing.UI
{
    public class ItemInfoMenu : MonoBehaviour
    {
        [Header("Object Stats")]
        [SerializeField] private Image itemImage;
        [SerializeField] private Text itemName;
        [SerializeField] private Text itemWeight;
        [SerializeField] private Text itemLength;
        [SerializeField] private Text itemValue;
        [SerializeField] private Text itemDescription;
        [SerializeField] private List<GameObject> models;
        [SerializeField] private List<string> modelNames;
        private GameObject currentModel;

        private FishData itemReference;
        private GameObject menuListingReference;

        public void UpdateMenu(string _name, string _value, string _weight, string _length, string _description, FishData _reference, GameObject _menuListing)
        {
            itemName.text = _name;
            itemValue.text = _value;
            itemWeight.text = _weight;
            itemLength.text = _length;
            itemDescription.text = _description;
            menuListingReference = _menuListing;
            itemReference = _reference;
            GenerateModel(_name);
            ItemViewerCamera.instance.UpdateCurrentItem(currentModel);
        }

        public void ThrowAway(bool _isSelling)
        {
            if (UIManager.instance.overflowItem != null)
            {
                if (UIManager.instance.overflowItem.activeSelf)
                {
                    OverflowItem.instance.ThrowAway(itemReference, currentModel, menuListingReference);
                    gameObject.SetActive(false);
                    return;
                }
            }

            BucketMenu.instance.ThrowAway(itemReference, currentModel, menuListingReference, _isSelling);
            gameObject.SetActive(false);
        }

        public string GetItemName()
        {
            return itemName.text;
        }

        public GameObject GenerateModel(string _itemName)
        {
            if (currentModel != null) Destroy(currentModel);

            for (int i = 0; i < models.Count; i++)
            {
                if (modelNames[i] == _itemName)
                {
                    currentModel = Instantiate(models[i]);
                }
            }

            currentModel.transform.parent = ItemViewerCamera.instance.transform.parent;
            currentModel.transform.position = ItemViewerCamera.instance.transform.parent.position;
            currentModel.transform.rotation = Quaternion.identity;

            return currentModel;

        }
    }

}