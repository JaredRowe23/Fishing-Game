using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.FishingMechanics;
using Fishing.Fishables.Fish;

namespace Fishing.Fishables
{
    public class BaitBehaviour : MonoBehaviour, IEdible
    {
        public BaitScriptable scriptable;
        public Vector3 anchorPoint;
        public Vector3 anchorRotation;

        private void Start()
        {
            GetComponent<CircleCollider2D>().radius = scriptable.areaOfEffect;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, scriptable.areaOfEffect);
        }
        public void Despawn()
        {
            FoodSearchManager.instance.RemoveFish(GetComponent<FoodSearch>());
            FoodSearchManager.instance.RemoveFood(GetComponent<Edible>());
            BaitManager.instance.RemoveFish(GetComponent<FoodSearch>());
            DestroyImmediate(gameObject);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.GetComponent<FoodSearch>() == null) return;

            FoodSearch colSearch = collision.GetComponent<FoodSearch>();

            if (!colSearch.DesiredTypesToInts().Contains(GetComponent<Edible>().GetFoodType())) return;

            colSearch.desiredFood = gameObject;
        }
    }
}
