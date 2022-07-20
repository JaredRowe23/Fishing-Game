using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.Fishables
{
    public class Fishable : MonoBehaviour
    {
        [Header("Stats")]
        [SerializeField] private string itemName;
        [SerializeField] private string itemDescription;
        private float weight;
        private float length;
        [SerializeField] private int baseValue;
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

        public float GetWeight() => weight;

        public float GetLength() => length;

        public float GetValue() => actualValue;

        public void DisableMinimapIndicator() => minimapIndicator.SetActive(false);

        public void OnHooked(Transform _hook)
        {
            isHooked = true;
            transform.parent.GetComponent<SpawnZone>().spawnList.Remove(gameObject);
            transform.parent = _hook;
        }
    }

}