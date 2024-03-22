using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fishing.Fishables;

namespace Fishing.FishingMechanics
{
    public class MinigameFish : MonoBehaviour
    {
        private Image fishIcon;
        private float fishIconOffsetX;

        [SerializeField] private Image swimmingIcon;

        [Range(0f, 1.0f)]
        [SerializeField] private float fishMoveModifier = 0.5f;

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

        public static MinigameFish instance;

        private MinigameFish() => instance = this;

        private void Awake()
        {
            minigame = ReelingMinigame.instance;
            fishIcon = GetComponent<Image>();
        }

        private void Update()
        {
            if (!minigame.IsInMinigame()) return;

            swimmingIcon.gameObject.SetActive(isFishSwimming);

            MoveFishIcon();

            if (!canSwim) return;

            if (isFishSwimming)
            {
                FishSwim();

                fishMoveCount -= Time.deltaTime;
                fishSwimCount -= Time.deltaTime;
                if (fishSwimCount <= 0f)
                {
                    fishRestCount = fishRestTime + Random.Range(-fishRestTimeVariance, fishRestTimeVariance);
                    isFishSwimming = false;
                }
            }
            else
            {
                fishRestCount -= Time.deltaTime;
                if (fishRestCount <= 0f)
                {
                    fishSwimCount = fishSwimTime + Random.Range(-fishSwimTimeVariance, fishSwimTimeVariance);
                    SetNewFishPosition();
                    isFishSwimming = true;
                }
            }

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

            canSwim = _fish.GetComponent<IMovement>() != null;
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
        private void MoveFishIcon()
        {
            float _newX = Mathf.Lerp(fishIcon.rectTransform.anchoredPosition.x, fishMovePosition, fishMoveModifier);
            fishIcon.rectTransform.anchoredPosition = new Vector2(_newX, 0f);
        }

        private void FishSwim()
        {
            RodBehaviour _rod = RodManager.instance.equippedRod;

            Vector2 _dir = Vector3.Normalize(fish.transform.position - _rod.GetLinePivotPoint().position);
            float _rotRad = Mathf.Atan2(_dir.y, _dir.x);
            float _rotAngle = _rotRad * (180 / Mathf.PI);
            _rotAngle += 180f;
            fish.transform.rotation = Quaternion.Euler(fish.transform.rotation.x, fish.transform.rotation.y, _rotAngle);
            _rod.GetHook().transform.position = Vector2.MoveTowards(fish.transform.position, (Vector2)fish.transform.position + _dir * fishSwimSpeed, fishSwimSpeed * Time.deltaTime);
        }
        public float GetFishStrength() => fishStrength;
        public float GetFishDifficulty() => fishDifficulty;
        public bool IsFishSwimming() => isFishSwimming;

        public Image GetMinigameFishIcon() => fishIcon;
    }
}
