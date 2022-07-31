using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.IO;

namespace Fishing.UI
{
    public class BaitMenu : MonoBehaviour
    {
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private GameObject content;

        public void ShowBaitMenu(bool _active)
        {
            this.gameObject.SetActive(_active);
            InventoryMenu.instance.UpdateActiveMenu(1);
            GenerateSlots();

            if (!_active) BaitInfoMenu.instance.gameObject.SetActive(false);
        }

        public void GenerateSlots()
        {
            foreach (Transform _child in content.transform)
            {
                Destroy(_child.gameObject);
            }

            int i = 0;
            if (PlayerData.instance.bait.Count == 0) return; 
            foreach (string _bait in PlayerData.instance.bait)
            {
                BaitInventorySlot _newSlot = Instantiate(slotPrefab, content.transform).GetComponent<BaitInventorySlot>();
                _newSlot.baitScriptable = ItemLookupTable.instance.StringToBait(_bait);
                _newSlot.UpdateSlot();
                _newSlot.countText.text = "x" + PlayerData.instance.baitCounts[i].ToString();

                i++;
            }
        }
    }

}