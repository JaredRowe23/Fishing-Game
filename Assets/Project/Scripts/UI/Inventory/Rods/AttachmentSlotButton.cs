using Fishing.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI {
    public abstract class AttachmentSlotButton : MonoBehaviour {
        [SerializeField, Tooltip("ScrollRect UI this button reveals when pressed.")] protected ScrollRect _scrollRect;
        [SerializeField, Tooltip("Text UI that displays the name of the current attachment in this slot.")] protected Text _attachmentName;
        [SerializeField, Tooltip("Image UI that displays the sprite of the current attachment in this slot.")] protected Image _attachmentSprite;
        [SerializeField, Tooltip("Prefab of the options available for this attachment slot.")] protected GameObject _optionPrefab;

        protected PlayerData _playerData;

        private void Start() {
            _playerData = SaveManager.Instance.LoadedPlayerData;
        }

        public virtual void UpdateButton(string name, Sprite sprite) {
            _attachmentName.text = name;
            _attachmentSprite.sprite = sprite;
        }

        public void ToggleScrollRect() { // Called by button
            if (!_scrollRect.gameObject.activeSelf) {
                _scrollRect.gameObject.SetActive(true);
                GenerateSlotOptions();
            }
            else {
                _scrollRect.gameObject.SetActive(false);
                DestroySlotOptions();
            }
        }

        public abstract void GenerateSlotOptions();

        protected virtual void DestroySlotOptions() {
            foreach (Transform child in _scrollRect.content.transform) {
                Destroy(child.gameObject);
            }
        }

        protected virtual void OnDisable() {
            DestroySlotOptions();
            _scrollRect.gameObject.SetActive(false);
        }
    }
}