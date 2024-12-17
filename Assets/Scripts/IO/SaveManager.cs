using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Fishing.IO {
    public class SaveManager : MonoBehaviour {
        [SerializeField] private PlayerData _loadedPlayerData;
        public PlayerData LoadedPlayerData { get => _loadedPlayerData; set => _loadedPlayerData = value; }

        private List<SaveFile> _saveFiles;
        public List<SaveFile> SaveFiles { get => _saveFiles; private set => _saveFiles = value; }

        private static SaveManager _instance;
        public static SaveManager Instance { get => _instance; private set => _instance = value; }

        private void Awake() {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void LoadSaveSlots() {
            SaveFiles = new List<SaveFile>();
            string path = Application.persistentDataPath;

            if (!Directory.Exists(path)) {
                return;
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            FileInfo[] fileInfo = directoryInfo.GetFiles();

            for (int i = 0; i < fileInfo.Length; i++) {
                if (fileInfo[i].Extension != ".fish") {
                    continue;
                }

                PlayerData data = new PlayerData();
                string jsonData = File.ReadAllText(fileInfo[i].FullName);
                JsonUtility.FromJsonOverwrite(jsonData, data);

                SaveFile saveFile = new SaveFile(data.SaveFileData.PlayerName, data.SaveFileData.Money, data.SaveFileData.DateTime, data.SaveFileData.Playtime, data.RecordSaveData.Count);
                SaveFiles.Add(saveFile);
            }

            SaveFiles = SortSaveFiles(SaveFiles);
        }

        private List<SaveFile> SortSaveFiles(List<SaveFile> unorganizedFiles) {
            List<SaveFile> sortedFiles = new List<SaveFile>();

            for (int i = 0; i < unorganizedFiles.Count; i++) {
                if (sortedFiles.Count == 0) {
                    sortedFiles.Add(unorganizedFiles[i]);
                    continue;
                }

                System.DateTime saveFileDateTime = System.DateTime.Parse(unorganizedFiles[i].DateTime);

                for (int j = 0; j < sortedFiles.Count; j++) {
                    System.DateTime sortedFileDateTime = System.DateTime.Parse(sortedFiles[j].DateTime);
                    int dateTimeComparison = System.DateTime.Compare(saveFileDateTime, sortedFileDateTime);

                    if (dateTimeComparison > 0) {
                        sortedFiles.Insert(j, unorganizedFiles[i]);
                        break;
                    }

                    else if (dateTimeComparison == 0) {
                        sortedFiles.Add(unorganizedFiles[i]);
                        continue;
                    }

                    else if (dateTimeComparison < 0) {
                        if (j != sortedFiles.Count - 1) {
                            continue;
                        }

                        sortedFiles.Add(unorganizedFiles[i]);
                        break;
                    }
                }
            }

            return sortedFiles;
        }

        public void SaveGame(string fileName) {
            string path = $"{Application.persistentDataPath}";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            string file = $"{path}/{fileName}.fish";
            string json = JsonUtility.ToJson(LoadedPlayerData, false);
            File.WriteAllText(file, json);
        }

        public void LoadGame(string path) {
            if (File.Exists(path)) {
                string jsonData = File.ReadAllText(path);
                JsonUtility.FromJsonOverwrite(jsonData, LoadedPlayerData);
            }
            else {
                Debug.LogError($"Save file not found in {path}.");
            }
        }
    }
}
