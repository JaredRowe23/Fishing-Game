using Fishing.FishingMechanics;
using Fishing.Util;
using UnityEngine;
using Fishing.Fishables.FishGrid;
using Fishing.FishingMechanics.Minigame;
using System;

namespace Fishing.Fishables {
    public class Fishable : MonoBehaviour {
        [Flags] public enum ItemTypes {
            Anglerfish = 1,
            Boot = 2,
            Carp = 4,
            Crab = 8,
            Driftwood = 16,
            EarthWorm = 32,
            Hook = 64,
            Minnow = 128,
            Salmon = 256,
            SeaSerpent = 512,
            Seaweed = 1024,
            TinCan = 2048,
            WaterLilyFruit = 4096
        };
        [SerializeField, Tooltip("The type of fishable this is.")] private ItemTypes _fishableType;
        public ItemTypes FishableType { get => _fishableType; private set { } }

        [Header("Descriptors")]
        [SerializeField, Tooltip("Name of this item.")] private string _itemName;
        public string ItemName { get => _itemName; private set { } }

        [SerializeField, Tooltip("Description of this item.")] private string _itemDescription;
        public string ItemDescription { get => _itemDescription; private set { } }

        [Header("Stats")]
        [SerializeField, Tooltip("Base value for a \"normal\" sized item of this type.")] private float _baseValue;
        public float BaseValue { get => _baseValue; private set { } }

        private float _value;
        public float Value {
            get {
                float _weightValueDelta = Mathf.InverseLerp(WeightMin, WeightMax, Weight) + 0.5f;
                float _lengthValueDelta = Mathf.InverseLerp(LengthMin, LengthMax, Length) + 0.5f;
                float _valueDelta = (_weightValueDelta + _lengthValueDelta) * 0.5f;
                return BaseValue * _valueDelta;
            }
            private set { } }

        [SerializeField, Tooltip("Maximum weight this item can reach.")] private float _weightMax;
        public float WeightMax { get => _weightMax; private set { } }

        [SerializeField, Tooltip("Minimum weight this item can reach.")] private float _weightMin;
        public float WeightMin { get => _weightMin; private set { } }

        private float _weight;
        public float Weight { get => _weight; set { _weight = value; } }

        [SerializeField, Tooltip("Maximum length this item can reach.")] private float _lengthMax;
        public float LengthMax { get => _lengthMax; private set { } }

        [SerializeField, Tooltip("Minimum length this item can reach.")] private float _lengthMin;
        public float LengthMin { get => _lengthMin; private set { } }

        private float _length;
        public float Length {
            get => _length;
            set {
                _length = value;
                Utilities.SetGlobalScale(transform, value);
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

        private void OnValidate() {
            if (WeightMin > WeightMax) {
                WeightMin = WeightMax;
            }
            if (LengthMin > LengthMax) {
                LengthMin = LengthMax;
            }
        }

        private void Awake() {
            _rodManager = RodManager.Instance;
            _spawner = transform.parent.GetComponent<ISpawn>();
            _minimapIndicator = GetComponentInChildren<RadarScanObject>();
            _fishableGrid = FishableGrid.instance;
        }

        private void Start() {
            IsHooked = false;
            SetWeightAndLength();
            _fishableGrid.SortFishableIntoGridSquare(this);
        }

        private void SetWeightAndLength() {
            Weight = Mathf.Round(UnityEngine.Random.Range(WeightMin, WeightMax) * 100f) / 100f;
            Length = Mathf.Round(UnityEngine.Random.Range(LengthMin, LengthMax) * 100f) / 100f;
        }

        public void DisableMinimapIndicator() {
            MinimapIndicator.gameObject.SetActive(false);
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