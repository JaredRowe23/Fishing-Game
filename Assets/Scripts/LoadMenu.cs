using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Fishing.UI
{
    public class LoadMenu : MonoBehaviour
    {
        [SerializeField] private GameObject saveFileListingPrefab;
        [SerializeField] private Transform contentParent;
        public int selectedSlotIndex;

        public static LoadMenu instance;

        private LoadMenu() => instance = this;

        public void LoadSaveSlot()
        {
            SaveManager.LoadGame(Application.persistentDataPath+ "/" + SaveManager.saveFiles[selectedSlotIndex].name + ".fish");
            SceneManager.LoadScene(PlayerData.instance.currentSceneName);
        }

        public void GenerateSaveListings()
        {
            SaveManager.LoadSaveSlots();

            for (int i = 0; i < SaveManager.saveFiles.Count; i++)
            {
                SaveSlotListing _newSlot = Instantiate(saveFileListingPrefab, contentParent).GetComponent<SaveSlotListing>();
                _newSlot.SetInfo(SaveManager.saveFiles[i].name, SaveManager.saveFiles[i].dateTime, i);
            }
        }

        public void DestroySaveListings()
        {
            foreach(Transform child in contentParent)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
