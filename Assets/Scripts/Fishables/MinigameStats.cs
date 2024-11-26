using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.Fishables
{
    public class MinigameStats : MonoBehaviour {
        [SerializeField] private float _minigameStrength;
        public float MinigameStrength { get => _minigameStrength; private set { } }

        [SerializeField] private float _minigameMoveDistance;
        public float MinigameMoveDistance { get => _minigameMoveDistance; private set { } }

        [SerializeField] private float _minigameMoveDistanceVariance;
        public float MinigameMoveDistanceVariance { get => _minigameMoveDistanceVariance; private set { } }

        [SerializeField] private float _minigameMoveTime;
        public float MinigameMoveTime { get => _minigameMoveTime; private set { } }

        [SerializeField] private float _minigameMoveTimeVariance;
        public float MinigameMoveTimeVariance { get => _minigameMoveTimeVariance; private set { } }

        [SerializeField] private float _minigameSwimSpeed;
        public float MinigameSwimSpeed { get => _minigameSwimSpeed; private set { } }

        [SerializeField] private float _minigameSwimTime;
        public float MinigameSwimTime { get => _minigameSwimTime; private set { } }

        [SerializeField] private float _minigameSwimTimeVariance;
        public float MinigameSwimTimeVariance { get => _minigameSwimTimeVariance; private set { } }

        [SerializeField] private float _minigameRestTime;
        public float MinigameRestTime { get => _minigameRestTime; private set { } }

        [SerializeField] private float _minigameRestTimeVariance;
        public float MinigameRestTimeVariance { get => _minigameRestTimeVariance; private set { } }
        public float MinigameDifficulty => fishable.Value / fishable.BaseValue;

        private Fishable fishable;

        private void Awake() {
            fishable = GetComponent<Fishable>();
        }
    }
}
