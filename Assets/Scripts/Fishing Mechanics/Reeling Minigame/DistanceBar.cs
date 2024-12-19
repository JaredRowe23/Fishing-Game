using Fishing.Fishables;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.FishingMechanics.Minigame {
    public class DistanceBar : MonoBehaviour {
        [SerializeField, Min(0), Tooltip("Width of the distance bar in pixels (should match size in editor)")] private float _distanceBarMaxX = 864f; // TODO: Change this to be automatically determined by UI size
        
        [SerializeField, Range(0f, 1.0f), Tooltip("Smoothing lerp value applied to hook icon when updating the fishable's distance. Can be thought of as \"Will move towards new target by this much per frame.\"")] private float _smoothing = 0.1f;

        [SerializeField, Tooltip("Text UI element above the minigame hook icon that displays the distance the fish needs to be reeled in.")] private Text _distanceText;
        [SerializeField, Tooltip("Image UI element that gives a visual representation of the fish's distance on the bar.")] private Image _hookIcon;

        private float _currentDistance;
        private float _furthestDistance;
        private float _hookIconTargetX;

        private ReelingMinigame _minigame;

        private static DistanceBar _instance;
        public static DistanceBar Instance { get => _instance; private set => _instance = value; }

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            _minigame = ReelingMinigame.Instance;
        }

        private void FixedUpdate() {
            _currentDistance = Vector2.Distance(_minigame.HookedFishable.transform.position, _minigame.MinigameRod.Hook.LinePivotPoint.position) - _minigame.MinigameRodScriptable.ReeledInDistance; // TODO: Change the hook resting position to a static position so that reeling animations don't change this distance.
            if (_currentDistance > _furthestDistance) {
                _furthestDistance = _currentDistance;
            }

            MoveIcon();

            _distanceText.text = _currentDistance.ToString("F2") + "m";
        }

        private void MoveIcon() {
            _hookIconTargetX = DistanceToBarPos();
            float newX = Mathf.Lerp(_hookIcon.rectTransform.anchoredPosition.x, _hookIconTargetX, _smoothing);
            _hookIcon.rectTransform.anchoredPosition = new Vector2(newX, 0f);
        }

        private float DistanceToBarPos() {
            float distanceValue = Mathf.InverseLerp(0f, _furthestDistance, _currentDistance);
            float barPos = Mathf.Lerp(0f, _distanceBarMaxX, distanceValue);
            return barPos;
        }

        public void InitializeMinigame() {
            _currentDistance = Vector2.Distance(_minigame.HookedFishable.transform.position, _minigame.MinigameRod.Hook.LinePivotPoint.position) - _minigame.MinigameRodScriptable.ReeledInDistance;
            _furthestDistance = _currentDistance;

            _hookIconTargetX = _distanceBarMaxX;
            _hookIcon.rectTransform.anchoredPosition = new Vector2(0f, 0f);
        }
    }
}
