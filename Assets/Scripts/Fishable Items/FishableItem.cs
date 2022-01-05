// This will hold and generate the data necessary
// for any objects we fish

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishableItem : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private string itemName;
    [SerializeField] private string itemDescription;
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

    private void Update()
    {
        if (isHooked || transform.parent == GameController.instance.bucket.transform)
        {
            minimapIndicator.SetActive(false);
        }
        else
        {
            minimapIndicator.SetActive(true);
        }
    }

    public string GetName()
    {
        return itemName;
    }

    public string GetDescription()
    {
        return itemDescription;
    }

    public float GetWeight()
    {
        return weight;
    }

    public float GetLength()
    {
        return length;
    }
}
