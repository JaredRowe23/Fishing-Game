using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.IO;

namespace Fishing.UI
{
    public class RodsMenu : MonoBehaviour
    {
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private GameObject content;
        [SerializeField] private RodInfoMenu rodInfoMenu;

        private PlayerData playerData;
        private RodManager rodManager;

        public static RodsMenu instance;

        private RodsMenu() => instance = this;

        private void Awake()
        {
            rodManager = RodManager.instance;
            playerData = PlayerData.instance;
        }


        private void Start() => GenerateSlots();

        public void ShowRodMenu(bool _active)
        {
            gameObject.SetActive(_active);
            InventoryMenu.instance.UpdateActiveMenu(0);
            GenerateSlots();
            if (!gameObject.activeSelf) rodInfoMenu.HideButtonScrollViews();
        }

        public void GenerateSlots()
        {
            foreach (Transform _child in content.transform)
            {
                Destroy(_child.gameObject);
            }
            int i = 0;
            foreach (string _rod in playerData.fishingRods)
            {
                RodInventorySlot _newSlot = Instantiate(slotPrefab, content.transform).GetComponent<RodInventorySlot>();

                _newSlot.title.text = _rod;

                int j = 0;
                List<GameObject> _rodPrefabs = rodManager.rodPrefabs;
                List<Sprite> _rodSprites = rodManager.rodSprites;
                foreach (GameObject _prefab in _rodPrefabs)
                {
                    if (_prefab.name == _rod)
                    {
                        _newSlot.itemReference = _prefab;
                        _newSlot.sprite.sprite = _rodSprites[j];
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
            foreach (Transform _slot in content.transform)
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
    }
}