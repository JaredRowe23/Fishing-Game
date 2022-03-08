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
        [SerializeField] private Slider _powerSlider;


        [Header("Angle Arrow")]
        [SerializeField] private float angleThreshold;
        [SerializeField] private RectTransform _arrowRect;

        private bool _charging;
        private float _targetCharge;
        private float _chargeFrequency;
        private float _charge;
        private float _minStrength;
        private float _maxStrength;

        private bool _angling;
        private float _targetAngle;
        private float _maxAngle;
        private float _angleFrequency;
        private float currentAngle;

        public static PowerAndAngle instance;

        private void Awake() => instance = this;

        private void Start()
        {
            transform.SetParent(GameController.instance.transform);
        }

        void Update()
        {
            if (_charging)
            {
                Charge();
                if (Input.GetMouseButtonUp(0))
                {
                    StartAngling();
                }    
            }
            else if (_angling)
            {
                Angle();
                if (Input.GetMouseButtonDown(0))
                {
                    Cast();
                }
            }
        }
        public void StartCharging(float __chargeFrequency, float __minStrength, float __maxStrength, float __maxAngle, float __angleFrequency)
        {
            transform.SetParent(GameController.instance.rodCanvas.transform);

            AudioManager.instance.PlaySound("Power Audio");

            _charge = 0f;
            _minStrength = __minStrength;
            _powerSlider.minValue = _minStrength;

            _maxStrength = __maxStrength;
            _powerSlider.maxValue = _maxStrength;

            _chargeFrequency = __chargeFrequency;
            _targetCharge = 1f;
            _charging = true;

            currentAngle = 0f;
            _maxAngle = __maxAngle;
            _targetAngle = _maxAngle;
            _angleFrequency = __angleFrequency;

            _arrowRect.rotation = Quaternion.identity;
            _angling = false;
        }

        private void Charge()
        {
            // Mathy schenanigans because I don't feel like recreating code to see if charging up or down
            _charge += ((_targetCharge * 2) - 1) * Time.deltaTime / _chargeFrequency;

            if (_charge >= 1f - chargeThreshold && _targetCharge * 2 - 1 > 0f)
            {
                _charge = 1f;
                _targetCharge = 0f;
            }
            else if (_charge <= 0f + chargeThreshold && _targetCharge * 2 - 1 < 0f)
            {
                _charge = 0f;
                _targetCharge = 1f;
            }
            _powerSlider.value = Mathf.Lerp(_minStrength, _maxStrength, _charge);
            AudioManager.instance.GetSource("Power Audio").pitch = _charge;
        }

        private void StartAngling()
        {
            _charge = Mathf.Lerp(_powerSlider.minValue, _powerSlider.maxValue, _charge);
            _angling = true;
            _charging = false;
        }

        private void Angle()
        {
            currentAngle += (((_targetAngle * 2 / _maxAngle) - 1) * Time.deltaTime / _angleFrequency) * _maxAngle;

            if (currentAngle >= _maxAngle - angleThreshold && (_targetAngle * 2 / _maxAngle) - 1 > 0f)
            {
                currentAngle = _maxAngle;
                _targetAngle = 0f;
            }
            else if (currentAngle <= 0f + angleThreshold && (_targetAngle * 2 / _maxAngle) - 1 < 0f)
            {
                currentAngle = 0f;
                _targetAngle = _maxAngle;
            }

            _arrowRect.rotation = Quaternion.Euler(0f, 0f, currentAngle);
            AudioManager.instance.GetSource("Power Audio").pitch = Mathf.InverseLerp(0f, _maxAngle, currentAngle) + AudioManager.instance.GetSound("Power Audio").pitch;
        }

        private void Cast()
        {
            AudioManager.instance.StopPlaying("Power Audio");
            _angling = false;
            transform.SetParent(GameController.instance.transform);
            GameController.instance.equippedRod.Cast(currentAngle, _charge);
        }
    }

}