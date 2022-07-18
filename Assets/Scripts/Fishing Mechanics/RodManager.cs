using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.FishingMechanics;
using Fishing.UI;
using Fishing.IO;

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
                _rodName = "Basic Rod";
            }

            foreach (GameObject _prefab in rodPrefabs)
            {
                if (_prefab.name != _rodName) continue;

                Instantiate(_prefab);
            }

            playerData.equippedRod = _rodName;
            rodsMenu.UpdateEquippedRod();
            if (_playSound) AudioManager.instance.PlaySound("Equip Rod");
        }
    }

}