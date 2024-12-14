using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.FishingMechanics;
using Fishing.UI;
using Fishing.IO;
using Fishing.Fishables;

namespace Fishing
{
    public class RodManager : MonoBehaviour
    {
        public RodBehaviour equippedRod;
        public RodsMenu rodsMenu;

        public List<GameObject> rodPrefabs;
        public List<Sprite> rodSprites;

        private PlayerData playerData;

        public static RodManager instance;

        private RodManager() => instance = this;

        private void Awake()
        {
            playerData = PlayerData.instance;
        }

        private void Start()
        {
            EquipRod(PlayerData.instance.equippedRod.rodName, false);
        }

        public void EquipRod(string _rodName, bool _playSound)
        {
            if (_rodName != "")
            {
                if (equippedRod != null) DestroyImmediate(equippedRod.gameObject);
            }
            else
            {
                _rodName = "Wooden Fishing Rod";
            }

            foreach (GameObject _prefab in rodPrefabs)
            {
                if (_prefab.name != _rodName) continue;

                equippedRod = Instantiate(_prefab).GetComponent<RodBehaviour>();
            }

            for (int i = 0; i < playerData.fishingRodSaveData.Count; i++)
            {
                if (playerData.fishingRodSaveData[i].rodName != _rodName) continue;
                playerData.equippedRod = playerData.fishingRodSaveData[i];
            }

            TooltipSystem.instance.NewTooltip(5f, "Equipped the " + _rodName);
            rodsMenu.UpdateEquippedCheckmark();
            if (_playSound) AudioManager.instance.PlaySound("Equip Rod");

            SpawnBait();
        }

        public void SpawnBait()
        {
            if (playerData.equippedRod.equippedBait == null) return;
            if (playerData.equippedRod.equippedBait.baitName == null) return;
            if (playerData.equippedRod.equippedBait.baitName == "") return;

            BaitBehaviour _newBait = Instantiate(ItemLookupTable.instance.StringToBait(playerData.equippedRod.equippedBait.baitName).prefab, equippedRod.GetHook().transform).GetComponent<BaitBehaviour>();
            equippedRod.equippedBait = _newBait;
            equippedRod.GetHook().hookedObject = _newBait.gameObject;
            _newBait.transform.localPosition = _newBait.AnchorPoint;
            _newBait.transform.localRotation = Quaternion.Euler(0f, 0f, _newBait.AnchorRotation);
        }
    }

}