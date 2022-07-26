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
    }
}