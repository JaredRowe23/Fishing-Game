using Fishing.IO;
using Fishing.PlayerInput;
using Fishing.UI;
using Fishing.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.FishingMechanics {
    public class PowerAndAngle : MonoBehaviour {
        private bool _isCharging;
        public bool IsCharging { get => _isCharging; private set => _isCharging = value; }

        private float _targetCharge;
        private float _chargeFrequency;
        private float _power;
        public float Power { get => _power; private set => _power = value; }
        private float _minStrength;
        private float _maxStrength;

        private bool _isAngling;
        public bool IsAngling { get => _isAngling; private set => _isAngling = value; }

        private float _targetAngle;
        private float _minAngle = 0;
        private float _maxAngle;
        private float _angleFrequency;
        private float _currentAngle;
        public float CurrentAngle { get => _currentAngle; private set => _currentAngle = value; }

        private RodManager _rodManager;
        private RodBehaviour _equippedRod;

        private static PowerAndAngle _instance;
        public static PowerAndAngle Instance { get => _instance; set => _instance = value; }

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            _rodManager = RodManager.Instance;
        }

        void Update() {
            if (IsAngling) {
                if (!_rodManager.EquippedRod.Hook.IsInStartCastPosition()) {
                    return;
                }

                Angle();
            }

            else if (IsCharging) {
                Charge();
            }
        }

        public void StartCharging() {
            _minStrength = _equippedRod.Scriptable.minCastStrength;
            _maxStrength = _equippedRod.Scriptable.maxCastStrength;
            _chargeFrequency = _equippedRod.Scriptable.chargeFrequency;

            Power = _minStrength;
            _targetCharge = _maxStrength;

            InputManager.onCastReel -= StartCharging;
            InputManager.onCastReel += Cast;

            IsCharging = true;
            IsAngling = false;

            if (SaveManager.Instance.LoadedPlayerData.HasSeenTutorialData.CastTutorial) {
                return;
            }

            TutorialSystem.instance.QueueTutorial("Release the left mouse button to set your power", true, 3f);
        }

        private void Charge() {
            OscillateInfo _oscillateInfo = Utilities.OscillateFloat(_minStrength, _maxStrength, Power, _chargeFrequency * Time.deltaTime / 1, _targetCharge);
            Power = _oscillateInfo.value;
            _targetCharge = _oscillateInfo.newTarget;

            AudioManager.instance.GetSource("Power Audio").pitch = Mathf.InverseLerp(_minStrength, _maxStrength, Power);
        }

        public void StartAngling() {
            _equippedRod = _rodManager.EquippedRod;
            _maxAngle = _equippedRod.Scriptable.maxCastAngle;
            _angleFrequency = _equippedRod.Scriptable.angleFrequency;

            CurrentAngle = 0f;
            _targetAngle = _maxAngle;

            InputManager.onCastReel += StartCharging;
            AudioManager.instance.PlaySound("Power Audio");

            IsAngling = true;
            IsCharging = false;

            if (SaveManager.Instance.LoadedPlayerData.HasSeenTutorialData.CastTutorial) {
                return;
            }

            TutorialSystem.instance.QueueTutorial("Click the left mouse button once more to set your angle and cast.", true, 3f);
            SaveManager.Instance.LoadedPlayerData.HasSeenTutorialData.CastTutorial = true;
        }

        private void Angle() {
            OscillateInfo _oscillateInfo = Utilities.OscillateFloat(_minAngle, _maxAngle, CurrentAngle, _angleFrequency * Time.deltaTime / 1, _targetAngle);
            CurrentAngle = _oscillateInfo.value;
            _targetAngle = _oscillateInfo.newTarget;

            AudioManager.instance.GetSource("Power Audio").pitch = Mathf.InverseLerp(_minAngle, _maxAngle, CurrentAngle) + AudioManager.instance.GetSound("Power Audio").pitch;
        }


        private void Cast() {
            if (!RodManager.Instance.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Start Cast")) { // TODO: Switch away from relying on animator state info to determine state
                return;
            }

            AudioManager.instance.StopPlaying("Power Audio");
            IsAngling = IsCharging = false;
            _rodManager.EquippedRod.Cast(CurrentAngle, Power);
            InputManager.onCastReel -= Cast;
        }
    }

}