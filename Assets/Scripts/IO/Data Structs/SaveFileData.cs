using UnityEngine;
using UnityEngine.Events;

namespace Fishing.IO {
    [System.Serializable]
    public class SaveFileData {
        [SerializeField] private string _playerName;
        public string PlayerName { get => _playerName; set => _playerName = value; }

        [SerializeField] private string _currentSceneName;
        public string CurrentSceneName { get => _currentSceneName; set => _currentSceneName = value; }

        [SerializeField] private float _money;
        public float Money {
            get => _money;
            set {
                OnMoneyUpdated?.Invoke();
                _money = value;
            }
        }

        [SerializeField] private string _dateTime;
        public string DateTime { get => _dateTime; set => _dateTime = value; }

        [SerializeField] private string _playtime;
        public string Playtime { get => _playtime; set => _playtime = value; }

        public static UnityAction OnMoneyUpdated;

        public SaveFileData(string playerName, string currentSceneName, float money, string dateTime, string playtime) {
            PlayerName = playerName;
            CurrentSceneName = currentSceneName;
            Money = money;
            DateTime = dateTime;
            Playtime = playtime;
        }
    }
}
