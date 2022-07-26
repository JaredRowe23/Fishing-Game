using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public string equippedRod;

        [Header("Gear")]
        public List<string> gear;
        public List<string> equippedGear;

        [Header("Bait")]
        public List<string> bait;
        public List<int> baitCounts;

        public string currentSceneName;

        public static PlayerData instance;

        private PlayerData() => instance = this;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void SavePlayer()
        {
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
            equippedRod = _saveData.equippedRod;

            gear = _saveData.gear;
            equippedGear = _saveData.equippedGear;

            bait = _saveData.bait;
            baitCounts = _saveData.baitCounts;
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

            fishingRods = new List<string>()
            {
                "Basic Rod"
            };
            equippedRod = fishingRods[0];

            gear = new List<string>();
            equippedGear = new List<string>();

            bait = new List<string>();
            baitCounts = new List<int>();
        }
    }
}