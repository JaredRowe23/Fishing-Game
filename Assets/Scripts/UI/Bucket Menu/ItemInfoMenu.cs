using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing
{
    public class ItemInfoMenu : MonoBehaviour
    {
        [Header("Object Stats")]
        [SerializeField] private Image itemImage;
        [SerializeField] private Text itemName;
        [SerializeField] private Text itemWeight;
        [SerializeField] private Text itemLength;
        [SerializeField] private Text itemDescription;
        [SerializeField] private List<GameObject> models;
        [SerializeField] private List<string> modelNames;
        private GameObject currentModel;

        //private GameObject itemReference;
        private FishData itemReference;
        private GameObject menuListingReference;

        public void UpdateMenu(string name, string weight, string length, string description, FishData reference, GameObject menuListing)
        {
            itemName.text = name;
            itemWeight.text = weight;
            itemLength.text = length;
            itemDescription.text = description;
            menuListingReference = menuListing;
            itemReference = reference;
            GenerateModel(name);
            ItemViewerCamera.instance.UpdateCurrentItem(currentModel);
        }

        public void ThrowAway()
        {
            BucketMenu.instance.ThrowAway(itemReference, currentModel, menuListingReference);
            gameObject.SetActive(false);
        }

        public string GetItemName()
        {
            return itemName.text;
        }

        public GameObject GenerateModel(string itemName)
        {
            if (currentModel != null)
            {
                Destroy(currentModel);
            }

            for (int i = 0; i < models.Count; i++)
            {
                if (modelNames[i] == itemName)
                {
                    currentModel = Instantiate(models[i]);
                }
            }

            currentModel.transform.parent = BucketBehaviour.instance.transform;
            currentModel.transform.position = BucketBehaviour.instance.transform.position;
            currentModel.transform.rotation = Quaternion.identity;

            return currentModel;

        }
    }

}