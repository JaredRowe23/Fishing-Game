using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BucketMenuItem : MonoBehaviour
{
    [SerializeField] private Text itemName;
    [SerializeField] private Text itemWeight;
    [SerializeField] private Text itemLength;
    private FishData itemReference;

    public void UpdateName(string name) => itemName.text = name;

    public void UpdateWeight(float weight) => itemWeight.text = weight.ToString() + " kg";

    public void UpdateLength(float length) => itemLength.text = length.ToString() + " cm";

    public void UpdateReference(FishData reference) => itemReference = reference;

    public void OpenInfoMenu()
    {
        if (!GameController.instance.itemInfoMenu.activeSelf)
        {
            GameController.instance.itemInfoMenu.SetActive(true);
        }
        GameController.instance.itemInfoMenu.GetComponent<ItemInfoMenu>().UpdateMenu(itemName.text, itemWeight.text, itemLength.text, itemReference.itemDescription, itemReference, gameObject);
    }
}
