using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.FishingMechanics.Minigame {
    public class LineStress : MonoBehaviour {
        [SerializeField, Tooltip("Series of colors that the line stress bar will blend through based on how much stress there is, ordered from least to most stressed.")] private List<Color> _reelingBarFillColors;

        [SerializeField, Min(0), Tooltip("Amount of stress per second the line is relieved by.")] private float _lineStressDecayRate = 5f;

        private Image _reelingBarFill;

        private float _lineStress;

        private ReelingMinigame _minigame;
        private ReelZone _reelZone;
        private MinigameFish _minigameFish;

        private static LineStress _instance;
        public static LineStress Instance { get => _instance; private set => _instance = value; }

        private void Awake() {
            Instance = this;
            _reelingBarFill = GetComponent<Image>();
        }

        private void Start() {
            _minigame = ReelingMinigame.Instance;
            _minigameFish = MinigameFish.Instance;
            _reelZone = ReelZone.Instance;
        }

        private void FixedUpdate() {
            if (!_minigame.IsInMinigame) {
                return;
            }

            if (_minigame.IsReeling) {
                HandleReelingStress();
            }
            else {
                DecayLineStress();
            }

            _reelingBarFill.color = GetStressColor();
        }

        private void HandleReelingStress() {
            AddLineStress();

            if (!_reelZone.IsFishInReelZone()) {
                AddLineStress();
            }

            if (_minigameFish.IsSwimming) {
                AddLineStress();
            }

            if (_lineStress >= _minigame.MinigameRodScriptable.LineStrength) {
                _minigame.OnLineSnap();
                return;
            }
        }

        private void AddLineStress() {
            float stress = _minigame.HookedFishableSO.Strength * _minigame.HookedFishable.Difficulty - _minigame.MinigameRodScriptable.LineStrength;
            if (stress > 0f) {
                _lineStress += stress * Time.deltaTime;
            }
        }

        private void DecayLineStress() {
            if (_lineStress > 0f) {
                _lineStress -= _lineStressDecayRate * Time.deltaTime;
            }

            if (_lineStress < 0f) {
                _lineStress = 0f;
            }
        }

        private Color GetStressColor() {
            if (_lineStress >= _minigame.MinigameRodScriptable.LineStrength) {
                return _reelingBarFillColors[_reelingBarFillColors.Count - 1];
            }

            else if (_lineStress <= 0f) {
                return _reelingBarFillColors[0];
            }

            float normalizedStress = Mathf.InverseLerp(0f, _minigame.MinigameRodScriptable.LineStrength, _lineStress);

            float singularColorRange = 1.0f / (_reelingBarFillColors.Count - 1);

            int colorIndex = Mathf.FloorToInt(normalizedStress / singularColorRange);
            float colorBlendValue = Mathf.InverseLerp(singularColorRange * colorIndex, singularColorRange * (colorIndex + 1), normalizedStress);

            Color newColor = Color.Lerp(_reelingBarFillColors[colorIndex], _reelingBarFillColors[colorIndex + 1], colorBlendValue);

            return newColor;
        }

        public void InitializeMinigame() {
            _lineStress = 0f;
            _reelingBarFill.color = GetStressColor();
        }
    }
}
