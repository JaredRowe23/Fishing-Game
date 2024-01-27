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
        private Camera playerCam;

        public static RodManager instance;

        private RodManager() => instance = this;

        private void Awake()
        {
            playerData = PlayerData.instance;
            playerCam = Camera.main;
        }

        private void Start()
        {
            EquipRod(PlayerData.instance.equippedRod, false);
        }

        public void EquipRod(string _rodName, bool _playSound)
        {
            if (_rodName != "")
            {
                playerCam.transform.parent = null;
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

            TooltipSystem.instance.NewTooltip(5f, "Equipped the " + _rodName);
            playerData.equippedRod = _rodName;
            rodsMenu.UpdateEquippedRod();
            if (_playSound) AudioManager.instance.PlaySound("Equip Rod");
            SpawnBait();
        }

        public void SpawnBait()
        {
            for (int i = 0; i < PlayerData.instance.fishingRods.Count; i++)
            {
                if (PlayerData.instance.fishingRods[i] != equippedRod.scriptable.rodName) continue;
                if (PlayerData.instance.equippedBaits[i] == "") return;

                BaitBehaviour _newBait = Instantiate(ItemLookupTable.instance.StringToBait(PlayerData.instance.equippedBaits[i]).prefab, equippedRod.GetHook().transform).GetComponent<BaitBehaviour>();
                equippedRod.equippedBait = _newBait;
                equippedRod.GetHook().hookedObject = _newBait.gameObject;
                _newBait.transform.localPosition = _newBait.anchorPoint;
                _newBait.transform.localRotation = Quaternion.Euler(_newBait.anchorRotation);
                BaitManager.instance.bait = _newBait;
            }
        }
    }

}