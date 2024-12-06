using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.FishingMechanics;
using Fishing.Fishables.Fish;

namespace Fishing.Fishables
{
    public class BaitBehaviour : MonoBehaviour
    {
        [SerializeField] private BaitScriptable scriptable;
        [SerializeField] private Vector3 anchorPoint;
        [SerializeField] private Vector3 anchorRotation;

        private FoodSearch _foodSearch;
        private Edible _edible;

        private void Awake() {
            _foodSearch = GetComponent<FoodSearch>();
            _edible = GetComponent<Edible>();
        }

        private void Start()
        {
            GetComponent<CircleCollider2D>().radius = scriptable.areaOfEffect;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, scriptable.areaOfEffect);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.GetComponent<FoodSearch>() == null) return;

            FoodSearch colSearch = collision.GetComponent<FoodSearch>();

            if (!colSearch.DesiredFoodTypes.HasFlag(_edible.FoodType)) return;

            colSearch.DesiredFood = gameObject;
        }

        public BaitScriptable GetScriptable() => scriptable;
        public Vector3 GetAnchorPoint() => anchorPoint;
        public Vector3 GetAnchorRotation() => anchorRotation;

        private void OnDestroy() {
            BaitManager.instance.RemoveFish(_foodSearch);
            if (RodManager.instance.equippedRod.GetHook().hookedObject = gameObject) {
                RodManager.instance.equippedRod.GetHook().hookedObject = null;
            }
        }
    }
}
