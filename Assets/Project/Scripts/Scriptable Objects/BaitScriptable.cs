using Fishing.Fishables;
using System.Collections.Generic;
using UnityEngine;
using Fishing.Fishables.Fish;
using System.Linq;

namespace Fishing.FishingMechanics {
    [CreateAssetMenu(fileName = "New Bait Type", menuName = "Bait")]
    public class BaitScriptable : ScriptableObject {
        [Header("General")]
        [SerializeField, Tooltip("Name of this type of bait.")] private string _baitName;
        public string BaitName { get => _baitName; private set => _baitName = value; }

        [SerializeField, Tooltip("Description of this type of bait.")] private string _description;
        public string Description { get => _description; set => _description = value; }

        [SerializeField, Tooltip("Sprite that will be used for inventory menus.")] private Sprite _inventorySprite;
        public Sprite InventorySprite { get => _inventorySprite; private set => _inventorySprite = value; }

        [SerializeField, Tooltip("Prefab used to spawn this bait into the world.")] private GameObject _prefab;
        public GameObject Prefab { get => _prefab; set => _prefab = value; }

        [SerializeField, Min(0), Tooltip("Cost of this bait when buying from stores.")] private float _cost;
        public float Cost { get => _cost; set => _cost = value; }

        [Header("Effects")]
        [SerializeField, Min(0), Tooltip("Range that fish must be within to gain the of the effects of this bait.")] private float _range;
        public float Range { get => _range; set => _range = value; }

        [SerializeField, Tooltip("Types of fish that can be affected by this bait.")] private List<FishableScriptable> _baitableFishTypes;
        public List<FishableScriptable> BaitableFishTypes { get => _baitableFishTypes; private set { } }

        [SerializeField, Tooltip("Names of the effects this bait contains.")] private List<string> _effects;
        public List<string> Effects { get => _effects; set => _effects = value; }

        [SerializeField, Tooltip("Sprites used for each bait. Respective to the effects list.")] private List<Sprite> _effectsSprites;
        public List<Sprite> EffectsSprites { get => _effectsSprites; set => _effectsSprites = value; }

        public List<string> GetFoodTypesAsString() {
            return new List<string>(from scriptable in BaitableFishTypes select scriptable.ItemName);
        }
    }
}
