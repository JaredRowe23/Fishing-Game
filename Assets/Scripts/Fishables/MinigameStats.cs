using UnityEngine;

namespace Fishing.Fishables {
    public class MinigameStats : MonoBehaviour {
        [SerializeField, Min(0), Tooltip("Strength of fish. Correlates to fishing rod strength.")] private float _minigameStrength = 15f;
        public float MinigameStrength { get => _minigameStrength; private set { } }

        [SerializeField, Min(0), Tooltip("Base distance in pixels the fish icon will move during the minigame.")] private float _minigameMoveDistance = 750f; // TODO: Move this over to a percentage/normalized based distance.
        public float MinigameMoveDistance { get => _minigameMoveDistance; private set { } }

        [SerializeField, Min(0), Tooltip("Amount of variance in MinigameMoveDistance.")] private float _minigameMoveDistanceVariance = 250f;
        public float MinigameMoveDistanceVariance { get => _minigameMoveDistanceVariance; private set { } }

        [SerializeField, Min(0), Tooltip("Base amount of time in seconds in between fish icon movements while the fish is swimming.")] private float _minigameMoveTime = 3f;
        public float MinigameMoveTime { get => _minigameMoveTime; private set { } }

        [SerializeField, Min(0), Tooltip("Amount of variance in MinigameMoveTime.")] private float _minigameMoveTimeVariance = 1f;
        public float MinigameMoveTimeVariance { get => _minigameMoveTimeVariance; private set { } }

        [SerializeField, Min(0), Tooltip("Speed in m/s the fish swims away from the player at when its icon is not in the green zone.")] private float _minigameSwimSpeed = 2f;
        public float MinigameSwimSpeed { get => _minigameSwimSpeed; private set { } }

        [SerializeField, Min(0), Tooltip("Base amount of time in seconds the fish attempts to swims for.")] private float _minigameSwimTime = 10f;
        public float MinigameSwimTime { get => _minigameSwimTime; private set { } }

        [SerializeField, Min(0), Tooltip("Amount of variance in MinigameSwimTime.")] private float _minigameSwimTimeVariance = 1f;
        public float MinigameSwimTimeVariance { get => _minigameSwimTimeVariance; private set { } }

        [SerializeField, Min(0), Tooltip("Base amount of time in seconds the fish rests for, causing no move actions for a period.")] private float _minigameRestTime = 5f;
        public float MinigameRestTime { get => _minigameRestTime; private set { } }

        [SerializeField, Min(0), Tooltip("Amount of variance in MinigameRestTime.")] private float _minigameRestTimeVariance = 1f;
        public float MinigameRestTimeVariance { get => _minigameRestTimeVariance; private set { } }
        public float MinigameDifficulty => _fishable.Value / _fishable.FishableScriptable.BaseValue;

        private Fishable _fishable;

        private void Awake() {
            _fishable = GetComponent<Fishable>();
        }
    }
}
