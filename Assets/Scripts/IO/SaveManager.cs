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

        public SaveFile(string _name, float _money, string _dateTime, string _playtime)
        {
            name = _name;
            money = _money;
            dateTime = _dateTime;
            playtime = _playtime;
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

                SaveFile _saveFile = new SaveFile(_data.playerName, _data.money, _data.dateTime, _data.playtime);
                saveFiles.Add(_saveFile);
            }

            saveFiles = SortSaveFiles(saveFiles);
        }

        private static List<SaveFile> SortSaveFiles(List<SaveFile> _unorganizedFiles)
        {
            List<SaveFile> _sortedFiles = new List<SaveFile>();

            foreach (SaveFile _saveFile in _unorganizedFiles)
            {
                Debug.Log("---------------------------------------");
                Debug.Log("checking unorganized file: " + _saveFile.name);
                if (_sortedFiles.Count == 0)
                {
                    Debug.Log("adding first file to sorted list");
                    _sortedFiles.Add(_saveFile);
                    continue;
                }

                System.DateTime _saveFileDateTime = System.DateTime.Parse(_saveFile.dateTime);
                int i = 0;

                foreach (SaveFile _sortedFile in _sortedFiles)
                {
                    System.DateTime _sortedFileDateTime = System.DateTime.Parse(_sortedFile.dateTime);
                    int _dateTimeComparison = System.DateTime.Compare(_saveFileDateTime, _sortedFileDateTime);

                    Debug.Log("comparing with sorted file");
                    if (_dateTimeComparison > 0)
                    {
                        Debug.Log("earlier than sorted file, adding in place");
                        _sortedFiles.Insert(i, _saveFile);
                        break;
                    }

                    else if (_dateTimeComparison == 0)
                    {
                        Debug.Log("same time as sorted file, adding in place");
                        _sortedFiles.Add(_saveFile);
                        continue;
                    }

                    else if (_dateTimeComparison < 0)
                    {
                        if (i == _sortedFiles.Count - 1)
                        {
                            Debug.Log("older than sorted file, which is the final file, adding to end");
                            _sortedFiles.Add(_saveFile);
                            break;
                        }
                        else
                        {
                            Debug.Log("older than sorted file, continuing to next file");
                            i++;
                            continue;
                        }
                    }
                }
            }

            Debug.Log("list completed:");
            foreach(SaveFile _saveFile in _sortedFiles)
            {
                Debug.Log(_saveFile.name);
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
