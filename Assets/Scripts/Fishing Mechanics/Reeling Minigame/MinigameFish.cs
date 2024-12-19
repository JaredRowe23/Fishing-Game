using Fishing.Fishables;
using Fishing.Fishables.Fish;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.FishingMechanics.Minigame {
    public class MinigameFish : MonoBehaviour {
        [SerializeField, Range(0f, 1.0f), Tooltip("Lerp smoothing applied when moving the fish to a new reeling bar location. Can be thought of as \"Will move towards new target by this much per frame.\"")] private float _fishMoveSmoothing = 0.1f;
        [SerializeField, Tooltip("Image UI element used for indicating when the fish is swimming.")] private Image _swimmingIcon;

        private Image _fishIcon;
        public Image FishIcon { get => _fishIcon; private set => _fishIcon = value; }
        private float _fishIconOffsetX;

        private bool _canSwim;
        private bool _isSwimming;
        public bool IsSwimming { get => _isSwimming; private set => _isSwimming = value; }
        private float _targetPosition;

        private ReelingMinigame _minigame;
        private RodManager _rodManager;

        private static MinigameFish _instance;
        public static MinigameFish Instance { get => _instance; set => _instance = value; }

        private void Awake() {
            Instance = this;
            FishIcon = GetComponent<Image>();
        }

        private void Start() {
            _minigame = ReelingMinigame.Instance;
            _rodManager = RodManager.Instance;
        }

        private void FixedUpdate() {
            if (!_minigame.IsInMinigame) {
                return;
            }

            MoveFishIcon();

            if (!_canSwim) {
                return;
            }

            if (IsSwimming) {
                MoveHookAwayFromPlayer();
            }
        }

        private void MoveFishIcon() {
            float newX = Mathf.Lerp(FishIcon.rectTransform.anchoredPosition.x, _targetPosition, _fishMoveSmoothing);
            FishIcon.rectTransform.anchoredPosition = new Vector2(newX, 0f);
        }

        private void MoveHookAwayFromPlayer() {
            Vector3 direction = Vector3.Normalize(_minigame.HookedFishable.transform.position - _rodManager.EquippedRod.LinePivotPoint.position);
            _rodManager.EquippedRod.Hook.transform.position += _minigame.HookedFishableSO.SwimSpeed * Time.fixedDeltaTime * direction;

            float rotationAngle = Vector2.Angle(Vector2.up, direction);
            _minigame.HookedFishable.transform.rotation = Quaternion.Euler(0, 0, rotationAngle);
        }

        private IEnumerator Co_Swim() {
            StopCoroutine("Co_Rest");

            IsSwimming = true;
            _swimmingIcon.gameObject.SetActive(true);

            StartCoroutine("Co_Move");
            yield return new WaitForSeconds(_minigame.HookedFishableSO.GetRandomSwimTime());

            StopCoroutine("Co_Move");
            StartCoroutine("Co_Rest");
        }

        private IEnumerator Co_Move() {
            while (true) {
                SetNewFishPosition();
                yield return new WaitForSeconds(_minigame.HookedFishableSO.GetRandomMoveTime());
            }
        }

        private IEnumerator Co_Rest() {
            StopCoroutine("Co_Move");
            StopCoroutine("Co_Swim");

            _swimmingIcon.gameObject.SetActive(false);

            yield return new WaitForSeconds(_minigame.HookedFishableSO.GetRandomRestTime());
            StartCoroutine("Co_Swim");
        }

        private void SetNewFishPosition() {
            float moveDistance = _minigame.HookedFishableSO.GetRandomMoveDistance();
            float newFishPosX = FishIcon.rectTransform.anchoredPosition.x + moveDistance;
            _targetPosition = Mathf.Clamp(newFishPosX, _fishIconOffsetX, _minigame.ReelBarMaxX - _fishIconOffsetX);
        }

        public void InitializeMinigame() {
            _canSwim = _minigame.HookedFishable.TryGetComponent(out IMovement _);

            FishIcon.rectTransform.anchoredPosition = Vector2.zero;
            _fishIconOffsetX = FishIcon.rectTransform.rect.width * 0.5f;
            _targetPosition = Random.Range(_fishIconOffsetX, _minigame.ReelBarMaxX - _fishIconOffsetX);

            StartCoroutine("Co_Swim");
        }
    }
}
