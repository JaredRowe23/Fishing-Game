using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fishing.IO;

namespace Fishing
{
    public class MoneyUI : MonoBehaviour {
        [SerializeField] private Text moneyText;

        private void Awake(){
            UpdateMoneyText();
            SaveFileData.onMoneyUpdated += UpdateMoneyText;
        }

        private void OnDestroy() {
            SaveFileData.onMoneyUpdated -= UpdateMoneyText;
        }

        public void UpdateMoneyText() {
            moneyText.text = SaveManager.Instance.LoadedPlayerData.SaveFileData.Money.ToString("C");
        }
    }
}
