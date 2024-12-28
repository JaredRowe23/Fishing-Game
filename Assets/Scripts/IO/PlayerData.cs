using Fishing.UI;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Fishing.IO {
    [System.Serializable]
    public class PlayerData {
        [Header("Save File Data")]
        [SerializeField] private SaveFileData _saveFileData;
        public SaveFileData SaveFileData { get => _saveFileData; private set => _saveFileData = value; }

        [SerializeField] private System.DateTime _sessionStartTime;

        [Header("Bucket Items Data")]
        [SerializeField] private List<BucketItemSaveData> _bucketItemSaveData;
        public List<BucketItemSaveData> BucketItemSaveData { get => _bucketItemSaveData; private set => _bucketItemSaveData = value; }

        [Header("Fishing Rods")]
        [SerializeField] private List<FishingRodSaveData> _fishingRodSaveData;
        public List<FishingRodSaveData> FishingRodSaveData { get => _fishingRodSaveData; private set => _fishingRodSaveData = value; }

        public UnityAction ChangedEquippedRod;
        [SerializeField] private FishingRodSaveData _equippedRod;
        public FishingRodSaveData EquippedRod {
            get => _equippedRod;
            set {
                if (_equippedRod != value) {
                    _equippedRod = value;
                    ChangedEquippedRod?.Invoke();
                }
            }
        }

        [Header("Bait")]
        [SerializeField] private List<BaitSaveData> _baitSaveData;
        public List<BaitSaveData> BaitSaveData { get => _baitSaveData; private set => _baitSaveData = value; }

        [Header("Tutorials")]
        [SerializeField] private HasSeenTutorialData _hasSeenTutorialData;
        public HasSeenTutorialData HasSeenTutorialData { get => _hasSeenTutorialData; private set => _hasSeenTutorialData = value; }

        [Header("Records")]
        [SerializeField] private List<RecordSaveData> _recordSaveData;
        public List<RecordSaveData> RecordSaveData { get => _recordSaveData; private set => _recordSaveData = value; }

        public void SavePlayer() {
            TooltipSystem.instance.NewTooltip(5f, "Game Saved!");

            System.TimeSpan currentSessionTime = System.DateTime.Now.Subtract(_sessionStartTime);
            System.TimeSpan previousPlaytime = System.TimeSpan.Parse(SaveFileData.Playtime);
            System.TimeSpan addPlaytime = previousPlaytime.Add(currentSessionTime);
            SaveFileData.Playtime = $"{addPlaytime.Hours}:{addPlaytime.Minutes}:{addPlaytime.Seconds}";
            SaveFileData.DateTime = System.DateTime.Now.ToString("G"); // TODO: Find out why this uses the G string format specifier, and if it can be changed or removed altogether

            SaveManager.Instance.SaveGame(SaveFileData.PlayerName);
        }

        public void NewGame() {
            SaveFileData = new SaveFileData("", "", 0, System.DateTime.Now.ToString("G"), System.TimeSpan.Zero.ToString());
            SaveFileData.CurrentSceneName = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(1));
            HasSeenTutorialData = new HasSeenTutorialData(false);
            BucketItemSaveData = new List<BucketItemSaveData>();
            FishingRodSaveData = new List<FishingRodSaveData>();
            BaitSaveData = new List<BaitSaveData>();
            RecordSaveData = new List<RecordSaveData>();
            for (int i = 0; i < ItemLookupTable.Instance.FishableScriptables.Count; i++) {
                RecordSaveData.Add(new RecordSaveData(ItemLookupTable.Instance.FishableScriptables[i].ItemName));
            }

            _sessionStartTime = System.DateTime.Now;

            FishingRodSaveData _defaultRod = new FishingRodSaveData(ItemLookupTable.Instance.RodScriptables[0].RodName, "", "", null);
            FishingRodSaveData.Add(_defaultRod);
            EquippedRod = _defaultRod;
        }

        public void AddBait(string baitName, int amount) {
            for (int i = 0; i < BaitSaveData.Count; i++) {
                if (baitName != BaitSaveData[i].BaitName) {
                    continue;
                }
                BaitSaveData[i].Amount += amount;
                return;
            }
            BaitSaveData.Add(new BaitSaveData(baitName, amount));
        }

        public void UpdateFishRecordData(BucketItemSaveData data) {
            for (int i = 0; i < RecordSaveData.Count; i++) {
                if (data.ItemName != RecordSaveData[i].ItemName) {
                    continue;
                }

                RecordSaveData[i].AmountCaught++;

                if (RecordSaveData[i].LengthRecord < data.Length) {
                    RecordSaveData[i].LengthRecord = data.Length;
                }

                if (RecordSaveData[i].WeightRecord < data.Weight) {
                    RecordSaveData[i].WeightRecord = data.Weight;
                }

                return;
            }
        }

        public RecordSaveData StringToFishRecordData(string dataName) {
            for (int i = 0; i < RecordSaveData.Count; i++) {
                if (dataName != RecordSaveData[i].ItemName) {
                    continue;
                }

                return RecordSaveData[i];
            }

            Debug.LogError($"Could not find fish record data from the string \"{dataName}\"");
            return null;
        }
    }
}