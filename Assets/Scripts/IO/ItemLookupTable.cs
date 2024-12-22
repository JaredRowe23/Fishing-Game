using Fishing.Fishables.Fish;
using Fishing.FishingMechanics;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.IO {
    public class ItemLookupTable : MonoBehaviour {
        [SerializeField] private List<FishableScriptable> _fishScriptables;
        public List<FishableScriptable> FishScriptables { get => _fishScriptables;}
        [SerializeField] private List<RodScriptable> _rodScriptables;
        public List<RodScriptable> RodScriptables { get => _rodScriptables;}
        [SerializeField] private List<BaitScriptable> _baitScriptables;
        public List<BaitScriptable> BaitScriptables { get => _baitScriptables;}

        private static ItemLookupTable _instance;
        public static ItemLookupTable Instance;

        private void Awake() {
            Instance = this;
        }

        public FishableScriptable StringToFishScriptable(string fishableName) {
            for (int i = 0; i < FishScriptables.Count; i++) {
                if (FishScriptables[i].ItemName != fishableName) {
                    continue;
                }
                return FishScriptables[i];
            }

            Debug.Log("No fishable exists with provided string");
            return null;
        }

        public RodScriptable StringToRodScriptable(string rodName) {
            for (int i = 0; i < RodScriptables.Count; i++) {
                if (RodScriptables[i].RodName != rodName) {
                    continue;
                }
                return RodScriptables[i];
            }

            Debug.Log("No rod exists with provided string");
            return null;
        }

        public BaitScriptable StringToBaitScriptable(string baitName) {
            for (int i = 0; i < BaitScriptables.Count; i++) {
                if (BaitScriptables[i].BaitName != baitName) {
                    continue;
                }
                return BaitScriptables[i];
            }

            Debug.Log("No bait exists with provided string");
            return null;
        }
    }
}