using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.IO;
using UnityEngine.UI;

namespace Fishing.UI
{
    public class BaitMenu : MonoBehaviour, IInventoryTab
    {
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private ScrollRect listingsScrollRect;

        public void ShowBaitMenu() {
            InventoryMenu.instance.UpdateActiveMenu(gameObject);
        }

        public void ShowTab() {
            DestroySlots();
            GenerateSlots();
        }

        public void HideTab() {
            BaitInfoMenu.instance.gameObject.SetActive(false);
        }

        public void GenerateSlots()
        {
            for (int i = 0; i < SaveManager.Instance.LoadedPlayerData.BaitSaveData.Count; i++)
            {
                BaitInventorySlot _newSlot = Instantiate(slotPrefab, listingsScrollRect.content.transform).GetComponent<BaitInventorySlot>();
                _newSlot.baitSaveData = SaveManager.Instance.LoadedPlayerData.BaitSaveData[i];
                _newSlot.UpdateSlot();
            }
        }

        private void DestroySlots() {
            foreach (Transform _child in listingsScrollRect.content.transform) {
                Destroy(_child.gameObject);
            }
        }
    }

}