using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.UI;

namespace Fishing.IO
{
    public class PlayerData : MonoBehaviour
    {
        [Header("Save File Data")]
        public string playerName;
        public float money;
        private System.DateTime sessionStartTime;
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

        public string currentSceneName;

        public static PlayerData instance;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void SavePlayer()
        {
            TooltipSystem.instance.NewTooltip(5f, "Game Saved!");

            System.DateTime previousDateTime = System.DateTime.Parse(dateTime);
            System.TimeSpan currentSessionTime = System.DateTime.Now.Subtract(sessionStartTime);
            System.TimeSpan previousPlaytime = System.TimeSpan.Parse(playtime);
            System.TimeSpan addPlaytime = previousPlaytime.Add(currentSessionTime);
            playtime = string.Format("{0}:{1}:{2}", addPlaytime.Hours, addPlaytime.Minutes, addPlaytime.Seconds);
            dateTime = System.DateTime.Now.ToString("G");

            SaveManager.SaveGame(this, playerName);
        }

        public void LoadPlayer(GameData _saveData)
        {
            SetFileData(_saveData);
            sessionStartTime = System.DateTime.Now;
            SetFishInventoryData(_saveData);
            SetFishingRodsData(_saveData);
            SetGearData(_saveData);
            SetBaitData(_saveData);
            SetTutorialsData(_saveData);
            SetFishableRecordsData(_saveData);
            SetFishableRecordsData(_saveData);
        }

        public void NewGame()
        {
            playerName = "";
            currentSceneName = "World Map";
            money = 0;
            dateTime = System.DateTime.Now.ToString("G");
            sessionStartTime = System.DateTime.Now;
            playtime = System.TimeSpan.Zero.ToString();

            bucketFish = new List<string>();
            bucketFishDescription = new List<string>();
            bucketFishWeight = new List<float>();
            bucketFishLength = new List<float>();
            bucketFishValue = new List<float>();

            fishingRods = new List<string>();
            equippedBaits = new List<string>();
            equippedHooks = new List<string>();
            equippedLines = new List<string>();
            AddRod("Wooden Fishing Rod");
            equippedRod = fishingRods[0];

            gear = new List<string>();
            equippedGear = new List<string>();

            bait = new List<string>();
            baitCounts = new List<int>();

            hasSeenCastTut = hasSeenReelingTut = hasSeenReelingMinigameTut = hasSeenBucketTut = hasSeenBucketMenuTut = hasSeenBaitTut = hasSeenInventoryTut = hasSeenFishTut = hasSeenNPCTut = false;

            caughtFish = new List<string>();
            fishCatchAmounts = new List<int>();
            lengthRecords = new List<float>();
            weightRecords = new List<float>();
        }

        public void AddRod(string _rod)
        {
            fishingRods.Add(_rod);
            equippedBaits.Add("");
            equippedLines.Add("");
            equippedHooks.Add("");
        }

        public void AddBait(string _baitName)
        {
            for (int j = 0; j < PlayerData.instance.bait.Count; j++)
            {
                if (_baitName != PlayerData.instance.bait[j]) continue;
                PlayerData.instance.baitCounts[j]++;
                return;
            }
            PlayerData.instance.baitCounts.Insert(PlayerData.instance.bait.Count, 1);
            PlayerData.instance.bait.Add(_baitName);
        }

        private void SetFileData(GameData _playerData)
        {
            playerName = _playerData.playerName;
            currentSceneName = _playerData.currentSceneName;
            money = _playerData.money;
            dateTime = _playerData.dateTime;
            playtime = _playerData.playtime;
        }
        private void SetFishInventoryData(GameData _playerData)
        {
            bucketFish = _playerData.bucketFish;
            bucketFishDescription = _playerData.bucketFishDescription;
            bucketFishWeight = _playerData.bucketFishWeight;
            bucketFishLength = _playerData.bucketFishLength;
            bucketFishValue = _playerData.bucketFishValue;
        }
        private void SetFishingRodsData(GameData _playerData)
        {
            fishingRods = _playerData.fishingRods;
            equippedLines = _playerData.equippedLines;
            equippedHooks = _playerData.equippedHooks;
            equippedBaits = _playerData.equippedBaits;
            equippedRod = _playerData.equippedRod;
        }
        private void SetGearData(GameData _playerData)
        {
            gear = _playerData.gear;
            equippedGear = _playerData.equippedGear;
        }
        private void SetBaitData(GameData _playerData)
        {
            bait = _playerData.bait;
            baitCounts = _playerData.baitCounts;
        }
        private void SetTutorialsData(GameData _playerData)
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
        private void SetFishableRecordsData(GameData _playerData)
        {
            caughtFish = _playerData.caughtFish;
            fishCatchAmounts = _playerData.fishCatchAmounts;
            lengthRecords = _playerData.lengthRecords;
            weightRecords = _playerData.weightRecords;
        }

        public void UpdateFishRecordData(string _fishName, float _length, float _weight)
        {
            int i = 0;
            foreach (string _name in caughtFish)
            {
                if (_name == _fishName)
                {
                    fishCatchAmounts[i]++;
                    if (_length > lengthRecords[i]) lengthRecords[i] = _length;
                    if (_weight > weightRecords[i]) weightRecords[i] = _weight;
                    return;
                }
                i++;
            }

            caughtFish.Add(_fishName);
            fishCatchAmounts.Add(1);
            lengthRecords.Add(_length);
            weightRecords.Add(_weight);
        }
    }
}