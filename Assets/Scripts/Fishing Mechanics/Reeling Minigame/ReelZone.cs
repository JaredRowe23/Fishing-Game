using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.FishingMechanics
{
    public class ReelZone : MonoBehaviour
    {
        [SerializeField] private float reelZoneImageWidth;

        private Image img;

        private float reelZoneWidth;
        private float reelZoneForce;
        private float reelZoneMaxVelocity;
        private float reelZoneGravity;

        private float reelZoneVelocity;

        private bool isInReelZone;

        private ReelingMinigame minigame;
        private MinigameFish minigameFish;
        private RodManager rodManager;

        public static ReelZone instance;

        private ReelZone() => instance = this;

        private void Awake()
        {
            img = GetComponent<Image>();
            minigame = ReelingMinigame.instance;
            minigameFish = MinigameFish.instance;
            rodManager = RodManager.instance;
        }

        private void Update()
        {
            if (!minigame.IsInMinigame()) return;

            if (minigame.IsReeling()) AddReelingForce();
            else ApplyGravityToReel();

            MoveReelZone();

            isInReelZone = IsFishInReelZone();

            if (isInReelZone) rodManager.equippedRod.StartReeling();
            else rodManager.equippedRod.StopReeling();
        }

        public void InitializeMinigame()
        {
            RodBehaviour _rod = RodManager.instance.equippedRod;

            reelZoneWidth = _rod.scriptable.reelZoneWidth;
            reelZoneForce = _rod.scriptable.reelZoneForce;
            reelZoneMaxVelocity = _rod.scriptable.reelZoneMaxVelocity;
            reelZoneGravity = _rod.scriptable.reelZoneGravity;

            img.rectTransform.sizeDelta = new Vector2(reelZoneImageWidth + reelZoneWidth, 0f);
            img.rectTransform.anchoredPosition = Vector2.zero;
            reelZoneVelocity = 0f;
        }

        private void MoveReelZone()
        {
            float _newPosX = Mathf.Clamp(img.rectTransform.anchoredPosition.x + reelZoneVelocity, 0f, minigame.GetXAxisMax() - img.rectTransform.sizeDelta.x);
            img.rectTransform.anchoredPosition = new Vector2(_newPosX, 0f);
        }

        private void AddReelingForce() => reelZoneVelocity = Mathf.Clamp(reelZoneVelocity + reelZoneForce * Time.deltaTime, 0f, reelZoneMaxVelocity);

        private void ApplyGravityToReel()
        {
            float _reelZonePos = img.rectTransform.anchoredPosition.x;
            if (_reelZonePos <= 0f && reelZoneVelocity < 0f) reelZoneVelocity = 0f;
            else if (_reelZonePos >= minigame.GetXAxisMax() - img.rectTransform.sizeDelta.x && reelZoneVelocity > 0f) reelZoneVelocity = 0f;
            else reelZoneVelocity = Mathf.Clamp(reelZoneVelocity - reelZoneGravity * Time.deltaTime, -reelZoneMaxVelocity, reelZoneMaxVelocity);
        }
        public bool IsFishInReelZone()
        {
            float _fishIconPos = minigameFish.GetMinigameFishIcon().rectTransform.anchoredPosition.x;

            float _reelZoneXMin = img.rectTransform.anchoredPosition.x;
            float _reelZoneXMax = _reelZoneXMin + img.rectTransform.sizeDelta.x;

            bool _isInZone = _fishIconPos >= _reelZoneXMin && _fishIconPos <= _reelZoneXMax;

            return _isInZone;
        }
    }
}