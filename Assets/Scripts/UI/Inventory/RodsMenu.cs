using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RodsMenu : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject slotParent;
    [SerializeField] private float slotXStart;
    [SerializeField] private float slotYStart;
    [SerializeField] private float slotXPadding;
    [SerializeField] private float slotYPadding;
    [SerializeField] private int slotXMax;

    private bool initialized;

    [SerializeField] public List<GameObject> rodPrefabs;
    [SerializeField] public List<Sprite> rodSprites;

    public static RodsMenu instance;

    private RodsMenu()
    {
        instance = this;
    }

    private void Start()
    {
        GenerateSlots();
    }

    public void ShowRodMenu(bool active)
    {
        gameObject.SetActive(active);
        InventoryMenu.instance.UpdateActiveMenu(0);
        GenerateSlots();
    }

    public void GenerateSlots()
    {
        foreach (Transform child in slotParent.transform)
        {
            Destroy(child.gameObject);
        }
        int i = 0;
        foreach (string rod in GameController.instance.GetComponent<PlayerData>().fishingRods)
        {
            GameObject newSlot = Instantiate(slotPrefab, slotParent.transform);
            newSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2((i % slotXMax) * slotXPadding + slotXStart, Mathf.Floor(i / slotXMax) * slotYPadding + slotYStart);
            RodInventorySlot invSlot = newSlot.GetComponent<RodInventorySlot>();

            invSlot.title.text = rod;

            int j = 0;
            foreach (GameObject prefab in rodPrefabs)
            {
                if (prefab.name == rod)
                {
                    invSlot.itemReference = prefab;
                    invSlot.sprite.sprite = rodSprites[j];
                    break;
                }
                j++;
            }

            UpdateEquippedRod();

            i++;
        }
    }

    public void UpdateEquippedRod()
    {
        foreach(Transform slot in slotParent.transform)
        {
            RodInventorySlot invSlot = slot.GetComponent<RodInventorySlot>();
            if (invSlot.itemReference.name == GameController.instance.GetComponent<PlayerData>().equippedRod)
            {
                invSlot.equippedCheck.SetActive(true);
            }
            else
            {
                invSlot.equippedCheck.SetActive(false);
            }
        }
    }

    public void EquipRod(string rodName)
    {
        Destroy(GameController.instance.equippedRod);
        foreach(GameObject prefab in rodPrefabs)
        {
            if (prefab.name == rodName)
            {
                GameObject newRod = Instantiate(prefab);
                GameController.instance.equippedRod = newRod;
                foreach(Transform child in newRod.transform)
                {
                    if (child.GetComponent<HookControl>())
                    {
                        Camera.main.GetComponent<CameraBehaviour>().hook = child.GetComponent<HookControl>();
                        Camera.main.transform.parent = child.transform;
                    }
                }
            }
        }
        GameController.instance.GetComponent<PlayerData>().equippedRod = rodName;
        UpdateEquippedRod();
        AudioManager.instance.PlaySound("Equip Rod");
    }

}
