using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.IO;
using UnityEngine.UI;

namespace Fishing.UI
{
    public class ContinueMenu : MonoBehaviour
    {
        public SaveSlotDetails slotDetails;
        [SerializeField] private Button loadButton;
        [SerializeField] private GameObject noSavesText;

        public static ContinueMenu instance;

        private ContinueMenu() => instance = this;

        public void LoadContinueSlotDetails()
        {
            SaveManager.LoadSaveSlots();
            if (SaveManager.saveFiles.Count == 0)
            {
                slotDetails.gameObject.SetActive(false);
                noSavesText.SetActive(true);
                loadButton.interactable = false;
            }
            else
            {
                slotDetails.gameObject.SetActive(true);
                noSavesText.SetActive(false);
                loadButton.interactable = true;
                slotDetails.UpdateInfo(SaveManager.saveFiles[0].name, SaveManager.saveFiles[0].money, SaveManager.saveFiles[0].dateTime, SaveManager.saveFiles[0].playtime);
            }
        }
    }
}