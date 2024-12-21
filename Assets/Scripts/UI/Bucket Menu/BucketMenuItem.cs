using Fishing.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI {
    public class BucketMenuItem : MonoBehaviour {
        [SerializeField, Tooltip("Text UI that displays the name of the bucket item.")] private Text _nameText;
        [SerializeField, Tooltip("Text UI that displays the weight of the bucket item.")] private Text _weightText;
        [SerializeField, Tooltip("Text UI that displays the length of the bucket item.")] private Text _lengthText;
        [SerializeField, Tooltip("Text UI that displays the value of the bucket item.")] private Text _valueText;
        private BucketItemSaveData _data;

        private UIManager _UIManager;

        private void Start() {
            _UIManager = UIManager.instance;
        }

        public void UpdateInfo(BucketItemSaveData item) {
            _data = item;
            _nameText.text = item.ItemName;
            _weightText.text = $"{item.Weight.ToString("F2")} kg";
            _lengthText.text = $"{item.Length.ToString("F2")} cm";
            _valueText.text = item.Value.ToString("C");
        }

        public void OpenInfoMenu() {
            if (!_UIManager.itemInfoMenu.activeSelf) {
                _UIManager.itemInfoMenu.SetActive(true);
            }
            _UIManager.itemInfoMenu.GetComponent<ItemInfoMenu>().UpdateMenu(_data, this);
        }
    }

}