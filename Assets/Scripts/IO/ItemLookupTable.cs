using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.Fishables;

namespace Fishing.IO
{
    public class ItemLookupTable : MonoBehaviour
    {
        public enum ItemType { Fishable, Rod, Gear, Bait }

        [Header("Fishable Items")]
        public List<string> fishableNames;
        public List<GameObject> fishablePrefabs;

        [Header("Fishing Rods")]
        public List<string> rodNames;
        public List<GameObject> rodPrefabs;

        [Header("Gear")]
        public List<string> gearNames;
        public List<GameObject> gearPrefabs;

        [Header("Bait")]
        public List<string> baitNames;
        public List<GameObject> baitPrefabs;

        //FishableData FishableToData(FishableItem fish)
        //{
        //    return new FishableData(fish.GetName(), fish.GetWeight(), fish.GetLength());
        //}
    }

    //public class FishableData
    //{
    //    string name;
    //    float weight;
    //    float length;

    //    public FishableData(string _name, float _weight, float _length)
    //    {
    //        name = _name;
    //        weight = _weight;
    //        length = _length;
    //    }
    //}
}