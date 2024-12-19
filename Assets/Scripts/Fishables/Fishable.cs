using Fishing.Fishables.Fish;
using Fishing.Fishables.FishGrid;
using Fishing.FishingMechanics;
using Fishing.FishingMechanics.Minigame;
using Fishing.Util;
using UnityEngine;

namespace Fishing.Fishables {
    public class Fishable : MonoBehaviour {
        [SerializeField, Tooltip("Scriptable object that holds the stats for this type of fishable item.")] private FishableScriptable _fishableScriptable;
        public FishableScriptable FishableScriptable { get => _fishableScriptable; private set { _fishableScriptable = value; } }

        private float _value = -1; // Set to -1 to determine if this wasn't already set.
        public float Value {
            get {
                if (_value == -1) {
                    float _weightValueDelta = Mathf.InverseLerp(FishableScriptable.WeightMin, FishableScriptable.WeightMax, Weight) + 0.5f;
                    float _lengthValueDelta = Mathf.InverseLerp(FishableScriptable.LengthMin, FishableScriptable.LengthMax, Length) + 0.5f;
                    float _valueDelta = (_weightValueDelta + _lengthValueDelta) * 0.5f;
                    _value = FishableScriptable.BaseValue * _valueDelta;
                }

                return _value;
            }
            private set { _value = value; }
        }

        private float _weight;
        public float Weight { get => _weight; set { _weight = value; } }

        private float _length;
        public float Length {
            get => _length;
            set {
                _length = value;
                Utilities.SetGlobalScale(transform, value);
            }
        }

        private float _difficulty;
        public float Difficulty {
            get {
                return Value / _fishableScriptable.BaseValue;
            }
        }

        private RadarScanObject _minimapIndicator;
        public RadarScanObject MinimapIndicator { get => _minimapIndicator; private set { } }

        private bool _isHooked;
        public bool IsHooked { get => _isHooked; set => _isHooked = value; }

        private int[] _gridSquare = {0, 0};
        public int[] GridSquare { get => _gridSquare; set { _gridSquare = value; } }

        private RodManager _rodManager;
        private ISpawn _spawner;
        private FishableGrid _fishableGrid;

        private void Awake() {
            _rodManager = RodManager.Instance;
            _spawner = transform.parent.GetComponent<ISpawn>();
            _minimapIndicator = GetComponentInChildren<RadarScanObject>();
            _fishableGrid = FishableGrid.instance;
        }

        private void Start() {
            IsHooked = false;
            Weight = FishableScriptable.GetRandomWeightValue();
            Length = FishableScriptable.GetRandomLengthValue();
            _fishableGrid.SortFishableIntoGridSquare(this);
        }

        public void OnHooked() {
            IsHooked = true;
            _spawner.RemoveFromSpawner(gameObject);
            transform.parent = _rodManager.EquippedRod.Hook.transform;
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.TryGetComponent(out HookBehaviour hook)) {
                if (hook.HookedObject == null) {
                    hook.SetHook(this);
                    ReelingMinigame.Instance.InitiateMinigame(this);
                }
            }
        }

        private void OnDestroy() {
            _fishableGrid.RemoveFromGridSquares(this, _gridSquare[0], _gridSquare[1]);
            _spawner.RemoveFromSpawner(gameObject);
            if (_rodManager.EquippedRod.Hook.HookedObject == gameObject) {
                _rodManager.EquippedRod.Hook.HookedObject = null;
            }
        }
    }
}