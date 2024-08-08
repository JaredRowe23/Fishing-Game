using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.IO
{
    [System.Serializable]
    public class FishingRodSaveData
    {
        public string rodName;
        public string equippedLine;
        public string equippedHook;
        public BaitSaveData equippedBait;

        public FishingRodSaveData(string _rodName, string _equippedLine, string _equippedHook, BaitSaveData _equippedBait)
        {
            rodName = _rodName;
            equippedLine = _equippedLine;
            equippedHook = _equippedHook;
            equippedBait = _equippedBait;
        }
    }
}
