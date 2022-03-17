using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.IO;

namespace Fishing.UI
{
    public class RodsMenu : MonoBehaviour
    {
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private GameObject slotParent;
        [SerializeField] private float slotXStart;
        [SerializeField] private float slotYStart;
        [SerializeField] private float slotXPadding;
        [SerializeField] private float slotYPadding;
        [SerializeField] private int slotXMax;

        public List<GameObject> rodPrefabs;
        public List<Sprite> rodSprites;

        private PlayerData playerData;

        public static RodsMenu instance;

        private RodsMenu() => instance = this;

        private void Awake() => playerData = GameController.instance.GetComponent<PlayerData>();

        private void Start() => GenerateSlots();

        public void ShowRodMenu(bool _active)
        {
            gameObject.SetActive(_active);
            InventoryMenu.instance.UpdateActiveMenu(0);
            GenerateSlots();
        }

        public void GenerateSlots()
        {
            foreach (Transform _child in slotParent.transform)
            {
                Destroy(_child.gameObject);
            }
            int i = 0;
            foreach (string _rod in playerData.fishingRods)
            {
                GameObject _newSlot = Instantiate(slotPrefab, slotParent.transform);
                _newSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2((i % slotXMax) * slotXPadding + slotXStart, Mathf.Floor(i / slotXMax) * slotYPadding + slotYStart);
                RodInventorySlot _invSlot = _newSlot.GetComponent<RodInventorySlot>();

                _invSlot.title.text = _rod;

                int j = 0;
                foreach (GameObject _prefab in rodPrefabs)
                {
                    if (_prefab.name == _rod)
                    {
                        _invSlot.itemReference = _prefab;
                        _invSlot.sprite.sprite = rodSprites[j];
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
            foreach (Transform _slot in slotParent.transform)
            {
                RodInventorySlot _invSlot = _slot.GetComponent<RodInventorySlot>();
                if (_invSlot.itemReference.name == playerData.equippedRod)
                {
                    _invSlot.equippedCheck.SetActive(true);
                }
                else
                {
                    _invSlot.equippedCheck.SetActive(false);
                }
            }
        }

        //may want to move this to another script to handle inventory item instancing and such
        public void EquipRod(string _rodName, bool _playSound)
        {
            if (_rodName != "")
            {
                Camera.main.transform.parent = null;
                DestroyImmediate(GameController.instance.equippedRod.gameObject);
            }
            else
            {
                _rodName = "Basic Rod";
            }

            foreach (GameObject _prefab in rodPrefabs)
            {
                if (_prefab.name != _rodName) continue;

                Instantiate(_prefab);
            }
            playerData.equippedRod = _rodName;
            UpdateEquippedRod();
            if (_playSound) AudioManager.instance.PlaySound("Equip Rod");
        }
    }
}