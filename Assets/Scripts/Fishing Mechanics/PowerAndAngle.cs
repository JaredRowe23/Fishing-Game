using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fishing.IO;
using Fishing.UI;
using Fishing.Util;

namespace Fishing.FishingMechanics
{
    public class PowerAndAngle : MonoBehaviour
    {
        [Range(0f, 1.0f)]
        [SerializeField] private float inactiveTransparency;

        [Header("Power Charge")]
        [SerializeField] private Slider powerSlider;
        [SerializeField] private Image powerImage;
        [SerializeField] private Text powerText;


        [Header("Angle Arrow")]
        [SerializeField] private RectTransform arrowRect;
        [SerializeField] private Image arrowImage;
        [SerializeField] private Text arrowText;

        private bool isCharging;
        private float targetCharge;
        private float chargeFrequency;
        private float charge;
        private float minStrength;
        private float maxStrength;

        private bool isAngling;
        private float targetAngle;
        private float minAngle = 0;
        private float maxAngle;
        private float angleFrequency;
        private float currentAngle;

        private RodManager rodManager;
        private RodBehaviour equippedRod;

        public static PowerAndAngle instance;

        private PowerAndAngle() => instance = this;

        private void Awake()
        {
            rodManager = RodManager.instance;
        }

        private void Start()
        {
            transform.SetParent(UIManager.instance.transform);
        }

        void Update()
        {
            if (isAngling)
            {
                if (!rodManager.equippedRod.GetHook().IsInStartCastPosition()) return;

                transform.SetParent(UIManager.instance.rodCanvas.transform);
                Angle();
            }
            else if (isCharging) Charge();
        }
        public void StartCharging()
        {
            minStrength = equippedRod.scriptable.minCastStrength;
            maxStrength = equippedRod.scriptable.maxCastStrength;
            chargeFrequency = equippedRod.scriptable.chargeFrequency;

            charge = minStrength;
            targetCharge = maxStrength;

            powerImage.color = Utilities.SetTransparency(powerImage.color, 1f);
            powerText.color = Utilities.SetTransparency(powerText.color, 1f);
            arrowImage.color = Utilities.SetTransparency(arrowImage.color, inactiveTransparency);
            arrowText.color = Utilities.SetTransparency(arrowText.color, inactiveTransparency);

            InputManager.onCastReel -= StartCharging;
            InputManager.onCastReel += Cast;

            isCharging = true;
            isAngling = false;

            if (PlayerData.instance.hasSeenTutorialData.castTutorial) return;
            TutorialSystem.instance.QueueTutorial("Release the left mouse button to set your power", true, 3f);
        }

        private void Charge()
        {
            OscillateInfo _oscillateInfo = Utilities.OscillateFloat(minStrength, maxStrength, charge, chargeFrequency * Time.deltaTime / 1, targetCharge);
            charge = _oscillateInfo.value;
            targetCharge = _oscillateInfo.newTarget;

            powerSlider.value = Mathf.InverseLerp(minStrength, maxStrength, charge);
            AudioManager.instance.GetSource("Power Audio").pitch = Mathf.InverseLerp(minStrength, maxStrength, charge);
        }

        public void StartAngling()
        {
            equippedRod = rodManager.equippedRod;
            maxAngle = equippedRod.scriptable.maxCastAngle;
            angleFrequency = equippedRod.scriptable.angleFrequency;

            currentAngle = 0f;
            targetAngle = maxAngle;

            arrowRect.rotation = Quaternion.identity;

            arrowImage.color = Utilities.SetTransparency(arrowImage.color, 1f);
            arrowText.color = Utilities.SetTransparency(arrowText.color, 1f);
            powerImage.color = Utilities.SetTransparency(powerImage.color, inactiveTransparency);
            powerText.color = Utilities.SetTransparency(powerText.color, inactiveTransparency);

            InputManager.onCastReel += StartCharging;
            AudioManager.instance.PlaySound("Power Audio");

            isAngling = true;
            isCharging = false;

            if (PlayerData.instance.hasSeenTutorialData.castTutorial) return;
            TutorialSystem.instance.QueueTutorial("Click the left mouse button once more to set your angle and cast.", true, 3f);
            PlayerData.instance.hasSeenTutorialData.castTutorial = true;
        }

        private void Angle()
        {
            OscillateInfo _oscillateInfo = Utilities.OscillateFloat(minAngle, maxAngle, currentAngle, angleFrequency * Time.deltaTime / 1, targetAngle);
            currentAngle = _oscillateInfo.value;
            targetAngle = _oscillateInfo.newTarget;

            arrowRect.rotation = Quaternion.Euler(0f, 0f, currentAngle);
            AudioManager.instance.GetSource("Power Audio").pitch = Mathf.InverseLerp(minAngle, maxAngle, currentAngle) + AudioManager.instance.GetSound("Power Audio").pitch;
        }


        private void Cast()
        {
            if (!RodManager.instance.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Start Cast")) return;
            AudioManager.instance.StopPlaying("Power Audio");
            isAngling = isCharging = false;
            transform.SetParent(UIManager.instance.transform);
            rodManager.equippedRod.Cast(currentAngle, charge);
            InputManager.onCastReel -= Cast;
        }

        public bool IsCharging() => isCharging;
        public bool IsAngling() => isAngling;
        public float GetCharge() => charge;
        public float GetAngle() => currentAngle;
    }

}