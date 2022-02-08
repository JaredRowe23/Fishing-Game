using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        GenerateRod();
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
        GameData saveData = SaveSystem.LoadGame();

        playerName = saveData.playerName;
        money = saveData.money;

        fishingRods = saveData.fishingRods;
        equippedRod = saveData.equippedRod;

        gear = saveData.gear;
        equippedGear = saveData.equippedGear;

        bait = saveData.bait;
        baitCounts = saveData.baitCounts;

        GenerateRod();
    }

    public void NewGame()
    {
        playerName = "";
        money = 0;
        fishingRods = new List<string>();
        fishingRods.Add("Basic Rod");
        equippedRod = fishingRods[0];
        gear = new List<string>();
        equippedGear = new List<string>();
        bait = new List<string>();
        baitCounts = new List<int>();

        GenerateRod();
    }

    private void GenerateRod()
    {
        if (this.GetComponent<GameController>().equippedRod != null)
        {
            Destroy(this.GetComponent<GameController>().equippedRod);
        }
        else
        {
            equippedRod = "Basic Rod";
        }

        foreach (GameObject prefab in RodsMenu.instance.rodPrefabs)
        {
            if (prefab.name != equippedRod) continue;

            GameObject newRod = Instantiate(prefab);
            this.GetComponent<GameController>().equippedRod = newRod;
            foreach (Transform child in newRod.transform)
            {
                if (child.GetComponent<HookControl>())
                {
                    Camera.main.GetComponent<CameraBehaviour>().hook = child.GetComponent<HookControl>();
                    Camera.main.transform.parent = child.transform;
                }
            }
        }
    }
}
