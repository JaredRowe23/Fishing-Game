using Fishing.FishingMechanics;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.IO {
    public class ItemLookupTable : MonoBehaviour {
        [SerializeField] public List<RodScriptable> rodScriptables;
        [SerializeField] public List<BaitScriptable> baitScriptables;

        public static ItemLookupTable instance;

        private ItemLookupTable() => instance = this;
        private void Awake() {
            DontDestroyOnLoad(gameObject);
        }

        public RodScriptable StringToRodScriptable(string rodName) {
            for (int i = 0; i < rodScriptables.Count; i++) {
                if (rodScriptables[i].rodName != rodName) {
                    continue;
                }
                return rodScriptables[i];
            }

            Debug.Log("No rod exists with provided string");
            return null;
        }

        public BaitScriptable StringToBaitScriptable(string baitName) {
            for (int i = 0; i < baitScriptables.Count; i++) {
                if (baitScriptables[i].baitName != baitName) {
                    continue;
                }
                return baitScriptables[i];
            }

            Debug.Log("No bait exists with provided string");
            return null;
        }
    }
}