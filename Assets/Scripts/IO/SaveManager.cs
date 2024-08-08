using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Fishing.IO
{
    [System.Serializable]
    public class SaveFile
    {
        public string name;
        public float money;
        public string dateTime;
        public string playtime;
        public int fishTypesCaught;

        public SaveFile(string _name, float _money, string _dateTime, string _playtime, int _fishTypesCaught)
        {
            name = _name;
            money = _money;
            dateTime = _dateTime;
            playtime = _playtime;
            fishTypesCaught = _fishTypesCaught;
        }
    }

    public class SaveManager : MonoBehaviour
    {
        public static List<SaveFile> saveFiles;

        public static SaveManager instance;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public static void LoadSaveSlots()
        {
            saveFiles = new List<SaveFile>();
            string _path = Application.persistentDataPath;

            if (!Directory.Exists(_path))
            {
                return;
            }

            DirectoryInfo _directoryInfo = new DirectoryInfo(_path);
            FileInfo[] _fileInfo = _directoryInfo.GetFiles();

            for (int i = 0; i < _fileInfo.Length; i++)
            {
                if (_fileInfo[i].Extension != ".fish")
                {
                    continue;
                }
                BinaryFormatter _formatter = new BinaryFormatter();
                FileStream _stream = File.Open(_fileInfo[i].FullName, FileMode.Open);

                GameData _data = _formatter.Deserialize(_stream) as GameData;
                _stream.Close();

                SaveFile _saveFile = new SaveFile(_data.saveFileData.playerName, _data.saveFileData.money, _data.saveFileData.dateTime, _data.saveFileData.playtime, 0);
                if (_data.recordSaveData != null) _saveFile.fishTypesCaught = _data.recordSaveData.Count;
                saveFiles.Add(_saveFile);
            }

            saveFiles = SortSaveFiles(saveFiles);
        }

        private static List<SaveFile> SortSaveFiles(List<SaveFile> _unorganizedFiles)
        {
            List<SaveFile> _sortedFiles = new List<SaveFile>();

            for (int i = 0; i < _unorganizedFiles.Count; i++)
            {
                if (_sortedFiles.Count == 0)
                {
                    _sortedFiles.Add(_unorganizedFiles[i]);
                    continue;
                }

                System.DateTime _saveFileDateTime = System.DateTime.Parse(_unorganizedFiles[i].dateTime);

                for (int j = 0; j < _sortedFiles.Count; j++)
                {
                    System.DateTime _sortedFileDateTime = System.DateTime.Parse(_sortedFiles[j].dateTime);
                    int _dateTimeComparison = System.DateTime.Compare(_saveFileDateTime, _sortedFileDateTime);

                    if (_dateTimeComparison > 0)
                    {
                        _sortedFiles.Insert(j, _unorganizedFiles[i]);
                        break;
                    }

                    else if (_dateTimeComparison == 0)
                    {
                        _sortedFiles.Add(_unorganizedFiles[i]);
                        continue;
                    }

                    else if (_dateTimeComparison < 0)
                    {
                        if (j != _sortedFiles.Count - 1) continue;

                        _sortedFiles.Add(_unorganizedFiles[i]);
                        break;
                    }
                }
            }

            return _sortedFiles;
        }

        public static void SaveGame(PlayerData _player, string _fileName)
        {
            BinaryFormatter _formatter = new BinaryFormatter();

            string _path = Application.persistentDataPath + "/" + _fileName + ".fish";
            FileStream _stream = new FileStream(_path, FileMode.Create);

            GameData _data = new GameData(_player);

            _formatter.Serialize(_stream, _data);
            _stream.Close();
        }

        public static void LoadGame(string _path)
        {
            if (File.Exists(_path))
            {
                BinaryFormatter _formatter = new BinaryFormatter();
                FileStream _stream = new FileStream(_path, FileMode.Open);

                GameData _data = _formatter.Deserialize(_stream) as GameData;
                _stream.Close();

                PlayerData.instance.LoadPlayer(_data);
            }
            else
            {
                Debug.LogError("Save file not found in " + _path);
            }
        }
    }
}
