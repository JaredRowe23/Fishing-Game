using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Fishing.IO
{
    public static class SaveSystem
    {
        public static void SaveGame(PlayerData _player)
        {
            BinaryFormatter _formatter = new BinaryFormatter();

            string _path = Application.persistentDataPath + "/player.fish";
            FileStream _stream = new FileStream(_path, FileMode.Create);

            GameData _data = new GameData(_player);

            _formatter.Serialize(_stream, _data);
            _stream.Close();
        }

        public static GameData LoadGame()
        {
            string _path = Application.persistentDataPath + "/player.fish";
            if (File.Exists(_path))
            {
                BinaryFormatter _formatter = new BinaryFormatter();
                FileStream _stream = new FileStream(_path, FileMode.Open);

                GameData _data = _formatter.Deserialize(_stream) as GameData;
                _stream.Close();

                return _data;
            }
            else
            {
                Debug.LogError("Save file not found in " + _path);
                return null;
            }
        }
    }
}