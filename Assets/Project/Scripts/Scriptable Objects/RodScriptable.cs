using UnityEngine;

namespace Fishing.FishingMechanics {
    [CreateAssetMenu(fileName = "New Fishing Rod", menuName = "Fishing Rod")]
    public class RodScriptable : ScriptableObject {
        [Header("General info")]
        [SerializeField, Tooltip("Name of this fishing rod.")] private string _rodName;
        public string RodName { get => _rodName; set => _rodName = value; }

        [SerializeField, Tooltip("Description of this fishing rod.")] private string _description;
        public string Description { get => _description; set => _description = value; }

        [SerializeField, Min(0), Tooltip("Cost of this fishing rod when buying from stores.")] private float _cost;
        public float Cost { get => _cost; set => _cost = value; }

        [SerializeField, Tooltip("GameObject prefab that will be spawned in for this rod.")] private GameObject _prefab;
        public GameObject Prefab { get => _prefab; set => _prefab = value; }

        [SerializeField, Tooltip("Sprite that will be used for inventory menus.")] private Sprite _inventorySprite;
        public Sprite InventorySprite { get => _inventorySprite; set => _inventorySprite = value; }

        [Header("Line")]
        [SerializeField, Min(0), Tooltip("Length in meters of the fishing line.")] private float _lineLength;
        public float LineLength { get => _lineLength; set => _lineLength = value; }

        [SerializeField, Min(0), Tooltip("Strength of the fishing line. For use in the fishing minigame.")] private float _lineStrength;
        public float LineStrength { get => _lineStrength; set => _lineStrength = value; }

        [Header("Casting")]

        [SerializeField, Min(0), Tooltip("Amount of force to cast the hook with when casting with the minimum strength value.")] private float _minCastStrength;
        public float MinCastStrength { get => _minCastStrength; set => _minCastStrength = value; }

        [SerializeField, Min(0), Tooltip("Amount of force to cast the hook with when casting with the maximum strength value.")] private float _maxCastStrength;
        public float MaxCastStrength { get => _maxCastStrength; set => _maxCastStrength = value; }

        [SerializeField, Range(0, 45), Tooltip("Highest angle you can cast the hook at.")] private float _maxCastAngle;
        public float MaxCastAngle { get => _maxCastAngle; set => _maxCastAngle = value; }

        [SerializeField, Min(0), Tooltip("Oscillations per second for charging the power of the cast.")] private float _chargeFrequency;
        public float ChargeFrequency { get => _chargeFrequency; set => _chargeFrequency = value; }

        [SerializeField, Min(0), Tooltip("Oscillations per second for selecting the angle to cast at.")] private float _angleFrequency;
        public float AngleFrequency { get => _angleFrequency; set => _angleFrequency = value; }

        [Header("Reeling")]
        [SerializeField, Min(0), Tooltip("Amount of force to add to the fishable when the minigame icon is within the reel zone.")] private float _reelForce;
        public float ReelForce { get => _reelForce; set => _reelForce = value; }

        [SerializeField, Min(0), Tooltip("Distance in meters that the hook must be within the water's surface beneath the hook's hanging point in order for the hook to be considered reeled in.")] private float _reeledInDistance = 10f;
        public float ReeledInDistance { get => _reeledInDistance; private set => _reeledInDistance = value; }

        [Header("Minigame values")]
        [SerializeField, Min(0), Tooltip("Width in pixels of the reel zone during the fishing minigame.")] private float _reelZoneWidth;
        public float ReelZoneWidth { get => _reelZoneWidth; set => _reelZoneWidth = value; }

        [SerializeField, Min(0), Tooltip("Amount of velocity in pixels per frame to add to the reel zone for every frame the input is pressed.")] private float _reelZoneForce;
        public float ReelZoneForce { get => _reelZoneForce; set => _reelZoneForce = value; }

        [SerializeField, Min(0), Tooltip("Maximum velocity in pixels per frame of the reel zone.")] private float _reelZoneMaxVelocity;
        public float ReelZoneMaxVelocity { get => _reelZoneMaxVelocity; set => _reelZoneMaxVelocity = value; }

        [SerializeField, Min(0), Tooltip("Gravity force in pixels per frame to enact on the reel zone.")] private float _reelZoneGravity;
        public float ReelZoneGravity { get => _reelZoneGravity; set => _reelZoneGravity = value; }

        private void OnValidate() {
            if (MinCastStrength > MaxCastStrength) {
                MinCastStrength = MaxCastStrength;
            }
        }
    }
}
