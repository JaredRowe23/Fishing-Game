using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Fishing.IO
{
    [System.Serializable]
    public class GameData
    {
        [Header("Save File Data")]
        public string playerName;
        public string currentSceneName;
        public float money;
        public string dateTime;
        public string playtime;

        [Header("Fish Inventory")]
        public List<string> bucketFish;
        public List<string> bucketFishDescription;
        public List<float> bucketFishWeight;
        public List<float> bucketFishLength;
        public List<float> bucketFishValue;

        [Header("Fishing Rods")]
        public List<string> fishingRods;
        public List<string> equippedLines;
        public List<string> equippedHooks;
        public List<string> equippedBaits;
        public string equippedRod;

        [Header("Gear")]
        public List<string> gear;
        public List<string> equippedGear;

        [Header("Bait")]
        public List<string> bait;
        public List<int> baitCounts;

        public GameData(PlayerData _playerData)
        {
            playerName = _playerData.playerName;
            currentSceneName = _playerData.currentSceneName;
            dateTime = _playerData.dateTime;
            playtime = _playerData.playtime;

            bucketFish = _playerData.bucketFish;
            bucketFishDescription = _playerData.bucketFishDescription;
            bucketFishWeight = _playerData.bucketFishWeight;
            bucketFishLength = _playerData.bucketFishLength;
            bucketFishValue = _playerData.bucketFishValue;

            money = _playerData.money;

            fishingRods = _playerData.fishingRods;
            equippedLines = _playerData.equippedLines;
            equippedHooks = _playerData.equippedHooks;
            equippedBaits = _playerData.equippedBaits;
            equippedRod = _playerData.equippedRod;

            gear = _playerData.gear;
            equippedGear = _playerData.equippedGear;

            bait = _playerData.bait;
            baitCounts = _playerData.baitCounts;
        }
    }

}