using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.IO
{
    public class PlayerData : MonoBehaviour
    {
        public string playerName;
        public int money;

        public List<string> fishingRods;
        public string equippedRod;

        public List<string> gear;
        public List<string> equippedGear;

        public List<string> bait;
        public List<int> baitCounts;

        private RodManager rodManager;

        public static PlayerData instance;

        private PlayerData() => instance = this;

        private void Awake()
        {
            rodManager = RodManager.instance;
        }

        private void Start()
        {
            rodManager.EquipRod(equippedRod, false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F9))
            {
                LoadPlayer();
                Debug.Log("Loaded Game Save");
            }
            if (Input.GetKeyDown(KeyCode.F5))
            {
                Debug.Log("Saved Game");
                SavePlayer();
            }
        }

        public void SavePlayer()
        {
            SaveSystem.SaveGame(this);
        }

        public void LoadPlayer()
        {
            GameData _saveData = SaveSystem.LoadGame();

            playerName = _saveData.playerName;
            money = _saveData.money;

            fishingRods = _saveData.fishingRods;
            equippedRod = _saveData.equippedRod;

            gear = _saveData.gear;
            equippedGear = _saveData.equippedGear;

            bait = _saveData.bait;
            baitCounts = _saveData.baitCounts;

            rodManager.EquipRod(equippedRod, false);
        }

        public void NewGame()
        {
            playerName = "";
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

            rodManager.EquipRod(equippedRod, false);
        }
    }
}