using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

namespace Fishing.UI
{
    public class LoadMenu : MonoBehaviour
    {
        [SerializeField] private GameObject saveFileListingPrefab;
        [SerializeField] private ScrollRect saveFileListings;
        [SerializeField] private Selectable loadButton;
        [SerializeField] private Selectable deleteButton;
        public SaveSlotDetails slotDetails;
        public int selectedSlotIndex;

        public static LoadMenu instance;

        private LoadMenu() => instance = this;

        public void LoadSaveSlot() {
            TitleMenuManager.instance.LoadGame(selectedSlotIndex);
        }

        private void RefreshSaveSlotListings() {
            DestroySaveListings();
            GenerateSaveListings();
        }

        public void GenerateSaveListings() {
            SaveManager.Instance.LoadSaveSlots();

            for (int i = 0; i < SaveManager.Instance.SaveFiles.Count; i++) {
                SaveSlotListing _newSlot = Instantiate(saveFileListingPrefab, saveFileListings.content).GetComponent<SaveSlotListing>();
                _newSlot.SetInfo(SaveManager.Instance.SaveFiles[i].Name, SaveManager.Instance.SaveFiles[i].DateTime, i);
            }
        }

        public void DestroySaveListings() {
            SaveSlotListing[] _saveSlots = saveFileListings.content.transform.GetComponentsInChildren<SaveSlotListing>();
            for (int i = 0; i < _saveSlots.Length; i++) {
                Destroy(_saveSlots[i].gameObject);
            }
        }

        public void DeleteSaveSlot() {
            string _path = $"{Application.persistentDataPath}/{SaveManager.Instance.SaveFiles[selectedSlotIndex].Name}.fish";
            if (File.Exists(_path)) {
                File.Delete(_path);
                RefreshSaveSlotListings();
            }
            else {
                Debug.LogError($"Save file not found in {Application.persistentDataPath}/{SaveManager.Instance.SaveFiles[selectedSlotIndex].Name}.fish");
            }
            
        }
    }
}
