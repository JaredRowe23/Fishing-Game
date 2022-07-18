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

        public static SaveSlotDetails instance;

        private SaveSlotDetails() => instance = this;

        public void UpdateInfo(string _saveName, int _money)
        {
            saveNameText.text = _saveName;
            moneyText.text = _money.ToString();
        }
    }
}
