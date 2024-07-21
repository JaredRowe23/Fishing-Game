using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.Util
{
    public static class Utilities
    {
        public static ClosestPointInfo ClosestPointFromColliders(Vector2 _position, PolygonCollider2D[] _colliders)
        {
            if (_colliders.Length <= 0)
            {
                Debug.LogWarning("ClosestPointFromColliders called with no provided colliders! Returning empty ClosestPointInfo");
                return new ClosestPointInfo(Vector2.zero, 0, null);
            }

            Vector2 _initialPoint = _colliders[0].ClosestPoint(_position);
            float _initialPointDistance = Vector2.Distance(_position, _initialPoint);
            ClosestPointInfo _closestPoint = new ClosestPointInfo(_initialPoint, _initialPointDistance, _colliders[0]);

            for (int i = 1; i < _colliders.Length; i++)
            {
                Vector2 _pointToEvaluate = _colliders[i].ClosestPoint(_position);
                float _distanceToEvaluate = Vector2.Distance(_position, _pointToEvaluate);

                if (_distanceToEvaluate >= _closestPoint.distance) continue;

                _closestPoint = new ClosestPointInfo(_pointToEvaluate, _distanceToEvaluate, _colliders[i]);
            }

            return _closestPoint;
        }

    }

    public struct ClosestPointInfo
    {
        public float distance;
        public Vector2 position;
        public Collider2D collider;

        public ClosestPointInfo(Vector2 _position, float _distance, Collider2D _collider)
        {
            position = _position;
            distance = _distance;
            collider = _collider;
        }
    }
}
