using Fishing.Fishables;
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

        private Fishable _fishable;

        private float _strength;
        public float Strength { get => _strength; private set => _strength = value; }
        private float _difficulty;
        public float Difficulty { get => _difficulty; private set => _difficulty = value; }
        private float _moveTime;
        private float _moveTimeVariance;
        private float _moveDistance;
        private float _moveDistanceVariance;

        private bool _canSwim;
        private bool _isSwimming;
        public bool IsSwimming { get => _isSwimming; private set => _isSwimming = value; }
        private float _swimSpeed;
        private float _swimTime;
        private float _swimTimeVariance;
        private float _restTime;
        private float _restTimeVariance;
        private float _swimCount;
        private float _restCount;
        private float _moveCount;
        private float _movePosition;

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
            float newX = Mathf.Lerp(FishIcon.rectTransform.anchoredPosition.x, _movePosition, _fishMoveSmoothing);
            FishIcon.rectTransform.anchoredPosition = new Vector2(newX, 0f);
        }

        private void MoveHookAwayFromPlayer() {
            Vector3 direction = Vector3.Normalize(_fishable.transform.position - _rodManager.EquippedRod.LinePivotPoint.position);
            _rodManager.EquippedRod.Hook.transform.position += _swimSpeed * Time.fixedDeltaTime * direction;

            float rotationAngle = Vector2.Angle(Vector2.up, direction);
            _fishable.transform.rotation = Quaternion.Euler(0, 0, rotationAngle);
        }

        private IEnumerator Co_Swim() {
            StopCoroutine(Co_Rest());

            IsSwimming = true;
            _swimmingIcon.gameObject.SetActive(true);

            StartCoroutine(Co_Move());
            yield return new WaitForSeconds(_swimTime + Random.Range(-_swimTimeVariance, _swimTimeVariance));

            StopCoroutine(Co_Move());
            StartCoroutine(Co_Rest());
        }

        private IEnumerator Co_Move() {
            while (true) {
                SetNewFishPosition();
                yield return new WaitForSeconds(_moveTime + Random.Range(-_moveTimeVariance, _moveTimeVariance));
            }
        }

        private IEnumerator Co_Rest() {
            StopCoroutine(Co_Swim());

            _swimmingIcon.gameObject.SetActive(false);

            yield return new WaitForSeconds(_restTime + Random.Range(-_restTimeVariance, _restTimeVariance));
            StartCoroutine(Co_Swim());
        }

        private void SetNewFishPosition() {
            float moveDistance = Random.Range(_moveDistance - _moveDistanceVariance, _moveDistance + _moveDistanceVariance);
            float newFishPosX = FishIcon.rectTransform.anchoredPosition.x + moveDistance;
            _movePosition = Mathf.Clamp(newFishPosX, _fishIconOffsetX, _minigame.ReelBarMaxX - _fishIconOffsetX);
        }

        public void InitializeMinigame(Fishable fishable) {
            SetFishStats(fishable);

            FishIcon.rectTransform.anchoredPosition = Vector2.zero;
            _fishIconOffsetX = FishIcon.rectTransform.rect.width * 0.5f;
            _movePosition = Random.Range(_fishIconOffsetX, _minigame.ReelBarMaxX - _fishIconOffsetX);

            StartCoroutine(Co_Swim());
        }

        private void SetFishStats(Fishable fishable) {
            _fishable = fishable;
            MinigameStats stats = _fishable.GetComponent<MinigameStats>();

            _canSwim = _fishable.GetComponent<IMovement>() != null;
            Strength = stats.MinigameStrength;
            Difficulty = stats.MinigameDifficulty;
            _moveTime = stats.MinigameMoveTime;
            _moveTimeVariance = stats.MinigameMoveTimeVariance;
            _moveDistance = stats.MinigameMoveDistance;
            _moveDistanceVariance = stats.MinigameMoveDistanceVariance;
            _swimSpeed = stats.MinigameSwimSpeed;
            _swimTime = stats.MinigameSwimTime;
            _swimTimeVariance = stats.MinigameSwimTimeVariance;
            _restTime = stats.MinigameRestTime;
            _restTimeVariance = stats.MinigameRestTimeVariance;
        }
    }
}
