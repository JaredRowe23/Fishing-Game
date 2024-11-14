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
            PlayerData.onMoneyUpdated += UpdateMoneyText;
        }

        private void OnDestroy() {
            PlayerData.onMoneyUpdated -= UpdateMoneyText;
        }

        public void UpdateMoneyText() {
            moneyText.text = PlayerData.instance.saveFileData.money.ToString("C");
        }
    }
}
