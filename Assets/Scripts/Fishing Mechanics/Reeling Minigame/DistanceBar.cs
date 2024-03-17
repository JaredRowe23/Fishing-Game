using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fishing.Fishables;

namespace Fishing.FishingMechanics
{
    public class DistanceBar : MonoBehaviour
    {
        [SerializeField] private float distanceBarMaxX = 864f;
        [Range(0f, 1.0f)]
        [SerializeField] private float smoothing = 0.1f;

        [SerializeField] private Text distanceText;
        [SerializeField] private Image hookIcon;

        private Fishable fish;
        private RodBehaviour rod;

        private float currentDistance;
        private float furthestDistance;
        private float rodReeledInDistance;
        private float hookIconTargetX;

        private RodManager rodManager;

        public static DistanceBar instance;

        private DistanceBar() => instance = this;

        private void Awake() => rodManager = RodManager.instance;

        private void Update()
        {
            currentDistance = Vector2.Distance(fish.transform.position, rod.GetHook().GetHookAnchorPoint().position) - rodReeledInDistance;
            if (currentDistance >= furthestDistance) furthestDistance = currentDistance;

            MoveIcon();

            distanceText.text = currentDistance.ToString("F2") + "m";
        }

        public void InitializeMinigame(Fishable _fish)
        {
            fish = _fish;
            rod = rodManager.equippedRod;

            rodReeledInDistance = rod.GetReeledInDistance();
            currentDistance = Vector2.Distance(fish.transform.position, rod.GetHook().GetHookAnchorPoint().position) - rodReeledInDistance;
            furthestDistance = currentDistance;

            hookIconTargetX = distanceBarMaxX;
            hookIcon.rectTransform.anchoredPosition = new Vector2(0f, 0f);
        }

        private float DistanceToBarPos()
        {
            float _distanceValue = Mathf.InverseLerp(0f, furthestDistance, currentDistance);
            float _barPos = Mathf.Lerp(0f, distanceBarMaxX, _distanceValue);
            return _barPos;
        }

        private void MoveIcon()
        {
            hookIconTargetX = DistanceToBarPos();
            float _newX = Mathf.Lerp(hookIcon.rectTransform.anchoredPosition.x, hookIconTargetX, smoothing);
            hookIcon.rectTransform.anchoredPosition = new Vector2(_newX, 0f);
        }
    }
}
