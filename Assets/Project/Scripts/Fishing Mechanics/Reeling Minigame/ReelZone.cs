using Fishing.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.FishingMechanics.Minigame {
    public class ReelZone : MonoBehaviour {
        [SerializeField, Min(0), Tooltip("Width in pixels of the UI image representing the reel zone.")] private float _reelZoneImageWidth; // TODO: Set this automatically from the UI image's bounds.
        [SerializeField, Range(0f, 1.0f), Tooltip("Alpha value from 0 to 1 for the fish icon when it's within the reel zone.")] private float _reelingAlpha = 1f;
        [SerializeField, Range(0f, 1.0f), Tooltip("Alpha value from 0 to 1 for the fish icon when it's outside of the reel zone.")] private float _notReelingAlpha = 0.5f;

        private Image _image;

        private float _reelZoneVelocity;

        private ReelingMinigame _minigame;
        private MinigameFish _minigameFish;

        private static ReelZone _instance;
        public static ReelZone Instance { get => _instance; private set => _instance = value; }

        private void Awake() {
            _image = GetComponent<Image>();
            Instance = this;
        }

        private void Start() {
            _minigame = ReelingMinigame.Instance;
            _minigameFish = MinigameFish.Instance;
        }

        private void FixedUpdate() {
            if (!_minigame.IsInMinigame) {
                return;
            }

            if (_minigame.IsReeling) {
                AddReelingForce();
            }
            else {
                ApplyGravityToReel();
            }

            MoveReelZone();
            HandleReelZone();
        }

        private void AddReelingForce() {
            _reelZoneVelocity = Mathf.Clamp(_reelZoneVelocity + _minigame.MinigameRodScriptable.ReelZoneForce * Time.fixedDeltaTime, 0f, _minigame.MinigameRodScriptable.ReelZoneMaxVelocity);
        }

        private void ApplyGravityToReel() {
            float reelZonePos = _image.rectTransform.anchoredPosition.x;

            if (reelZonePos <= 0f && _reelZoneVelocity < 0f) {
                _reelZoneVelocity = 0f;
            }

            else if (reelZonePos >= _minigame.ReelBarMaxX - _image.rectTransform.sizeDelta.x && _reelZoneVelocity > 0f) {
                _reelZoneVelocity = 0f;
            }

            else {
                _reelZoneVelocity = Mathf.Clamp(_reelZoneVelocity - _minigame.MinigameRodScriptable.ReelZoneGravity * Time.deltaTime, -_minigame.MinigameRodScriptable.ReelZoneMaxVelocity, _minigame.MinigameRodScriptable.ReelZoneMaxVelocity);
            }
        }

        private void MoveReelZone() {
            float newPosX = Mathf.Clamp(_image.rectTransform.anchoredPosition.x + _reelZoneVelocity, 0f, _minigame.ReelBarMaxX - _image.rectTransform.sizeDelta.x);
            _image.rectTransform.anchoredPosition = new Vector2(newPosX, 0f);
        }

        private void HandleReelZone() {
            if (IsFishInReelZone()) {
                _image.color = Utilities.SetTransparency(_image.color, _reelingAlpha);
                _minigame.MinigameRod.StartReeling();
            }
            else {
                _image.color = Utilities.SetTransparency(_image.color, _notReelingAlpha);
                _minigame.MinigameRod.StopReeling();
            }
        }

        public bool IsFishInReelZone() {
            float fishIconPos = _minigameFish.FishIcon.rectTransform.anchoredPosition.x;

            float reelZoneXMin = _image.rectTransform.anchoredPosition.x;
            float reelZoneXMax = reelZoneXMin + _image.rectTransform.sizeDelta.x;

            bool isInZone = fishIconPos >= reelZoneXMin && fishIconPos <= reelZoneXMax;

            return isInZone;
        }

        public void InitializeMinigame() {
            _image.rectTransform.sizeDelta = new Vector2(_reelZoneImageWidth + _minigame.MinigameRodScriptable.ReelZoneWidth, 0f);
            _image.rectTransform.anchoredPosition = Vector2.zero;
            _reelZoneVelocity = 0f;
        }
    }
}
