using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing
{
    public class PowerSlider : MonoBehaviour
    {
        private bool isCharging;
        private float target;
        private float chargeFrequency;
        [SerializeField] private float threshold;
        private Slider slider;
        private RodBehaviour castingRod;

        public float charge;

        public static PowerSlider instance;

        private void Awake() => instance = this;

        private void Start()
        {
            slider = this.GetComponent<Slider>();
            transform.SetParent(GameController.instance.transform);
        }

        void Update()
        {
            if (!isCharging) return;
            // Mathy schenanigans because I don't feel like recreating code to see if charging up or down
            charge += ((target * 2) - 1) * Time.deltaTime / chargeFrequency;

            if (charge >= 1f - threshold && target * 2 - 1 > 0f)
            {
                charge = 1f;
                target = 0f;
            }
            else if (charge <= 0f + threshold && target * 2 - 1 < 0f)
            {
                charge = 0f;
                target = 1f;
            }

            slider.value = charge;
            AudioManager.instance.GetSource("Power Audio").pitch = charge + AudioManager.instance.GetSound("Power Audio").pitch;

            if (Input.GetMouseButtonUp(0))
            {
                castingRod.castStrength = Mathf.Lerp(castingRod.GetMinStrength(), castingRod.GetMaxStrength(), charge);
                isCharging = false;
                AngleArrow.instance.transform.SetParent(GameController.instance.rodCanvas.transform);
                AngleArrow.instance.StartAngling(castingRod.GetMaxAngle(), castingRod.GetAngleFrequency(), castingRod);
            }
        }

        public void StartCharging(float frequency, RodBehaviour rod)
        {
            AudioManager.instance.PlaySound("Power Audio");
            charge = 0f;
            slider.value = 0f;
            castingRod = rod;
            chargeFrequency = frequency;
            target = 1f;
            isCharging = true;
        }
    }

}