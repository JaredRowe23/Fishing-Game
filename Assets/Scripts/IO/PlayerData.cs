using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.UI;

namespace Fishing.IO
{
    public class PlayerData : MonoBehaviour
    {
        [Header("Save File Data")]
        public SaveFileData saveFileData;
        private System.DateTime sessionStartTime;

        [Header("Bucket Items Data")]
        public List<BucketItemSaveData> bucketItemSaveData;

        [Header("Fishing Rods")]
        public List<FishingRodSaveData> fishingRodSaveData;
        public FishingRodSaveData equippedRod;

        [Header("Bait")]
        public List<BaitSaveData> baitSaveData;

        [Header("Tutorials")]
        public HasSeenTutorialData hasSeenTutorialData;

        [Header("Records")]
        public List<RecordSaveData> recordSaveData;

        public delegate void OnMoneyUpdated();
        public static event OnMoneyUpdated onMoneyUpdated;
        private float previousMoney;

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

            previousMoney = saveFileData.money;
        }

        public void Update() {

            if (previousMoney != saveFileData.money) {
                onMoneyUpdated?.Invoke();
                previousMoney = saveFileData.money;
            }
        }

        public void SavePlayer()
        {
            TooltipSystem.instance.NewTooltip(5f, "Game Saved!");

            System.TimeSpan currentSessionTime = System.DateTime.Now.Subtract(sessionStartTime);
            System.TimeSpan previousPlaytime = System.TimeSpan.Parse(saveFileData.playtime);
            System.TimeSpan addPlaytime = previousPlaytime.Add(currentSessionTime);
            saveFileData.playtime = string.Format("{0}:{1}:{2}", addPlaytime.Hours, addPlaytime.Minutes, addPlaytime.Seconds);
            saveFileData.dateTime = System.DateTime.Now.ToString("G");

            SaveManager.SaveGame(this, saveFileData.playerName);
        }

        public void LoadPlayer(GameData _saveData)
        {
            saveFileData = _saveData.saveFileData;
            bucketItemSaveData = _saveData.bucketItemSaveData;
            fishingRodSaveData = _saveData.fishingRodSaveData;
            baitSaveData = _saveData.baitSaveData;
            hasSeenTutorialData = _saveData.hasSeenTutorialData;
            recordSaveData = _saveData.recordSaveData;

            sessionStartTime = System.DateTime.Now;
            equippedRod = _saveData.equippedRod;
        }

        public void NewGame()
        {
            saveFileData = new SaveFileData("", "World Map", 0, System.DateTime.Now.ToString("G"), System.TimeSpan.Zero.ToString());
            hasSeenTutorialData = new HasSeenTutorialData(false);
            bucketItemSaveData = new List<BucketItemSaveData>();
            fishingRodSaveData = new List<FishingRodSaveData>();
            baitSaveData = new List<BaitSaveData>();
            recordSaveData = new List<RecordSaveData>();

            sessionStartTime = System.DateTime.Now;

            FishingRodSaveData _defaultRod = new FishingRodSaveData("Wooden Fishing Rod", "", "", null);
            fishingRodSaveData.Add(_defaultRod);
            equippedRod = fishingRodSaveData[0];
        }

        public void AddBait(string _baitName)
        {
            for (int i = 0; i < baitSaveData.Count; i++)
            {
                if (_baitName != baitSaveData[i].baitName) continue;
                baitSaveData[i].amount++;
                return;
            }
            baitSaveData.Add(new BaitSaveData(_baitName, 1));
        }

        public void UpdateFishRecordData(BucketItemSaveData _data)
        {
            for (int i = 0; i < recordSaveData.Count; i++)
            {
                if (_data.itemName != recordSaveData[i].itemName) continue;

                recordSaveData[i].amountCaught++;
                if (recordSaveData[i].lengthRecord < _data.length) recordSaveData[i].lengthRecord = _data.length;
                if (recordSaveData[i].weightRecord < _data.weight) recordSaveData[i].weightRecord = _data.weight;

                return;
            }

            recordSaveData.Add(new RecordSaveData(_data.itemName, 1, _data.length, _data.weight));
        }
    }
}