using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.Fishables;
using Fishing.FishingMechanics;

namespace Fishing.IO
{
    public class ItemLookupTable : MonoBehaviour
    {
        [SerializeField] public List<RodScriptable> rodScriptables;
        [SerializeField] public List<BaitScriptable> baitScriptables;

        public static ItemLookupTable instance;

        private ItemLookupTable() => instance = this;
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public RodScriptable StringToRod(string _rodName)
        {
            foreach (RodScriptable _rodScriptable in rodScriptables)
            {
                if (_rodName == _rodScriptable.rodName)
                {
                    return _rodScriptable;
                }
            }

            Debug.Log("No rod exists with provided string");
            return null;
        }

        public BaitScriptable StringToBait(string _baitName)
        {
            foreach (BaitScriptable _baitScriptable in baitScriptables)
            {
                if (_baitName == _baitScriptable.baitName)
                {
                    return _baitScriptable;
                }
            }

            Debug.Log("No bait exists with provided string");
            return null;
        }
    }
}