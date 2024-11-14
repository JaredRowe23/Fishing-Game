using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.IO {
    [System.Serializable]
    public class SaveFile {
        public string name;
        public float money;
        public string dateTime;
        public string playtime;
        public int fishTypesCaught;

        public SaveFile(string _name, float _money, string _dateTime, string _playtime, int _fishTypesCaught) {
            name = _name;
            money = _money;
            dateTime = _dateTime;
            playtime = _playtime;
            fishTypesCaught = _fishTypesCaught;
        }
    }
}
