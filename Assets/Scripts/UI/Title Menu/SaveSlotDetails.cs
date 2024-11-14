using Fishing.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI
{
    public class SaveSlotDetails : MonoBehaviour
    {
        [SerializeField] private Text saveNameText;
        [SerializeField] private Text saveTimeDateText;
        [SerializeField] private Text moneyText;
        [SerializeField] private Text percentageText;
        [SerializeField] private Text fishCaughtText;
        [SerializeField] private Text playTimeText;

        public void UpdateInfo(SaveFile _saveFile)
        {
            saveNameText.text = _saveFile.name;
            moneyText.text = _saveFile.money.ToString("C");
            saveTimeDateText.text = _saveFile.dateTime;
            playTimeText.text = _saveFile.playtime;
            fishCaughtText.text = $"{_saveFile.fishTypesCaught} / 11";
        }
    }
}
