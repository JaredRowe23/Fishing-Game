using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.IO
{
    [System.Serializable]
    public class BaitSaveData
    {
        public string baitName;
        public int amount;

        public BaitSaveData(string _baitName, int _amount)
        {
            baitName = _baitName;
            amount = _amount;
        }
    }
}
