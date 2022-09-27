using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fishing.IO;
using UnityEngine.InputSystem;

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

        private RodManager rodManager;

        public static PowerAndAngle instance;

        private PowerAndAngle() => instance = this;

        private void Awake()
        {
            rodManager = RodManager.instance;
            Controls _controls = new Controls();
            _controls.FishingLevelInputs.Enable();
            _controls.FishingLevelInputs.SetPower.performed += StartAngling;
            _controls.FishingLevelInputs.Cast.performed += Cast;
        }

        private void Start()
        {
            transform.SetParent(UIManager.instance.transform);
        }

        void Update()
        {
            if (charging)
            {
                Charge(); 
            }
            else if (angling)
            {
                Angle();
            }
        }
        public void StartCharging(float _chargeFrequency, float _minStrength, float _maxStrength, float _maxAngle, float _angleFrequence)
        {
            transform.SetParent(UIManager.instance.rodCanvas.transform);

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

        private void StartAngling(InputAction.CallbackContext _context)
        {
            if (!_context.performed) return;
            if (!RodManager.instance.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Start Cast")) return;
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

        private void Cast(InputAction.CallbackContext _context)
        {
            if (!_context.performed) return;
            if (!RodManager.instance.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Start Cast")) return;
            AudioManager.instance.StopPlaying("Power Audio");
            angling = false;
            transform.SetParent(UIManager.instance.transform);
            rodManager.equippedRod.Cast(currentAngle, charge);
        }
    }

}