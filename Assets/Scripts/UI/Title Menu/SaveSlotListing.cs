using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fishing.IO;

namespace Fishing.UI
{
    public class SaveSlotListing : MonoBehaviour
    {
        [SerializeField] private Text saveNameText;
        [SerializeField] private Text saveTimeDateText;
        private int saveIndex;

        public void SetInfo(string _saveName, string _saveDate, int _saveIndex) {
            saveNameText.text = _saveName;
            saveTimeDateText.text = _saveDate;
            saveIndex = _saveIndex;
        }

        public void ShowData() {
            LoadMenu.instance.slotDetails.gameObject.SetActive(true);
            LoadMenu.instance.slotDetails.UpdateInfo(SaveManager.saveFiles[saveIndex]);
            LoadMenu.instance.selectedSlotIndex = saveIndex;
        }
    }
}
