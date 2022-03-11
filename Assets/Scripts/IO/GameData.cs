using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Fishing.IO
{
    [System.Serializable]
    public class GameData
    {
        public string playerName;
        public int money;
        public List<string> fishingRods;
        public string equippedRod;
        public List<string> gear;
        public List<string> equippedGear;
        public List<string> bait;
        public List<int> baitCounts;

        public GameData(PlayerData _playerData)
        {
            playerName = _playerData.playerName;
            money = _playerData.money;

            fishingRods = _playerData.fishingRods;
            equippedRod = _playerData.equippedRod;

            gear = _playerData.gear;
            equippedGear = _playerData.equippedGear;

            bait = _playerData.bait;
            baitCounts = _playerData.baitCounts;
        }
    }

}