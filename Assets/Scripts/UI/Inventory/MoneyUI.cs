using Fishing.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing {
    public class MoneyUI : MonoBehaviour {
        [SerializeField, Tooltip("Text UI that displays the player's current money.")] private Text _moneyText;
        private PlayerData _playerData;

        private void Awake() {
            _playerData = SaveManager.Instance.LoadedPlayerData;
        }

        public void UpdateMoneyText() {
            _moneyText.text = _playerData.SaveFileData.Money.ToString("C");
        }

        private void OnEnable() {
            SaveFileData.OnMoneyUpdated += UpdateMoneyText;
        }

        private void OnDisable() {
            SaveFileData.OnMoneyUpdated -= UpdateMoneyText;
        }
    }
}
