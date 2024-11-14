using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.IO
{
    [System.Serializable]
    public class SaveFileData
    {
        public string playerName;
        public string currentSceneName;
        public float money;
        public string dateTime;
        public string playtime;

        public SaveFileData(string _playerName, string _currentSceneName, float _money, string _dateTime, string _playtime)
        {
            playerName = _playerName;
            currentSceneName = _currentSceneName;
            money = _money;
            dateTime = _dateTime;
            playtime = _playtime;
        }
    }
}
