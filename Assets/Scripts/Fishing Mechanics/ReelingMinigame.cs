using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fishing.Fishables;
using Fishing.IO;

namespace Fishing.FishingMechanics
{
    public class ReelingMinigame : MonoBehaviour
    {
        [SerializeField] private Image reelingBarOutline;
        [SerializeField] private Image reelingBarFill;
        [SerializeField] private Image reelZone;
        [SerializeField] private Image fishIcon;

        private List<Image> minigameImages;

        [SerializeField] private List<Color> reelingBarFillColors;

        [SerializeField] private float reelZoneImageWidth;
        private float fishIconOffsetX;

        [SerializeField] private float xAxisMax = 1124f;

        [Range(0f, 1.0f)]
        [SerializeField] private float fishMoveModifier = 0.5f;

        [SerializeField] private float lineStressDecayRate = 1f;

        private Fishable fish;
        private RodScriptable rod;

        private float fishStrength;
        private float fishMoveTime;
        private float fishMoveTimeVariance;
        private float fishMoveDistance;
        private float fishMoveDistanceVariance;

        private float lineStrength;
        private float lineStress;

        private float reelZoneWidth;
        private float reelZoneForce;
        private float reelZoneMaxVelocity;
        private float reelZoneGravity;
        private bool isInReelZone;

        private float fishMoveCount;
        private float fishMovePosition;

        private bool isReeling;
        private float reelZoneVelocity;

        private bool isInMinigame = false;

        private RodBehaviour equippedRod;
        private RodManager rodManager;

        public static ReelingMinigame instance;

        private ReelingMinigame() => instance = this;

        private void Start()
        {
            rodManager = RodManager.instance;
            isInMinigame = false;

            minigameImages = new List<Image>();
            minigameImages.Add(reelingBarOutline);
            minigameImages.Add(reelingBarFill);
            minigameImages.Add(reelZone);
            minigameImages.Add(fishIcon);

            ShowMinigame(false);
        }

        private void Update()
        {
            if (!isInMinigame) return;

            fishMoveCount -= Time.deltaTime;
            if (fishMoveCount <= 0f) SetNewFishPosition();
            MoveFishIcon();

            isInReelZone = IsFishInReelZone();

            if (isReeling)
            {
                AddReelingForce();
                if (!isInReelZone) AddLineStress();
                if (lineStress >= lineStrength)
                {
                    OnLineSnap();
                    return;
                }
            }
            else 
            {
                ApplyGravityToReel();
                if (lineStress > 0f) lineStress -= lineStressDecayRate * Time.deltaTime;
                if (lineStress < 0f) lineStress = 0f;
            }

            MoveReelZone();
            reelingBarFill.color = GetStressColor();

            if (isInReelZone) equippedRod.StartReeling();
            else equippedRod.StopReeling();
        }

        public void InitiateMinigame(Fishable _fish)
        {
            SetRodStats();
            SetFishStats(_fish);

            ShowMinigame(true);

            reelZone.rectTransform.sizeDelta = new Vector2(reelZoneImageWidth + reelZoneWidth, 0f);
            reelZone.rectTransform.anchoredPosition = Vector2.zero;
            reelZoneVelocity = 0f;

            fishIcon.rectTransform.anchoredPosition = Vector2.zero;
            fishMovePosition = Random.Range(0f + fishIconOffsetX, xAxisMax - fishIconOffsetX);
            fishMoveCount = fishMoveTime + Random.Range(-fishMoveTimeVariance, fishMoveTimeVariance);

            fishIconOffsetX = fishIcon.rectTransform.rect.width * 0.5f;

            lineStress = 0f;
            reelingBarFill.color = GetStressColor();

            equippedRod.ClearReelInputs();
            InputManager.onCastReel += StartReeling;
            InputManager.releaseCastReel += EndReeling;

            isInMinigame = true;
        }

        private void SetFishStats(Fishable _fish)
        {
            fish = _fish;

            fishStrength = fish.GetMinigameStrength();
            fishMoveTime = fish.GetMinigameMoveTime();
            fishMoveTimeVariance = fish.GetMinigameMoveTimeVariance();
            fishMoveDistance = fish.GetMinigameMoveDistance();
            fishMoveDistanceVariance = fish.GetMinigameMoveDistanceVariance();
        }
        private void SetRodStats()
        {
            equippedRod = rodManager.equippedRod;
            rod = equippedRod.scriptable;

            lineStrength = rod.lineStrength;
            reelZoneWidth = rod.reelZoneWidth;
            reelZoneForce = rod.reelZoneForce;
            reelZoneMaxVelocity = rod.reelZoneMaxVelocity;
            reelZoneGravity = rod.reelZoneGravity;
        }

        private void SetNewFishPosition()
        {
            float _moveDistance = Random.Range(-fishMoveDistance + Random.Range(-fishMoveDistanceVariance, fishMoveDistanceVariance), fishMoveDistance + Random.Range(-fishMoveDistanceVariance, fishMoveDistanceVariance));
            float _newFishPosX = fishIcon.rectTransform.anchoredPosition.x + _moveDistance;
            fishMovePosition = Mathf.Clamp(_newFishPosX, 0f + fishIconOffsetX, xAxisMax - fishIconOffsetX);
            fishMoveCount = fishMoveTime + Random.Range(-fishMoveTimeVariance, fishMoveTimeVariance);
        }
        private void MoveFishIcon()
        {
            float _newX = Mathf.Lerp(fishIcon.rectTransform.anchoredPosition.x, fishMovePosition, fishMoveModifier);
            fishIcon.rectTransform.anchoredPosition = new Vector2(_newX, 0f);
        } 

        private void StartReeling() => isReeling = true;
        private void EndReeling() => isReeling = false;
        private void AddReelingForce()
        {
            reelZoneVelocity = Mathf.Clamp(reelZoneVelocity + reelZoneForce * Time.deltaTime, 0f, reelZoneMaxVelocity);
            AddLineStress();
        }
        private void ApplyGravityToReel()
        {
            float _reelZonePos = reelZone.rectTransform.anchoredPosition.x;
            if (_reelZonePos <= 0f && reelZoneVelocity < 0f) reelZoneVelocity = 0f;
            else if (_reelZonePos >= xAxisMax - reelZone.rectTransform.sizeDelta.x && reelZoneVelocity > 0f) reelZoneVelocity = 0f;
            else reelZoneVelocity = Mathf.Clamp(reelZoneVelocity - reelZoneGravity * Time.deltaTime, -reelZoneMaxVelocity, reelZoneMaxVelocity);
        } 
        private void MoveReelZone()
        {
            float _newPosX = Mathf.Clamp(reelZone.rectTransform.anchoredPosition.x + reelZoneVelocity, 0f, xAxisMax - reelZone.rectTransform.sizeDelta.x);
            reelZone.rectTransform.anchoredPosition = new Vector2(_newPosX, 0f);
        }

        private bool IsFishInReelZone()
        {
            float _fishIconPos = fishIcon.rectTransform.anchoredPosition.x;

            float _reelZoneXMin = reelZone.rectTransform.anchoredPosition.x;
            float _reelZoneXMax = _reelZoneXMin + reelZone.rectTransform.sizeDelta.x;

            bool _isInZone = _fishIconPos >= _reelZoneXMin && _fishIconPos <= _reelZoneXMax;

            return _isInZone;
        }

        private void AddLineStress()
        {
            float _additionalStress = fishStrength - lineStrength;
            if (_additionalStress > 0f) lineStress += _additionalStress * Time.deltaTime;
        }

        private Color GetStressColor()
        {
            if (lineStress >= lineStrength) return reelingBarFillColors[reelingBarFillColors.Count - 1];
            else if (lineStress <= 0f) return reelingBarFillColors[0];

            float _normalizedStress = Mathf.InverseLerp(0f, lineStrength, lineStress);

            float _colorFactor = 1.0f / (reelingBarFillColors.Count - 1);

            int _gradientStartIndex = Mathf.FloorToInt(_normalizedStress / _colorFactor);
            float _gradientBlend = _normalizedStress % _colorFactor;
            float _gradientValueNormalized = Mathf.InverseLerp(_normalizedStress - _gradientBlend, _normalizedStress - _gradientBlend + _colorFactor, _normalizedStress);

            float _r = Mathf.Lerp(reelingBarFillColors[_gradientStartIndex].r, reelingBarFillColors[_gradientStartIndex + 1].r, _gradientValueNormalized);
            float _g = Mathf.Lerp(reelingBarFillColors[_gradientStartIndex].g, reelingBarFillColors[_gradientStartIndex + 1].g, _gradientValueNormalized);
            float _b = Mathf.Lerp(reelingBarFillColors[_gradientStartIndex].b, reelingBarFillColors[_gradientStartIndex + 1].b, _gradientValueNormalized);

            Color _newColor = new Color(_r, _g, _b);

            return _newColor;
        }

        private void OnLineSnap()
        {
            equippedRod.GetHook().DespawnHookedObject();
            equippedRod.GetHook().hookedObject = null;
            equippedRod.StopReeling();

            InputManager.onCastReel -= StartReeling;
            InputManager.releaseCastReel -= EndReeling;
            equippedRod.OnReeledIn();

            EndMinigame();
        }

        private void ShowMinigame(bool _show)
        {
            foreach (Image _img in minigameImages) _img.gameObject.SetActive(_show);
        }

        public void EndMinigame()
        {
            isInMinigame = false;
            ShowMinigame(false);
        }
    }
}
