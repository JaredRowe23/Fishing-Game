using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Fishing.IO
{
    [System.Serializable]
    public class GameData
    {
        public string playerName;
        public string currentSceneName;

        public int money;

        public List<string> bucketFish;
        public List<string> bucketFishDescription;
        public List<float> bucketFishWeight;
        public List<float> bucketFishLength;
        public List<float> bucketFishValue;

        public List<string> fishingRods;
        public string equippedRod;

        public List<string> gear;
        public List<string> equippedGear;

        public List<string> bait;
        public List<int> baitCounts;

        public GameData(PlayerData _playerData)
        {
            playerName = _playerData.playerName;
            currentSceneName = _playerData.currentSceneName;

            bucketFish = _playerData.bucketFish;
            bucketFishDescription = _playerData.bucketFishDescription;
            bucketFishWeight = _playerData.bucketFishWeight;
            bucketFishLength = _playerData.bucketFishLength;
            bucketFishValue = _playerData.bucketFishValue;

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