using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.Fishables;
using Fishing.Fishables.Fish;
using Fishing.FishingMechanics;
using Fishing.UI;

namespace Fishing
{
    public class GameController : MonoBehaviour
    {
        public RodBehaviour equippedRod;
        public GameObject rodsMenu;

        public static GameController instance;

        private GameController() => instance = this;

        public void SpawnRod(string _rodName) => rodsMenu.GetComponent<RodsMenu>().EquipRod(_rodName, false);

    }

}