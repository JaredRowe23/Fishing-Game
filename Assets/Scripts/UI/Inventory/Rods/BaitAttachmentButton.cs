using UnityEngine;

namespace Fishing.UI {
    public class BaitAttachmentButton : AttachmentSlotButton {
        private static BaitAttachmentButton _instance;
        public static BaitAttachmentButton Instance { get => _instance; private set { _instance = value; } }

        private void Awake() {
            Instance = this;
        }

        public void UpdateSlotOptions() {
            DestroySlotOptions();
            GenerateSlotOptions();
        }

        public override void GenerateSlotOptions() {
            for (int i = 0; i < _playerData.BaitSaveData.Count; i++) {
                BaitAttachmentSlot _newSlot = Instantiate(_optionPrefab, _scrollRect.content.transform).GetComponent<BaitAttachmentSlot>();
                _newSlot.UpdateSlot(_playerData.BaitSaveData[i]);
            }
        }

        protected override void DestroySlotOptions() {
            foreach (Transform child in _scrollRect.content.transform) {
                if (child.TryGetComponent(out BaitAttachmentSlot baitSlot)) {
                    if (baitSlot.IsNoBaitOption) {
                        continue;
                    }
                }

                Destroy(child.gameObject);
            }
        }
    }
}