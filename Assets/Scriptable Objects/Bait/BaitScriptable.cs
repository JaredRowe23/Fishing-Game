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
        [SerializeField] private Edible.FoodTypes[] baitableFishTypes;
        public List<string> effects;
        public List<Sprite> effectsSprites;

        public List<string> GetFoodTypesAsString()
        {
            List<string> _types = new List<string>();
            foreach(Edible.FoodTypes _type in baitableFishTypes)
            {
                _types.Add(_type.ToString());
            }
            if (_types.Count == 0) return null;
            return _types;
        }

        public long GetFoodTypes()
        {
            if (baitableFishTypes.Length > 9)
            {
                Debug.LogError("Too many food types assigned to object for c# long to handle!", this);
                return 0;
            }
            string _typesString = "1";
            for (int i = 0; i < baitableFishTypes.Length; i++)
            {
                int _typeInt = (int)baitableFishTypes[i];
                if (_typeInt < 10)
                {
                    _typesString += "0";
                }
                _typesString += _typeInt.ToString();
            }
            long types = long.Parse(_typesString);
            return types;
        }
    }
}
