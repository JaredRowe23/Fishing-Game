using Fishing.Fishables.Fish;
using Fishing.Fishables.FishGrid;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.FishingMechanics {
    public class BaitBehaviour : MonoBehaviour {
        [SerializeField, Tooltip("Scriptable object that carries the stats for this type of bait.")] private BaitScriptable _scriptable;
        public BaitScriptable Scriptable { get => _scriptable; private set { } }

        [SerializeField, Tooltip("The position relative to the hook for this object to appear attached.")] private Vector3 _anchorPoint;
        public Vector3 AnchorPoint { get => _anchorPoint; private set { } }

        [SerializeField, Tooltip("The rotation this object will be spawned at to appear attached.")] private float _anchorRotation;
        public float AnchorRotation { get => _anchorRotation; private set { } }

        [SerializeField] private bool _drawBaitRangeGizmo = false;
        [SerializeField] private Color _baitRangeGizmoColor = Color.blue;

        private Edible _edible;
        private RodManager _rodManager;
        private FishableGrid _fishableGrid;

        private void OnValidate() {
            if (_anchorRotation >= 360f) {
                _anchorRotation -= 360f;
            }
            else if (_anchorRotation <= -360f) {
                _anchorRotation += 360f;
            }
        }

        private void Awake() {
            _edible = GetComponent<Edible>();
        }

        private void Start() {
            GetComponent<CircleCollider2D>().radius = Scriptable.areaOfEffect;
            _rodManager = RodManager.Instance;
            _fishableGrid = FishableGrid.instance;
        }

        private void FixedUpdate() {
            ApplyBaitToNearbyFoodSearches();
        }

        private void ApplyBaitToNearbyFoodSearches() {
            int[] gridSquare = _fishableGrid.Vector2ToGrid(transform.position);
            List<FoodSearch> foodSearches = _fishableGrid.GetNearbyFoodSearches(gridSquare[0], gridSquare[1], Scriptable.areaOfEffect);
            for (int foodSearchIndex = 0; foodSearchIndex < foodSearches.Count; foodSearchIndex++) {
                float distance = Vector2.Distance(foodSearches[foodSearchIndex].transform.position, transform.position);
                if (distance > Scriptable.areaOfEffect) {
                    continue;
                }

                if (!Scriptable.BaitableFishTypes.HasFlag(foodSearches[foodSearchIndex].GetComponent<Edible>().FoodType)) {
                    continue;
                }

                foodSearches[foodSearchIndex].DesiredFood = gameObject;
            }
        }

        private void OnTriggerStay2D(Collider2D collision) {
            if (collision.GetComponent<FoodSearch>() == null) {
                return;
            }

            FoodSearch colSearch = collision.GetComponent<FoodSearch>();

            if (!colSearch.DesiredFoodTypes.HasFlag(_edible.FoodType)) {
                return;
            }

            colSearch.DesiredFood = gameObject;
        }

        private void OnDestroy() {
            if (_rodManager.EquippedRod.Hook.HookedObject == gameObject) {
                _rodManager.EquippedRod.Hook.HookedObject = null;
            }
        }

        private void OnDrawGizmosSelected() {
            if (_drawBaitRangeGizmo) {
                DrawBaitRangeGizmo();
            }
        }

        private void DrawBaitRangeGizmo() {
            Gizmos.color = _baitRangeGizmoColor;
            Gizmos.DrawWireSphere(transform.position, Scriptable.areaOfEffect);
        }
    }
}
