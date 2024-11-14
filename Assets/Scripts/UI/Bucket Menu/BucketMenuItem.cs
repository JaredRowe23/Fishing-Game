using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fishing.Inventory;
using Fishing.IO;

namespace Fishing.UI
{
    public class BucketMenuItem : MonoBehaviour
    {
        [SerializeField] private Text itemName;
        [SerializeField] private Text itemWeight;
        [SerializeField] private Text itemLength;
        [SerializeField] private Text itemValue;
        [HideInInspector] public BucketItemSaveData itemReference;

        public void UpdateInfo(BucketItemSaveData _item)
        {
            itemReference = _item;
            itemName.text = _item.itemName;
            itemWeight.text = _item.weight.ToString("F2") + " kg";
            itemLength.text = _item.length.ToString("F2") + " cm";
            itemValue.text = "$" + _item.value.ToString("F2");
        }

        public void OpenInfoMenu()
        {
            if (!UIManager.instance.itemInfoMenu.activeSelf)
            {
                UIManager.instance.itemInfoMenu.SetActive(true);
            }
            UIManager.instance.itemInfoMenu.GetComponent<ItemInfoMenu>().UpdateMenu(itemReference, gameObject);
        }
    }

}