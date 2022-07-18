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
        public int money;

        public SaveFile(string _name, int _money)
        {
            name = _name;
            money = _money;
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

                SaveFile _saveFile = new SaveFile(_data.playerName, _data.money);
                saveFiles.Add(_saveFile);
            }
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
