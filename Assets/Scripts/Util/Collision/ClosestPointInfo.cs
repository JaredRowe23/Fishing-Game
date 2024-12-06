using UnityEngine;

namespace Fishing.Util.Collision {
    public struct ClosestPointInfo {
        public float distance;
        public Vector2 position;
        public Collider2D collider;

        public ClosestPointInfo(Vector2 _position, float _distance, Collider2D _collider) {
            position = _position;
            distance = _distance;
            collider = _collider;
        }
    }
}