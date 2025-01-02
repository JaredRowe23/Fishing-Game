using Fishing.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI {
    public class SaveSlotListing : MonoBehaviour {
        [SerializeField, Tooltip("Text UI that displays this save slot's name.")] private Text _saveNameText;
        [SerializeField, Tooltip("Text UI that displays this save slot's time and date.")] private Text _saveTimeDateText;
        private int _saveIndex;
        public int SaveIndex { get => _saveIndex; private set { _saveIndex = value; } }

        public void SetInfo(string saveName, string saveDate, int saveIndex) {
            _saveNameText.text = saveName;
            _saveTimeDateText.text = saveDate;
            SaveIndex = saveIndex;
        }
    }
}
