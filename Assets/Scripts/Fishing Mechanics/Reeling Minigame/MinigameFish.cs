using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fishing.Fishables;

namespace Fishing.FishingMechanics.Minigame
{
    public class MinigameFish : MonoBehaviour
    {
        private Image fishIcon;
        private float fishIconOffsetX;

        [SerializeField] private Image swimmingIcon;

        [Range(0f, 1.0f)]
        [SerializeField] private float fishMoveSmoothing = 0.1f;

        private Fishable fish;

        private float fishStrength;
        private float fishDifficulty;
        private float fishMoveTime;
        private float fishMoveTimeVariance;
        private float fishMoveDistance;
        private float fishMoveDistanceVariance;

        private bool canSwim;
        private bool isFishSwimming;
        private float fishSwimSpeed;
        private float fishSwimTime;
        private float fishSwimTimeVariance;
        private float fishRestTime;
        private float fishRestTimeVariance;
        private float fishSwimCount;
        private float fishRestCount;
        private float fishMoveCount;
        private float fishMovePosition;

        private ReelingMinigame minigame;
        private RodManager rodManager;

        public static MinigameFish instance;

        private MinigameFish() => instance = this;

        private void Awake()
        {
            minigame = ReelingMinigame.instance;
            fishIcon = GetComponent<Image>();
            rodManager = RodManager.instance;
        }

        private void Update()
        {
            if (!minigame.IsInMinigame()) return;

            swimmingIcon.gameObject.SetActive(isFishSwimming);

            MoveFishIcon();

            if (!canSwim) return;

            if (isFishSwimming) HandleSwimming();
            else HandleResting();

            if (fishMoveCount <= 0f) SetNewFishPosition();
        }

        public void InitializeMinigame(Fishable _fish)
        {
            SetFishStats(_fish);

            fishIcon.rectTransform.anchoredPosition = Vector2.zero;
            fishMovePosition = Random.Range(0f + fishIconOffsetX, minigame.GetXAxisMax() - fishIconOffsetX);
            fishMoveCount = fishMoveTime + Random.Range(-fishMoveTimeVariance, fishMoveTimeVariance);

            fishIconOffsetX = fishIcon.rectTransform.rect.width * 0.5f;
        }

        private void SetFishStats(Fishable _fish)
        {
            fish = _fish;

            canSwim = fish.GetComponent<IMovement>() != null;
            fishStrength = fish.GetMinigameStrength();
            fishDifficulty = fish.GetMinigameDifficulty();
            fishMoveTime = fish.GetMinigameMoveTime();
            fishMoveTimeVariance = fish.GetMinigameMoveTimeVariance();
            fishMoveDistance = fish.GetMinigameMoveDistance();
            fishMoveDistanceVariance = fish.GetMinigameMoveDistanceVariance();
            fishSwimSpeed = fish.GetMinigameSwimSpeed();
            fishSwimTime = fish.GetMinigameSwimTime();
            fishSwimTimeVariance = fish.GetMinigameSwimTimeVariance();
            fishRestTime = fish.GetMinigameRestTime();
            fishRestTimeVariance = fish.GetMinigameRestTimeVariance();
        }

        private void SetNewFishPosition()
        {
            float _moveDistance = Random.Range(-fishMoveDistance + Random.Range(-fishMoveDistanceVariance, fishMoveDistanceVariance), fishMoveDistance + Random.Range(-fishMoveDistanceVariance, fishMoveDistanceVariance));
            float _newFishPosX = fishIcon.rectTransform.anchoredPosition.x + _moveDistance;
            fishMovePosition = Mathf.Clamp(_newFishPosX, 0f + fishIconOffsetX, minigame.GetXAxisMax() - fishIconOffsetX);
            fishMoveCount = fishMoveTime + Random.Range(-fishMoveTimeVariance, fishMoveTimeVariance);
        }

        private void HandleSwimming()
        {
            MoveHook();

            fishMoveCount -= Time.deltaTime;
            fishSwimCount -= Time.deltaTime;
            if (fishSwimCount <= 0f)
            {
                fishRestCount = fishRestTime + Random.Range(-fishRestTimeVariance, fishRestTimeVariance);
                isFishSwimming = false;
            }
        }

        private void HandleResting()
        {
            fishRestCount -= Time.deltaTime;
            if (fishRestCount <= 0f)
            {
                fishSwimCount = fishSwimTime + Random.Range(-fishSwimTimeVariance, fishSwimTimeVariance);
                SetNewFishPosition();
                isFishSwimming = true;
            }
        }

        private void MoveFishIcon()
        {
            float _newX = Mathf.Lerp(fishIcon.rectTransform.anchoredPosition.x, fishMovePosition, fishMoveSmoothing);
            fishIcon.rectTransform.anchoredPosition = new Vector2(_newX, 0f);
        }

        private void MoveHook()
        {
            Vector2 _dir = Vector3.Normalize(fish.transform.position - rodManager.equippedRod.GetLinePivotPoint().position);
            float _rotationAngle = Vector2.Angle(Vector2.up, _dir);
            fish.transform.rotation = Quaternion.Euler(0, 0, _rotationAngle);
            rodManager.equippedRod.GetHook().transform.position += (Vector3)_dir * fishSwimSpeed * Time.deltaTime;
        }

        public float GetFishStrength() => fishStrength;
        public float GetFishDifficulty() => fishDifficulty;
        public bool IsFishSwimming() => isFishSwimming;

        public Image GetMinigameFishIcon() => fishIcon;
    }
}
