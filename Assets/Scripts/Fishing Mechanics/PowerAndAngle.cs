using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.FishingMechanics
{
    public class PowerAndAngle : MonoBehaviour
    {
        [Header("Power Charge")]
        [SerializeField] private float chargeThreshold;
        [SerializeField] private Slider powerSlider;


        [Header("Angle Arrow")]
        [SerializeField] private float angleThreshold;
        [SerializeField] private RectTransform arrowRect;

        private bool charging;
        private float targetCharge;
        private float chargeFrequency;
        private float charge;
        private float minStrength;
        private float maxStrength;

        private bool angling;
        private float targetAngle;
        private float maxAngle;
        private float angleFrequence;
        private float currentAngle;

        public static PowerAndAngle instance;

        private void Awake() => instance = this;

        private void Start()
        {
            transform.SetParent(GameController.instance.transform);
        }

        void Update()
        {
            if (charging)
            {
                Charge();
                if (Input.GetMouseButtonUp(0))
                {
                    StartAngling();
                }    
            }
            else if (angling)
            {
                Angle();
                if (Input.GetMouseButtonDown(0))
                {
                    Cast();
                }
            }
        }
        public void StartCharging(float _chargeFrequency, float _minStrength, float _maxStrength, float _maxAngle, float _angleFrequence)
        {
            transform.SetParent(GameController.instance.rodCanvas.transform);

            AudioManager.instance.PlaySound("Power Audio");

            charge = 0f;
            minStrength = _minStrength;
            powerSlider.minValue = minStrength;

            maxStrength = _maxStrength;
            powerSlider.maxValue = maxStrength;

            chargeFrequency = _chargeFrequency;
            targetCharge = 1f;
            charging = true;

            currentAngle = 0f;
            maxAngle = _maxAngle;
            targetAngle = maxAngle;
            angleFrequence = _angleFrequence;

            arrowRect.rotation = Quaternion.identity;
            angling = false;
        }

        private void Charge()
        {
            // Mathy schenanigans because I don't feel like recreating code to see if charging up or down
            charge += ((targetCharge * 2) - 1) * Time.deltaTime / chargeFrequency;

            if (charge >= 1f - chargeThreshold && targetCharge * 2 - 1 > 0f)
            {
                charge = 1f;
                targetCharge = 0f;
            }
            else if (charge <= 0f + chargeThreshold && targetCharge * 2 - 1 < 0f)
            {
                charge = 0f;
                targetCharge = 1f;
            }
            powerSlider.value = Mathf.Lerp(minStrength, maxStrength, charge);
            AudioManager.instance.GetSource("Power Audio").pitch = charge;
        }

        private void StartAngling()
        {
            charge = Mathf.Lerp(powerSlider.minValue, powerSlider.maxValue, charge);
            angling = true;
            charging = false;
        }

        private void Angle()
        {
            currentAngle += (((targetAngle * 2 / maxAngle) - 1) * Time.deltaTime / angleFrequence) * maxAngle;

            if (currentAngle >= maxAngle - angleThreshold && (targetAngle * 2 / maxAngle) - 1 > 0f)
            {
                currentAngle = maxAngle;
                targetAngle = 0f;
            }
            else if (currentAngle <= 0f + angleThreshold && (targetAngle * 2 / maxAngle) - 1 < 0f)
            {
                currentAngle = 0f;
                targetAngle = maxAngle;
            }

            arrowRect.rotation = Quaternion.Euler(0f, 0f, currentAngle);
            AudioManager.instance.GetSource("Power Audio").pitch = Mathf.InverseLerp(0f, maxAngle, currentAngle) + AudioManager.instance.GetSound("Power Audio").pitch;
        }

        private void Cast()
        {
            AudioManager.instance.StopPlaying("Power Audio");
            angling = false;
            transform.SetParent(GameController.instance.transform);
            GameController.instance.equippedRod.Cast(currentAngle, charge);
        }
    }

}