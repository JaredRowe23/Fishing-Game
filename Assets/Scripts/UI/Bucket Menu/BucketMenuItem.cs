using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fishing.Inventory;

namespace Fishing.UI
{
    public class BucketMenuItem : MonoBehaviour
    {
        [SerializeField] private Text itemName;
        [SerializeField] private Text itemWeight;
        [SerializeField] private Text itemLength;
        private FishData itemReference;

        public void UpdateName(string _name) => itemName.text = _name;

        public void UpdateWeight(float _weight) => itemWeight.text = _weight.ToString() + " kg";

        public void UpdateLength(float _length) => itemLength.text = _length.ToString() + " cm";

        public void UpdateReference(FishData _reference) => itemReference = _reference;

        public void OpenInfoMenu()
        {
            if (!GameController.instance.itemInfoMenu.activeSelf)
            {
                GameController.instance.itemInfoMenu.SetActive(true);
            }
            GameController.instance.itemInfoMenu.GetComponent<ItemInfoMenu>().UpdateMenu(itemName.text, itemWeight.text, itemLength.text, itemReference.itemDescription, itemReference, gameObject);
        }
    }

}