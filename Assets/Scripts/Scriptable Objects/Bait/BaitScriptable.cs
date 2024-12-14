using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.Fishables.Fish;

namespace Fishing.FishingMechanics
{
    [CreateAssetMenu(fileName = "New Bait Type", menuName = "Bait")]
    public class BaitScriptable : ScriptableObject
    {
        public Sprite inventorySprite;
        public string baitName;
        public string description;
        public GameObject prefab;
        public float cost;
        public float areaOfEffect;
        [SerializeField] private Edible.FoodTypes baitableFishTypes;
        public Edible.FoodTypes BaitableFishTypes { get => baitableFishTypes; private set { } }
        public List<string> effects;
        public List<Sprite> effectsSprites;

        public List<string> GetFoodTypesAsString()
        {
            string typesString = BaitableFishTypes.ToString();
            string[] typesStrings = typesString.Split(", ");
            return new List<string>(typesStrings);
        }
    }
}
