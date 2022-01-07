using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaitMenu : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject slotParent;
    [SerializeField] private float slotXStart;
    [SerializeField] private float slotYStart;
    [SerializeField] private float slotXPadding;
    [SerializeField] private float slotYPadding;
    [SerializeField] private int slotXMax;

    private bool initialized;

    [SerializeField] private List<Sprite> itemSprites;

    private void Start()
    {
        GenerateSlots();
    }

    public void ShowRodMenu()
    {
        this.gameObject.SetActive(!this.gameObject.activeSelf);
        GameController.instance.inventoryMenu.UpdateActiveMenu(1);
        GenerateSlots();
    }

    public void GenerateSlots()
    {
        foreach (Transform child in slotParent.transform)
        {
            Destroy(child.gameObject);
        }

        int i = 0;
        foreach (string bait in GameController.instance.GetComponent<PlayerData>().bait)
        {
            Debug.Log("test");
            GameObject newSlot = Instantiate(slotPrefab, slotParent.transform);
            newSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2((i % slotXMax) * slotXPadding + slotXStart, Mathf.Floor(i / slotXMax) * slotYPadding + slotYStart);
            BaitInventorySlot invSlot = newSlot.GetComponent<BaitInventorySlot>();

            invSlot.title.text = bait;
            invSlot.countText.text = "x " + GameController.instance.GetComponent<PlayerData>().baitCounts[i].ToString();
            
            i++;
        }
    }
}
