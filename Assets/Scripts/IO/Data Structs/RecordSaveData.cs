using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.IO
{
    [System.Serializable]
    public class RecordSaveData
    { 
        public string itemName;
        public int amountCaught;
        public float lengthRecord;
        public float weightRecord;

        public RecordSaveData(string _itemName, int _amountCaught, float _lengthRecord, float _weightRecord)
        {
            itemName = _itemName;
            amountCaught = _amountCaught;
            lengthRecord = _lengthRecord;
            weightRecord = _weightRecord;
        }
    }
}
