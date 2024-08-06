using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.FishingMechanics.Minigame
{
    public class LineStress : MonoBehaviour
    {

        [SerializeField] private List<Color> reelingBarFillColors;

        [SerializeField] private float lineStressDecayRate = 1f;

        private Image reelingBarFill;

        private float lineStrength;
        private float lineStress;

        private ReelingMinigame minigame;
        private ReelZone reelZone;
        private MinigameFish minigameFish;

        public static LineStress instance;

        private LineStress() => instance = this;

        private void Awake()
        {
            reelingBarFill = GetComponent<Image>();
            minigame = ReelingMinigame.instance;
            minigameFish = MinigameFish.instance;
            reelZone = ReelZone.instance;
        }

        private void Update()
        {
            if (!minigame.IsInMinigame()) return;

            if (minigame.IsReeling())
            {
                AddLineStress();
                if (!reelZone.IsFishInReelZone()) AddLineStress();
                if (minigameFish.IsFishSwimming()) AddLineStress();
                if (lineStress >= lineStrength)
                {
                    minigame.OnLineSnap();
                    return;
                }
            }
            else
            {
                if (lineStress > 0f) lineStress -= lineStressDecayRate * Time.deltaTime;
                if (lineStress < 0f) lineStress = 0f;
            }

            reelingBarFill.color = GetStressColor();
        }

        public void InitializeMinigame()
        {
            lineStress = 0f;
            lineStrength = RodManager.instance.equippedRod.scriptable.lineStrength;
            reelingBarFill.color = GetStressColor();
        }

        private void AddLineStress()
        {
            float _stress = minigameFish.GetFishStrength() * minigameFish.GetFishDifficulty() - lineStrength;
            if (_stress > 0f) lineStress += _stress * Time.deltaTime;
        }

        private Color GetStressColor()
        {
            if (lineStress >= lineStrength) return reelingBarFillColors[reelingBarFillColors.Count - 1];
            else if (lineStress <= 0f) return reelingBarFillColors[0];

            float _normalizedStress = Mathf.InverseLerp(0f, lineStrength, lineStress);

            float _colorFactor = 1.0f / (reelingBarFillColors.Count - 1);

            int _gradientStartIndex = Mathf.FloorToInt(_normalizedStress / _colorFactor);
            float _gradientValueNormalized = Mathf.InverseLerp(_colorFactor * _gradientStartIndex, _colorFactor * (_gradientStartIndex + 1), _normalizedStress);

            Color _newColor = Color.Lerp(reelingBarFillColors[_gradientStartIndex], reelingBarFillColors[_gradientStartIndex + 1], _gradientValueNormalized);

            return _newColor;
        }
    }
}
