using Fishing.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI {
    public class SaveSlotDetails : MonoBehaviour {
        [SerializeField, Tooltip("Text UI that displays this slot's name.")] private Text _saveNameText;
        [SerializeField, Tooltip("Text UI that displays this slot's save time and date.")] private Text _saveTimeDateText;
        [SerializeField, Tooltip("Text UI that displays this slot's current money.")] private Text _moneyText;
        [SerializeField, Tooltip("Text UI that displays this slot's completion percentage.")] private Text _percentageText;
        [SerializeField, Tooltip("Text UI that displays this slot's fish types caught.")] private Text _fishCaughtText;
        [SerializeField, Tooltip("Text UI that displays this slot's total playtime.")] private Text _playTimeText;

        public void UpdateInfo(SaveFile saveFile) {
            _saveNameText.text = saveFile.Name;
            _moneyText.text = saveFile.Money.ToString("C");
            _saveTimeDateText.text = saveFile.DateTime;
            _playTimeText.text = saveFile.Playtime;
            _fishCaughtText.text = $"{saveFile.FishTypesCaught} / 11";
        }

        private void OnDisable() {
            gameObject.transform.parent.gameObject.SetActive(false);
        }
    }
}
