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
        [SerializeField] private Transform contentParent;
        [SerializeField] private Selectable loadButton;
        [SerializeField] private Selectable deleteButton;
        public SaveSlotDetails slotDetails;
        public int selectedSlotIndex;

        private MenuNavigation navi;

        public static LoadMenu instance;

        private LoadMenu() => instance = this;

        private void Awake()
        {
            navi = GetComponent<MenuNavigation>();
        }

        public void LoadSaveSlot()
        {
            TitleMenuManager.instance.LoadGame(selectedSlotIndex);
        }

        public void GenerateSaveListings()
        {
            SaveManager.LoadSaveSlots();

            for (int i = 0; i < SaveManager.saveFiles.Count; i++)
            {
                SaveSlotListing _newSlot = Instantiate(saveFileListingPrefab, contentParent).GetComponent<SaveSlotListing>();
                _newSlot.SetInfo(SaveManager.saveFiles[i].name, SaveManager.saveFiles[i].dateTime, i);
                navi.menuOptions.Insert(navi.menuOptions.Count - 1, _newSlot.GetComponent<Selectable>());
                navi.menuCoordinates.Insert(navi.menuCoordinates.Count - 1, new Vector3(i, 0, i));
                navi.menuOptions.TrimExcess();
                navi.menuCoordinates.TrimExcess();
            }
            navi.menuCoordinates[navi.menuCoordinates.Count - 1] = new Vector3(navi.menuCoordinates.Count - 1, 0, SaveManager.saveFiles.Count);
        }

        public void EnableDetailButtons()
        {
            if (navi.menuOptions[navi.menuOptions.Count - 2] == deleteButton) return;
            navi.menuOptions.Insert(navi.menuOptions.Count - 1, loadButton);
            navi.menuOptions.Insert(navi.menuOptions.Count - 1, deleteButton);
            navi.menuCoordinates.Insert(navi.menuCoordinates.Count - 1, new Vector3(navi.menuCoordinates.Count - 1, 1, navi.menuCoordinates.Count - 1));
            navi.menuCoordinates.Insert(navi.menuCoordinates.Count - 1, new Vector3(navi.menuCoordinates.Count - 1, 2, navi.menuCoordinates.Count - 2));
            navi.menuCoordinates[navi.menuCoordinates.Count - 1] = new Vector3 (navi.menuCoordinates.Count - 1, navi.menuCoordinates[navi.menuCoordinates.Count - 1].y, navi.menuCoordinates[navi.menuCoordinates.Count - 1].z);
        }

        public void DestroySaveListings()
        {
            navi.menuOptions.RemoveRange(0, navi.menuOptions.Count - 3);
            navi.menuCoordinates.RemoveRange(0, navi.menuCoordinates.Count - 3);
            if (slotDetails.gameObject.activeSelf)
            {
                navi.menuOptions.RemoveRange(navi.menuOptions.Count - 3, navi.menuOptions.Count - 1);
                navi.menuCoordinates.RemoveRange(navi.menuCoordinates.Count - 3, navi.menuCoordinates.Count - 1);
            }
            navi._currentIndex = 0;
            navi._currentCoordinates = navi.menuCoordinates[0];
            foreach (Transform child in contentParent)
            {
                Destroy(child.gameObject);
            }
        }

        public void DeleteSaveSlot()
        {
            string _path = Application.persistentDataPath + "/" + SaveManager.saveFiles[selectedSlotIndex].name + ".fish";
            if (File.Exists(_path))
            {
                File.Delete(_path);
                TitleMenuManager.instance.HideLoadMenu();
            }
            else
            {
                Debug.LogError("Save file not found in " + Application.persistentDataPath + "/" + SaveManager.saveFiles[selectedSlotIndex].name + ".fish");
            }
            
        }
    }
}
