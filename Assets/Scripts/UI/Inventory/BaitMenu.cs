using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.IO;

namespace Fishing.UI
{
    public class BaitMenu : MonoBehaviour
    {
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private GameObject slotParent;
        [SerializeField] private float slotXStart;
        [SerializeField] private float slotYStart;
        [SerializeField] private float slotXPadding;
        [SerializeField] private float slotYPadding;
        [SerializeField] private int slotXMax;

        [SerializeField] private List<Sprite> itemSprites;

        public void ShowBaitMenu(bool _active)
        {
            this.gameObject.SetActive(_active);
            InventoryMenu.instance.UpdateActiveMenu(1);
            GenerateSlots();
        }

        public void GenerateSlots()
        {
            foreach (Transform _child in slotParent.transform)
            {
                Destroy(_child.gameObject);
            }

            int i = 0;
            if (GameController.instance.GetComponent<PlayerData>().bait.Count > 0)
            {
                foreach (string _bait in GameController.instance.GetComponent<PlayerData>().bait)
                {
                    GameObject _newSlot = Instantiate(slotPrefab, slotParent.transform);
                    _newSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2((i % slotXMax) * slotXPadding + slotXStart, Mathf.Floor(i / slotXMax) * slotYPadding + slotYStart);
                    BaitInventorySlot _invSlot = _newSlot.GetComponent<BaitInventorySlot>();

                    _invSlot.title.text = _bait;
                    _invSlot.countText.text = "x" + GameController.instance.GetComponent<PlayerData>().baitCounts[i].ToString();

                    i++;
                }
            }
        }
    }

}