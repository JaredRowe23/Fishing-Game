using UnityEngine;

namespace Fishing.Fishables.Fish {
    [CreateAssetMenu(fileName = "New Fishable", menuName = "Fishable")]
    public class FishableScriptable : ScriptableObject {
        [Header("General")]
        [SerializeField, Tooltip("Name of this item.")] private string _itemName;
        public string ItemName { get => _itemName; private set { } }

        [SerializeField, Tooltip("Description of this item.")] private string _itemDescription;
        public string ItemDescription { get => _itemDescription; private set { } }

        [SerializeField, Min(0), Tooltip("Base value for a \"normal\" sized item of this type.")] private float _baseValue;
        public float BaseValue { get => _baseValue; private set { } }

        [SerializeField, Min(0), Tooltip("Maximum weight this item can reach.")] private float _weightMax;
        public float WeightMax { get => _weightMax; private set { } }

        [SerializeField, Min(0), Tooltip("Minimum weight this item can reach.")] private float _weightMin;
        public float WeightMin { get => _weightMin; private set { } }

        [SerializeField, Min(0), Tooltip("Maximum length this item can reach.")] private float _lengthMax;
        public float LengthMax { get => _lengthMax; private set { } }

        [SerializeField, Min(0), Tooltip("Minimum length this item can reach.")] private float _lengthMin;
        public float LengthMin { get => _lengthMin; private set { } }

        [Header("Minigame Stats")]
        [SerializeField, Min(0), Tooltip("Strength of fish. Correlates to fishing rod strength.")] private float _strength = 15f;
        public float Strength { get => _strength; private set { } }

        [SerializeField, Min(0), Tooltip("Base distance in pixels the fish icon will move during the minigame.")] private float _baseMoveDistance = 750f; // TODO: Move this over to a percentage/normalized based distance.
        public float BaseMoveDistance { get => _baseMoveDistance; private set { } }

        [SerializeField, Min(0), Tooltip("Amount of variance in MinigameMoveDistance.")] private float _moveDistanceVariance = 250f;
        public float MoveDistanceVariance { get => _moveDistanceVariance; private set { } }

        [SerializeField, Min(0), Tooltip("Base amount of time in seconds in between fish icon movements while the fish is swimming.")] private float _baseMoveTime = 3f;
        public float BaseMoveTime { get => _baseMoveTime; private set { } }

        [SerializeField, Min(0), Tooltip("Amount of variance in MinigameMoveTime.")] private float _moveTimeVariance = 1f;
        public float MoveTimeVariance { get => _moveTimeVariance; private set { } }

        [SerializeField, Min(0), Tooltip("Speed in m/s the fish swims away from the player at when its icon is not in the green zone.")] private float _swimSpeed = 2f;
        public float SwimSpeed { get => _swimSpeed; private set { } }

        [SerializeField, Min(0), Tooltip("Base amount of time in seconds the fish attempts to swims for.")] private float _baseSwimTime = 10f;
        public float BaseSwimTime { get => _baseSwimTime; private set { } }

        [SerializeField, Min(0), Tooltip("Amount of variance in MinigameSwimTime.")] private float _swimTimeVariance = 1f;
        public float SwimTimeVariance { get => _swimTimeVariance; private set { } }

        [SerializeField, Min(0), Tooltip("Base amount of time in seconds the fish rests for, causing no move actions for a period.")] private float _baseRestTime = 5f;
        public float BaseRestTime { get => _baseRestTime; private set { } }

        [SerializeField, Min(0), Tooltip("Amount of variance in MinigameRestTime.")] private float _restTimeVariance = 1f;
        public float RestTimeVariance { get => _restTimeVariance; private set { } }

        private void OnValidate() {
            if (WeightMin > WeightMax) {
                WeightMin = WeightMax;
            }
            if (LengthMin > LengthMax) {
                LengthMin = LengthMax;
            }

            if (MoveDistanceVariance > BaseMoveDistance) {
                MoveDistanceVariance = BaseMoveDistance;
            }
            if (MoveTimeVariance > BaseMoveTime) {
                MoveTimeVariance = BaseMoveTime;
            }
            if (SwimTimeVariance > BaseSwimTime) {
                SwimTimeVariance = BaseSwimTime;
            }
            if (RestTimeVariance > BaseRestTime) {
                RestTimeVariance = BaseRestTime;
            }
        }

        public float GetRandomWeightValue() {
            return Random.Range(WeightMin, WeightMax);
        }
        public float GetRandomLengthValue() {
            return Random.Range(LengthMin, LengthMax);
        }

        public float GetRandomMoveDistance() {
            return Random.Range(-BaseMoveDistance - MoveDistanceVariance, BaseMoveDistance + MoveDistanceVariance);
        }
        public float GetRandomMoveTime() {
            return BaseMoveTime + Random.Range(-MoveTimeVariance, MoveTimeVariance);
        }
        public float GetRandomSwimTime() {
            return BaseSwimTime + Random.Range(-SwimTimeVariance, SwimTimeVariance);
        }
        public float GetRandomRestTime() {
            return BaseRestTime + Random.Range(-RestTimeVariance, RestTimeVariance);
        }
    }
}
