using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.FishingMechanics;
using Fishing.Fishables.Fish;

namespace Fishing.Fishables
{
    public class BaitBehaviour : MonoBehaviour, IEdible
    {
        [SerializeField] private BaitScriptable scriptable;
        [SerializeField] private Vector3 anchorPoint;
        [SerializeField] private Vector3 anchorRotation;

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
            BaitManager.instance.RemoveFish(GetComponent<FoodSearch>());
            DestroyImmediate(gameObject);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.GetComponent<FoodSearch>() == null) return;

            FoodSearch colSearch = collision.GetComponent<FoodSearch>();

            if (!colSearch.DesiredFoodTypes.HasFlag(GetComponent<Edible>().FoodType)) return;

            colSearch.DesiredFood = gameObject;
        }

        public BaitScriptable GetScriptable() => scriptable;
        public Vector3 GetAnchorPoint() => anchorPoint;
        public Vector3 GetAnchorRotation() => anchorRotation;
    }
}
