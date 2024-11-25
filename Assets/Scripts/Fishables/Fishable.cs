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
        [SerializeField] private string itemName;
        [SerializeField] private string itemDescription;
        private float weight;
        private float length;
        [SerializeField] private float baseValue;
        private float actualValue;

        [Header("Minigame")]
        [SerializeField] private float minigameStrength;
        [SerializeField] private float minigameMoveDistance;
        [SerializeField] private float minigameMoveDistanceVariance;
        [SerializeField] private float minigameMoveTime;
        [SerializeField] private float minigameMoveTimeVariance;
        [SerializeField] private float minigameSwimSpeed;
        [SerializeField] private float minigameSwimTime;
        [SerializeField] private float minigameSwimTimeVariance;
        [SerializeField] private float minigameRestTime;
        [SerializeField] private float minigameRestTimeVariance;

        private float minigameDifficulty;

        [Header("Variation")]
        [SerializeField] private float weightMax;
        [SerializeField] private float weightMin;
        [SerializeField] private float lengthMax;
        [SerializeField] private float lengthMin;

        public GameObject minimapIndicator;

        public bool isHooked;

        private int[] _gridSquare;
        public int[] GridSquare { get => _gridSquare; set { _gridSquare = value; } }
        private int _range = 1;
        public int Range { get => _range; private set { _range = value; } }
        private List<Fishable> _fishWithinRange;
        public List<Fishable> FishWithinRange {
            get {
                _fishWithinRange.RemoveAll(item => item == null); // TODO: Remove after source of null references is resolved
                return _fishWithinRange;
            }
            private set {
                value.RemoveAll(item => item == null);
                _fishWithinRange = value; 
            } 
        }

        private RodManager rodManager;

        private void Awake() => rodManager = RodManager.instance;

        private void Start()
        {
            isHooked = false;
            GridSquare = new int[] { 0, 0 };
            FishWithinRange = new List<Fishable>();
            SetWeightAndLength();
            AdjustValueAndDifficulty();
        }

        private void Update() {
            FishableGrid.instance.RemoveFromGridSquares(this);
            FishableGrid.instance.SortFishableIntoGridSquare(this);
            FishWithinRange = FishableGrid.instance.GetFishablesWithinRange(GridSquare[0], GridSquare[1], Range);
        }

        private void SetWeightAndLength()
        {
            weight = Mathf.Round(UnityEngine.Random.Range(weightMin, weightMax) * 100f) / 100f;
            length = Mathf.Round(UnityEngine.Random.Range(lengthMin, lengthMax) * 100f) / 100f;
            transform.localScale = Utilities.SetGlobalScale(transform, length / 100f);
        }

        public void AdjustValueAndDifficulty()
        {
            float _weightValueDelta = Mathf.InverseLerp(weightMin, weightMax, weight) + 0.5f;
            float _lengthValueDelta = Mathf.InverseLerp(lengthMin, lengthMax, length) + 0.5f;
            float _valueDelta = (_weightValueDelta + _lengthValueDelta) * 0.5f;
            actualValue = baseValue * _valueDelta;
            minigameDifficulty = _valueDelta;
        }

        public void DisableMinimapIndicator() => minimapIndicator.SetActive(false);

        public void OnHooked(Transform _hook)
        {
            isHooked = true;
            FoodSearchManager.instance.RemoveFood(GetComponent<Edible>());
            if (transform.parent.GetComponent<SpawnZone>()) transform.parent.GetComponent<SpawnZone>().spawnList.Remove(gameObject);
            else if (transform.parent.GetComponent<PlantStalk>()) transform.parent.GetComponent<PlantStalk>().RemoveFruit(gameObject);
            transform.parent = _hook;
        }
        public void SetThisToHooked()
        {
            GetComponent<AudioSource>().Play();
            if (GetComponent<FoodSearch>()) GetComponent<FoodSearch>().desiredFood.GetComponent<IEdible>().Despawn();
            rodManager.equippedRod.GetHook().hookedObject = null;
            rodManager.equippedRod.GetHook().SetHook(this);
        }

        private void OnTriggerEnter2D(Collider2D _other)
        {
            if (!_other.GetComponent<HookBehaviour>()) return;
            _other.GetComponent<HookBehaviour>().SetHook(this);
        }

        public string GetName() => itemName;
        public string GetDescription() => itemDescription;
        public float GetWeight() => weight;
        public void SetWeight(float _weight) => weight = _weight;
        public float GetLength() => length;
        public void SetLength(float _length)
        {
            length = _length;
            Transform parent = transform.parent;
            transform.parent = null;
            transform.localScale = Vector2.one * length / 100f;
            transform.parent = parent;
        }
        public float GetValue() => actualValue;

        public float GetMinWeight() => weightMin;
        public float GetMaxWeight() => weightMax;
        public float GetMinLength() => lengthMin;
        public float GetMaxLength() => lengthMax;

        public float GetMinigameDifficulty() => minigameDifficulty;
        public float GetMinigameStrength() => minigameStrength;
        public float GetMinigameMoveDistance() => minigameMoveDistance;
        public float GetMinigameMoveDistanceVariance() => minigameMoveDistanceVariance;
        public float GetMinigameMoveTime() => minigameMoveTime;
        public float GetMinigameMoveTimeVariance() => minigameMoveTimeVariance;
        public float GetMinigameSwimSpeed() => minigameSwimSpeed;
        public float GetMinigameSwimTime() => minigameSwimTime;
        public float GetMinigameSwimTimeVariance() => minigameSwimTimeVariance;
        public float GetMinigameRestTime() => minigameRestTime;
        public float GetMinigameRestTimeVariance() => minigameRestTimeVariance;

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < FishWithinRange.Count; i++) {
                Gizmos.DrawLine(transform.position, FishWithinRange[i].transform.position);
            }
        }

        private void OnDestroy() {
            FishableGrid.instance.RemoveFromGridSquares(GetComponent<Fishable>());
        }
    }
}