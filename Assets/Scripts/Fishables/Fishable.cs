using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.FishingMechanics;

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

        [Header("Variation")]
        [SerializeField] private float weightMax;
        [SerializeField] private float weightMin;
        [SerializeField] private float lengthMax;
        [SerializeField] private float lengthMin;

        [SerializeField] private GameObject minimapIndicator;

        public bool isHooked;

        private void Start()
        {
            isHooked = false;
            weight = Mathf.Round(Random.Range(weightMin, weightMax) * 100f) / 100f;
            length = Mathf.Round(Random.Range(lengthMin, lengthMax) * 100f) / 100f;
            Transform parent = transform.parent;
            transform.parent = null;
            transform.localScale = Vector2.one * length / 100f;
            transform.parent = parent;

            float _weightValueDelta = Mathf.InverseLerp(weightMin, weightMax, weight) + 0.5f;
            float _lengthValueDelta = Mathf.InverseLerp(lengthMin, lengthMax, length) + 0.5f;
            float _valueDelta = (_weightValueDelta + _lengthValueDelta) * 0.5f;
            actualValue = baseValue * _valueDelta;
        }

        public string GetName() => itemName;

        public string GetDescription() => itemDescription;

        public void SetWeight(float _weight) => weight = _weight;
        public float GetWeight() => weight;
        public float GetMinWeight() => weightMin;
        public float GetMaxWeight() => weightMax;

        public void SetLength(float _length)
        {
            length = _length;
            Transform parent = transform.parent;
            transform.parent = null;
            transform.localScale = Vector2.one * length / 100f;
            transform.parent = parent;
        }
        public float GetLength() => length;
        public float GetMinLength() => lengthMin;
        public float GetMaxLength() => lengthMax;

        public float GetValue() => actualValue;
        public void RecalculateValue()
        {
            float _weightValueDelta = Mathf.InverseLerp(weightMin, weightMax, weight) + 0.5f;
            float _lengthValueDelta = Mathf.InverseLerp(lengthMin, lengthMax, length) + 0.5f;
            float _valueDelta = (_weightValueDelta + _lengthValueDelta) * 0.5f;
            actualValue = baseValue * _valueDelta;
        }

        public void DisableMinimapIndicator() => minimapIndicator.SetActive(false);

        public void OnHooked(Transform _hook)
        {
            isHooked = true;
            if (transform.parent.GetComponent<SpawnZone>()) transform.parent.GetComponent<SpawnZone>().spawnList.Remove(gameObject);
            else if (transform.parent.GetComponent<PlantStalk>()) transform.parent.GetComponent<PlantStalk>().RemoveFruit(gameObject);
            transform.parent = _hook;
        }

        private void OnTriggerEnter2D(Collider2D _other)
        {
            if (!_other.GetComponent<HookBehaviour>()) return;
            _other.GetComponent<HookBehaviour>().SetHook(this);
        }
    }

}