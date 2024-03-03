using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fishing.IO;
using Fishing.UI;

namespace Fishing.FishingMechanics
{
    public class PowerAndAngle : MonoBehaviour
    {
        [Range(0f, 1.0f)]
        [SerializeField] private float lockedInTransparency;

        [Header("Power Charge")]
        [SerializeField] private float chargeThreshold;
        [SerializeField] private Slider powerSlider;
        [SerializeField] private Image powerImage;
        [SerializeField] private Text powerText;


        [Header("Angle Arrow")]
        [SerializeField] private float angleThreshold;
        [SerializeField] private RectTransform arrowRect;
        [SerializeField] private Image arrowImage;
        [SerializeField] private Text arrowText;

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
        }

        private void Start()
        {
            transform.SetParent(UIManager.instance.transform);
        }

        void Update()
        {
            if (angling)
            {
                if (rodManager.equippedRod.IsInStartingCastPosition() && rodManager.equippedRod.GetHook().IsInStartCastPosition())
                {
                    transform.SetParent(UIManager.instance.rodCanvas.transform);
                    Angle();
                }
            }
            else if (charging)
            {
                Charge();
            }
        }
        public void StartCharging()
        {
            InputManager.onCastReel -= StartCharging;
            InputManager.onCastReel += Cast;

            RodBehaviour equippedRod = rodManager.equippedRod;

            charge = 0f;
            minStrength = equippedRod.scriptable.minCastStrength;
            powerSlider.minValue = minStrength;

            maxStrength = equippedRod.scriptable.maxCastStrength;
            powerSlider.maxValue = maxStrength;

            chargeFrequency = equippedRod.scriptable.chargeFrequency;
            targetCharge = 1f;
            charging = true;
            angling = false;

            powerImage.color = SetTransparency(powerImage.color, 1f);
            powerText.color = SetTransparency(powerText.color, 1f);
            arrowImage.color = SetTransparency(arrowImage.color, lockedInTransparency);
            arrowText.color = SetTransparency(arrowText.color, lockedInTransparency);

            if (PlayerData.instance.hasSeenCastTut) return;
            TutorialSystem.instance.QueueTutorial("Release the left mouse button to set your power", true, 3f);
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

        public void StartAngling()
        {
            InputManager.onCastReel += StartCharging;

            AudioManager.instance.PlaySound("Power Audio");

            //if (!RodManager.instance.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Start Cast")) return;
            charge = Mathf.Lerp(powerSlider.minValue, powerSlider.maxValue, charge);

            RodBehaviour equippedRod = rodManager.equippedRod;

            currentAngle = 0f;
            maxAngle = equippedRod.scriptable.maxCastAngle;
            targetAngle = maxAngle;
            angleFrequence = equippedRod.scriptable.angleFrequency;

            arrowRect.rotation = Quaternion.identity;

            arrowImage.color = SetTransparency(arrowImage.color, 1f);
            arrowText.color = SetTransparency(arrowText.color, 1f);
            powerImage.color = SetTransparency(powerImage.color, lockedInTransparency);
            powerText.color = SetTransparency(powerText.color, lockedInTransparency);

            angling = true;
            charging = false;

            if (PlayerData.instance.hasSeenCastTut) return;
            TutorialSystem.instance.QueueTutorial("Click the left mouse button once more to set your angle and cast.", true, 3f);
            PlayerData.instance.hasSeenCastTut = true;
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
            if (!RodManager.instance.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Start Cast")) return;
            AudioManager.instance.StopPlaying("Power Audio");
            angling = charging = false;
            transform.SetParent(UIManager.instance.transform);
            rodManager.equippedRod.Cast(currentAngle, Mathf.Lerp(rodManager.equippedRod.scriptable.minCastStrength, rodManager.equippedRod.scriptable.maxCastStrength, charge));
            InputManager.onCastReel -= Cast;
        }

        private Color SetTransparency(Color _col, float _transparency)
        {
            return new Color(_col.r, _col.g, _col.b, _transparency);
        }

        public bool GetCharging() => charging;
        public bool GetAngling() => angling;
        public float GetCharge() => charge;
        public float GetAngle() => currentAngle;
    }

}