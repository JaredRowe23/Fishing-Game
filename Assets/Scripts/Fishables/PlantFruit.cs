using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.Fishables.Fish;
using Fishing.FishingMechanics;

namespace Fishing.Fishables
{
    public class PlantFruit : MonoBehaviour, IEdible
    {
        private PlantStalk stalk;

        private void Awake()
        {
            stalk = transform.parent.GetComponent<PlantStalk>();
        }
        public void Despawn()
        {
            stalk.RemoveFruit(gameObject);
            FoodSearchManager.instance.RemoveFood(GetComponent<Edible>());
            DestroyImmediate(gameObject);
        }
    }
}
