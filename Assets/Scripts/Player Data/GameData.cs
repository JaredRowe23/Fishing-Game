using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string playerName;
    public int money;
    public List<string> fishingRods;
    public string equippedRod;
    public List<string> gear;
    public List<string> equippedGear;
    public List<string> bait;
    public List<int> baitCounts;

    public GameData(PlayerData playerData)
    {
        playerName = playerData.playerName;
        money = playerData.money;

        fishingRods = playerData.fishingRods;
        equippedRod = playerData.equippedRod;

        gear = playerData.gear;
        equippedGear = playerData.equippedGear;

        bait = playerData.bait;
        baitCounts = playerData.baitCounts;
    }
}
