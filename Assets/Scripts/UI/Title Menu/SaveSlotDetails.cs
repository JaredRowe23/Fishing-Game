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

        public void UpdateInfo(string _saveName, float _money, string _saveTime, string _playtime)
        {
            saveNameText.text = _saveName;
            moneyText.text = "$" + _money.ToString("F2");
            saveTimeDateText.text = _saveTime;
            playTimeText.text = _playtime;
        }
    }
}
