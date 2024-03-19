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
            playerName = _saveData.playerName;
            currentSceneName = _saveData.currentSceneName;
            dateTime = _saveData.dateTime;
            playtime = _saveData.playtime;
            sessionStartTime = System.DateTime.Now;

            bucketFish = _saveData.bucketFish;
            bucketFishDescription = _saveData.bucketFishDescription;
            bucketFishWeight = _saveData.bucketFishWeight;
            bucketFishLength = _saveData.bucketFishLength;
            bucketFishValue = _saveData.bucketFishValue;

            money = _saveData.money;

            fishingRods = _saveData.fishingRods;
            equippedLines = _saveData.equippedLines;
            equippedHooks = _saveData.equippedHooks;
            equippedBaits = _saveData.equippedBaits;
            equippedRod = _saveData.equippedRod;

            gear = _saveData.gear;
            equippedGear = _saveData.equippedGear;

            bait = _saveData.bait;
            baitCounts = _saveData.baitCounts;

            hasSeenCastTut = _saveData.hasSeenCastTut;
            hasSeenBucketTut = _saveData.hasSeenBucketTut;
            hasSeenBucketMenuTut = _saveData.hasSeenBucketMenuTut;
            hasSeenBaitTut = _saveData.hasSeenBaitTut;
            hasSeenReelingTut = _saveData.hasSeenReelingTut;
            hasSeenReelingMinigameTut = _saveData.hasSeenReelingMinigameTut;
            hasSeenInventoryTut = _saveData.hasSeenInventoryTut;
            hasSeenFishTut = _saveData.hasSeenFishTut;
            hasSeenNPCTut = _saveData.hasSeenNPCTut;
        }

        public void NewGame()
        {
            playerName = "";
            currentSceneName = "World Map";
            dateTime = System.DateTime.Now.ToString("G");
            sessionStartTime = System.DateTime.Now;
            playtime = System.TimeSpan.Zero.ToString();

            bucketFish = new List<string>();
            bucketFishDescription = new List<string>();
            bucketFishWeight = new List<float>();
            bucketFishLength = new List<float>();
            bucketFishValue = new List<float>();

            money = 0;

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

            hasSeenCastTut = hasSeenBucketTut = hasSeenBucketMenuTut = hasSeenBaitTut = hasSeenReelingTut = hasSeenReelingMinigameTut = hasSeenInventoryTut = hasSeenFishTut = hasSeenNPCTut = false;
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
    }
}