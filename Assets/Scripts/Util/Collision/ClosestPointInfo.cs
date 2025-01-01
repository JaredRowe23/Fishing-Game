using UnityEngine;

namespace Fishing.Util.Collision {
    public struct ClosestPointInfo {
        private float _distance;
        public float Distance { get => _distance; private set => _distance = value; }
        private Vector2 _position;
        public Vector2 Position { get => _position; private set => _position = value; }
        private Collider2D _collider;
        public Collider2D Collider { get => _collider; private set => _collider = value; }

        public ClosestPointInfo(Vector2 position, float distance, Collider2D collider) {
            _position = position;
            _distance = distance;
            _collider = collider;
        }
    }
}