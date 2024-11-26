using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.FishingMechanics;
using System.Linq;
using Fishing.Util;

namespace Fishing.Fishables.Fish
{
    [RequireComponent(typeof(Fishable))]
    [RequireComponent(typeof(Hunger))]
    public class FoodSearch : MonoBehaviour {
        [SerializeField] private float _eatDistance;
        public float EatDistance { get => _eatDistance; private set { } }

        [SerializeField] private float _sightAngle;
        public float SightAngle { get => _sightAngle; private set { } }

        [SerializeField] private float _sightDistance;
        public float SightDistance { get => _sightDistance; private set { } }

        [SerializeField] private float _smellRadius;
        public float SmellRadius { get => _smellRadius; private set { } }

        [SerializeField] private Edible.FoodTypes _desiredFoodTypes;
        public Edible.FoodTypes DesiredFoodTypes { get => _desiredFoodTypes; private set { } }

        private GameObject _desiredFood;
        public GameObject DesiredFood { get => _desiredFood; set { _desiredFood = value; } }

        private Fishable _fishable;
        private Hunger _hunger;
        private Edible _edible;

        private void Awake() {
            _hunger = GetComponent<Hunger>();
            _fishable = GetComponent<Fishable>();
            _edible = GetComponent<Edible>();
        }

        private void FixedUpdate() {
            if (_fishable.isHooked) return;
            DetermineDesiredFood();
            if (DesiredFood == null) return;
            if (Vector2.Distance(transform.position, DesiredFood.transform.position) >= (transform.localScale.x * 0.5f) + (DesiredFood.transform.localScale.x * 0.5f)) return;
            Eat();
        }

        public void Eat() {
            HandleHookedItem();
            _hunger.AddFood(DesiredFood);
            GetComponent<AudioSource>().Play();
            Debug.Log($"{gameObject.name} ate {DesiredFood.name}");
            DesiredFood.GetComponent<IEdible>().Despawn();
            DesiredFood = null;
        }

        private void HandleHookedItem() {
            if (DesiredFood.TryGetComponent<HookBehaviour>(out HookBehaviour hook)) {
                hook.SetHook(_fishable);
                return;
            }

            if (DesiredFood.TryGetComponent<Fishable>(out Fishable hookedFishable)) {
                if (hookedFishable.isHooked) {
                    _fishable.SetThisToHooked();
                    return;
                }
            }
            if (DesiredFood.TryGetComponent<BaitBehaviour>(out BaitBehaviour bait)) {
                _fishable.SetThisToHooked();
                return;
            }
        }

        private void DetermineDesiredFood() {
            GameObject newDesiredFood = null;
            List<Edible> ediblesWithinRange = FishableGrid.instance.GetNearbyEdibles(_fishable.GridSquare[0], _fishable.GridSquare[1], _fishable.Range);
            foreach(Edible edible in ediblesWithinRange) {
                if (edible == _edible) {
                    continue;
                }
                if (edible.GetComponent<Fishable>().isHooked) {
                    continue;
                }
                if (edible.transform.position.y >= 0) {
                    continue;
                }

                float distance = Vector2.Distance(edible.transform.position, transform.position);
                if (distance > SightDistance) {
                    continue;
                }
                if (newDesiredFood != null) {
                    if (distance > Vector2.Distance(transform.position, (Vector2)newDesiredFood.transform.position)) {
                        continue;
                    }
                }

                if (!DesiredFoodTypes.HasFlag(edible.FoodType)) {
                    continue;
                }
                if (distance <= SmellRadius) {
                    newDesiredFood = edible.gameObject;
                    continue;
                }
                if (Utilities.IsWithinAngleOfDirection(transform.position, edible.transform.position, transform.forward, SightAngle)) {
                    newDesiredFood = edible.gameObject;
                    continue;
                }
            }
            DesiredFood = newDesiredFood;
        }

        private void OnDrawGizmosSelected() {
            if (!DesiredFood) {
                DrawNoFoodGizmos();
            }
            else {
                DrawDesiredFoodGizmos();
            }
        }

        private void DrawNoFoodGizmos() {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, SmellRadius);

            Vector2 _dir = transform.up;
            _dir = Quaternion.Euler(0f, 0f, -SightAngle) * _dir;
            Gizmos.DrawRay(transform.position, _dir * SightDistance);

            _dir = transform.up;
            _dir = Quaternion.Euler(0f, 0f, SightAngle) * _dir;
            Gizmos.DrawRay(transform.position, _dir * SightDistance);
        }

        private void DrawDesiredFoodGizmos() {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, (DesiredFood.transform.position - transform.position));
        }
    }
}