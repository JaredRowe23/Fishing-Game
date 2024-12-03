using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.FishingMechanics;
using Fishing.Fishables.Fish;
using Fishing.Util;
using System;

namespace Fishing.Fishables
{
    public class Fishable : MonoBehaviour
    {
        [Header("Stats")]
        #region
        [SerializeField] private string _itemName;
        public string ItemName { get => _itemName; private set { } }

        [SerializeField] private string _itemDescription;
        public string ItemDescription { get => _itemDescription; private set { } }

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

        [SerializeField] private float _baseValue;
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
        #endregion

        [Header("Variation")]
        #region
        [SerializeField] private float _weightMax;
        public float WeightMax { get => _weightMax; private set { } }

        [SerializeField] private float _weightMin;
        public float WeightMin { get => _weightMin; private set { } }

        [SerializeField] private float _lengthMax;
        public float LengthMax { get => _lengthMax; private set { } }

        [SerializeField] private float _lengthMin;
        public float LengthMin { get => _lengthMin; private set { } }
        #endregion

        [SerializeField] private GameObject _minimapIndicator;
        public GameObject MinimapIndicator { get => _minimapIndicator; private set { } }

        private bool _isHooked;
        public bool IsHooked { get => _isHooked; set => _isHooked = value; }

        private int[] _gridSquare = {0, 0};
        public int[] GridSquare { get => _gridSquare; set { _gridSquare = value; } }

        private int _range = 1;
        public int Range { get => _range; private set { _range = value; } }

        private RodManager _rodManager;
        private ISpawn _spawner;
        private AudioSource _audioSource;
        private FoodSearch _foodSearch;

        private void Awake() {
            _rodManager = RodManager.instance;
            _spawner = transform.parent.GetComponent<ISpawn>();
            _audioSource = GetComponent<AudioSource>();
            _foodSearch = GetComponent<FoodSearch>();
        }

        private void Start() {
            IsHooked = false;
            SetWeightAndLength();
            FishableGrid.instance.SortFishableIntoGridSquare(this);
        }

        private void SetWeightAndLength() {
            Weight = Mathf.Round(UnityEngine.Random.Range(WeightMin, WeightMax) * 100f) / 100f;
            Length = Mathf.Round(UnityEngine.Random.Range(LengthMin, LengthMax) * 100f) / 100f;
        }

        public void DisableMinimapIndicator() {
            MinimapIndicator.SetActive(false);
        }

        public void OnHooked(Transform _hook) {
            IsHooked = true;
            _spawner.RemoveFromSpawner(gameObject);
            transform.parent = _hook;
        }

        public void SetThisToHooked() {
            _audioSource.Play();
            if (_foodSearch) { 
                Destroy(_foodSearch.DesiredFood);
            }
            _rodManager.equippedRod.GetHook().hookedObject = null;
            _rodManager.equippedRod.GetHook().SetHook(this);
        }

        private void OnTriggerEnter2D(Collider2D _other) {
            if (!_other.GetComponent<HookBehaviour>()) return;
            _other.GetComponent<HookBehaviour>().SetHook(this);
        }

        private void OnDestroy() {
            FishableGrid.instance.RemoveFromGridSquares(this);
            _spawner.RemoveFromSpawner(gameObject);
        }
    }
}