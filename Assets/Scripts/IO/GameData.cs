using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Fishing.IO
{
    [System.Serializable]
    public class GameData
    {
        [Header("Save File Data")]
        public SaveFileData saveFileData;

        [Header("Fish Inventory")]
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

        public GameData(PlayerData _playerData)
        {
            saveFileData = _playerData.saveFileData;
            bucketItemSaveData = _playerData.bucketItemSaveData;
            fishingRodSaveData = _playerData.fishingRodSaveData;
            baitSaveData = _playerData.baitSaveData;
            hasSeenTutorialData = _playerData.hasSeenTutorialData;
            recordSaveData = _playerData.recordSaveData;
            equippedRod = _playerData.equippedRod;
        }
    }
}