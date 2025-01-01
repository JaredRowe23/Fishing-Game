using Fishing.Fishables.FishGrid;
using Fishing.FishingMechanics;
using Fishing.FishingMechanics.Minigame;
using Fishing.Util.Math;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.Fishables.Fish {
    public class FoodSearch : MonoBehaviour {
        [SerializeField, Range(0, 360), Tooltip("Angle in degrees in front this fish can see within.")] private float _sightAngle;
        public float SightAngle { get => _sightAngle; private set { } }

        [SerializeField, Min(0), Tooltip("Maximum distance this fish can see food items at.")] private float _sightDistance;
        public float SightDistance { get => _sightDistance; private set { } }

        [SerializeField, Min(0), Tooltip("Distance this fish can smell food items within.")] private float _smellRadius;
        public float SmellRadius { get => _smellRadius; private set { } }

        [SerializeField, Tooltip("Types of fish this fish is able to eat.")] private List<FishableScriptable> _desiredFoodTypes;
        public List<FishableScriptable> DesiredFoodTypes { get => _desiredFoodTypes; private set { } }

        private GameObject _desiredFood;
        public GameObject DesiredFood { get => _desiredFood; set { _desiredFood = value; } }

        [Header("Gizmos")]
        #region
        [SerializeField] private bool _drawSearchingGizmos = false;
        [SerializeField] private Color _smellRadiusColor = Color.green;
        [SerializeField] private Color _sightConeColor = Color.green;

        [Space(20)]
        [SerializeField] private bool _drawDesiredFoodGizmos = false;
        [SerializeField] private Color _desiredFoodRayColor = Color.green;
        #endregion

        private Fishable _fishable;
        private Hunger _hunger;
        private RodManager _rodManager;

        private void OnValidate() {
            if (_smellRadius > _sightDistance) {
                _smellRadius = _sightDistance;
            }
        }

        private void Awake() {
            _hunger = GetComponent<Hunger>();
            _fishable = GetComponent<Fishable>();
            _rodManager = RodManager.Instance;
        }

        private void FixedUpdate() {
            if (_fishable.IsHooked) {
                return;
            }
            if (DesiredFood != null && !DesiredFood.TryGetComponent(out BaitBehaviour _)) {
                DetermineDesiredFood();
            }
            if (DesiredFood == null) {
                return; 
            }
            if (!IsWithinEatRange()) {
                return;
            }
            Eat();
        }

        private bool IsWithinEatRange() {
            float distanceToFood = Vector2.Distance(transform.position, DesiredFood.transform.position);
            float thisHalfLength = transform.localScale.x * 0.5f;
            float foodHalfLength = DesiredFood.transform.localScale.x * 0.5f;
            float eatDistance = thisHalfLength + foodHalfLength;
            bool isWithinRange = distanceToFood <= eatDistance;
            return isWithinRange;
        }

        public void Eat() {
            HookBehaviour hook = _rodManager.EquippedRod.Hook;

            if (hook.gameObject == DesiredFood) {
                hook.SetHook(_fishable);
                ReelingMinigame.Instance.InitiateMinigame(_fishable);
                return;
            }

            if (DesiredFood.TryGetComponent(out Fishable fishable)) {
                if (fishable.IsHooked) {
                    hook.SetHook(_fishable);
                    ReelingMinigame.Instance.InitiateMinigame(_fishable);
                }
                _hunger.AddFood(DesiredFood.GetComponent<Edible>());
            }

            else if (DesiredFood.TryGetComponent(out BaitBehaviour _)) {
                hook.SetHook(_fishable);
                ReelingMinigame.Instance.InitiateMinigame(_fishable);
            }

            GetComponent<AudioSource>().Play();
            Destroy(DesiredFood);
            DesiredFood = null;
        }

        private void DetermineDesiredFood() {
            GameObject newDesiredFood = null;
            List<Fishable> fishablesWithinRange = FishableGrid.instance.GetNearbyFishables(_fishable.GridSquare[0], _fishable.GridSquare[1], SightDistance);
            Vector2 thisPosition = transform.position;
            Vector2 thisForward = transform.forward;
            foreach(Fishable fishable in fishablesWithinRange) {
                if (fishable == _fishable) {
                    continue;
                }
                if (fishable.IsHooked) {
                    continue;
                }
                Vector2 fishablePosition = fishable.transform.position;
                if (fishablePosition.y >= 0) {
                    continue;
                }

                float distance = Vector2.Distance(fishablePosition, thisPosition);
                if (distance > SightDistance) {
                    continue;
                }
                if (newDesiredFood != null) {
                    if (distance > Vector2.Distance(thisPosition, (Vector2)newDesiredFood.transform.position)) {
                        continue;
                    }
                }

                if (!DesiredFoodTypes.Contains(fishable.FishableScriptable)) {
                    continue;
                }

                if (distance <= SmellRadius) {
                    newDesiredFood = fishable.gameObject;
                    continue;
                }
                if (MathHelpers.IsWithinAngleOfDirection(thisPosition, fishablePosition, thisForward, SightAngle)) {
                    newDesiredFood = fishable.gameObject;
                    continue;
                }
            }
            DesiredFood = newDesiredFood;
        }

        public void RemoveAsActivePredator() {
            if (DesiredFood == null) return;
            if (!DesiredFood.GetComponent<FishMovement>()) return;

            DesiredFood.GetComponent<FishMovement>().ActivePredator = null;
        }

        private void OnDestroy() {
            RemoveAsActivePredator();
        }

        private void OnDrawGizmosSelected() {
            if (!DesiredFood) {
                if (!_drawSearchingGizmos) {
                    return;
                }
                DrawNoFoodGizmos();
            }
            else {
                if (!_drawDesiredFoodGizmos) {
                    return;
                }
                DrawDesiredFoodGizmos();
            }
        }

        private void DrawNoFoodGizmos() {
            DrawSmellRadius();
            DrawSightCone();
        }

        private void DrawDesiredFoodGizmos() {
            DrawDesiredFoodRay();
        }

        private void DrawSmellRadius() {
            Gizmos.color = _smellRadiusColor;
            Gizmos.DrawWireSphere(transform.position, SmellRadius);
        }
        private void DrawSightCone() {
            Gizmos.color = _sightConeColor;

            Vector2 _dir = Quaternion.Euler(0f, 0f, -SightAngle) * transform.up;
            Gizmos.DrawRay(transform.position, _dir * SightDistance);

            _dir = Quaternion.Euler(0f, 0f, SightAngle) * transform.up;
            Gizmos.DrawRay(transform.position, _dir * SightDistance);
        }
        private void DrawDesiredFoodRay() {
            Gizmos.color = _desiredFoodRayColor;
            Gizmos.DrawRay(transform.position, (DesiredFood.transform.position - transform.position));
        }
    }
}