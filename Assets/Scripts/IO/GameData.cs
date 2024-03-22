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

        [Header("Tutorials")]
        public bool hasSeenCastTut;
        public bool hasSeenReelingTut;
        public bool hasSeenReelingMinigameTut;
        public bool hasSeenBucketTut;
        public bool hasSeenBucketMenuTut;
        public bool hasSeenBaitTut;
        public bool hasSeenInventoryTut;
        public bool hasSeenFishTut;
        public bool hasSeenNPCTut;

        [Header("Records")]
        public List<string> caughtFish;
        public List<int> fishCatchAmounts;
        public List<float> lengthRecords;
        public List<float> weightRecords;

        public GameData(PlayerData _playerData)
        {
            SetFileData(_playerData);
            SetFishInventoryData(_playerData);
            SetFishingRodsData(_playerData);
            SetGearData(_playerData);
            SetBaitData(_playerData);
            SetTutorialsData(_playerData);
            SetFishableRecordsData(_playerData);
        }

        private void SetFileData(PlayerData _playerData)
        {
            playerName = _playerData.playerName;
            currentSceneName = _playerData.currentSceneName;
            money = _playerData.money;
            dateTime = _playerData.dateTime;
            playtime = _playerData.playtime;
        }
        private void SetFishInventoryData(PlayerData _playerData)
        {
            bucketFish = _playerData.bucketFish;
            bucketFishDescription = _playerData.bucketFishDescription;
            bucketFishWeight = _playerData.bucketFishWeight;
            bucketFishLength = _playerData.bucketFishLength;
            bucketFishValue = _playerData.bucketFishValue;
        }
        private void SetFishingRodsData(PlayerData _playerData)
        {
            fishingRods = _playerData.fishingRods;
            equippedLines = _playerData.equippedLines;
            equippedHooks = _playerData.equippedHooks;
            equippedBaits = _playerData.equippedBaits;
            equippedRod = _playerData.equippedRod;
        }
        private void SetGearData(PlayerData _playerData)
        {
            gear = _playerData.gear;
            equippedGear = _playerData.equippedGear;
        }
        private void SetBaitData(PlayerData _playerData)
        {
            bait = _playerData.bait;
            baitCounts = _playerData.baitCounts;
        }
        private void SetTutorialsData(PlayerData _playerData)
        {
            hasSeenCastTut = _playerData.hasSeenCastTut;
            hasSeenBucketTut = _playerData.hasSeenBucketTut;
            hasSeenBucketMenuTut = _playerData.hasSeenBucketMenuTut;
            hasSeenBaitTut = _playerData.hasSeenBaitTut;
            hasSeenReelingTut = _playerData.hasSeenReelingTut;
            hasSeenReelingMinigameTut = _playerData.hasSeenReelingMinigameTut;
            hasSeenInventoryTut = _playerData.hasSeenInventoryTut;
            hasSeenFishTut = _playerData.hasSeenFishTut;
            hasSeenNPCTut = _playerData.hasSeenNPCTut;
        }
        private void SetFishableRecordsData(PlayerData _playerData)
        {
            caughtFish = _playerData.caughtFish;
            fishCatchAmounts = _playerData.fishCatchAmounts;
            lengthRecords = _playerData.lengthRecords;
            weightRecords = _playerData.weightRecords;
        }
    }

}