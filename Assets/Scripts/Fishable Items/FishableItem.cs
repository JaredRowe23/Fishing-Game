using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishableItem : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private string itemName;
    [SerializeField] private string itemDescription;
    public enum FoodTypes { Fish1, SinkingTrash, Fish3, Fish4, Fish5, Fish6, Fish7, Fish8, Fish9, Fish10, Fish11, Fish12 };
    [SerializeField] private FoodTypes foodType;
    private float weight;
    private float length;

    [Header("Variation")]
    [SerializeField] private float weightMax;
    [SerializeField] private float weightMin;
    [SerializeField] private float lengthMax;
    [SerializeField] private float lengthMin;

    private GameObject minimapIndicator;

    public bool isHooked;

    private void Start()
    {
        isHooked = false;
        weight = Mathf.Round(Random.Range(weightMin, weightMax) * 100f) / 100f;
        length = Mathf.Round(Random.Range(lengthMin, lengthMax) * 100f) / 100f;
        Transform parent = transform.parent;
        transform.parent = null;
        transform.localScale = Vector3.one * length / 100f;
        transform.parent = parent;
        foreach (Transform child in transform)
        {
            if (child.name == "Cube")
            {
                minimapIndicator = child.gameObject;
            }
        }
    }

    public string GetName() => itemName;

    public string GetDescription() => itemDescription;

    public float GetWeight() => weight;

    public float GetLength() => length;

    public int GetFoodType() => (int)foodType;

    public void DisableMinimapIndicator()
    {
        minimapIndicator.SetActive(false);
    }
}
