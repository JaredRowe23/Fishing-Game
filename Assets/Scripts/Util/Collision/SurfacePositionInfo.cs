using System.Collections.Generic;
using UnityEngine;

namespace Fishing.Util.Collision {
    public struct SurfacePositionInfo {
        private Vector2 _surfacePosition;
        public Vector2 SurfacePosition { get => _surfacePosition; set => _surfacePosition = value; }
        private Vector3 _rotationFromFloor;
        public Vector3 RotationFromFloor { get => _rotationFromFloor; private set => _rotationFromFloor = value; }
        private bool _positionInsideTerrain;
        public bool PositionInsideTerrain { get => _positionInsideTerrain; private set => _positionInsideTerrain = value; }

        public SurfacePositionInfo(Vector2 position, PolygonCollider2D[] colliders) {
            _positionInsideTerrain = false;
            for (int i = 0; i < colliders.Length; i++) {
                if (colliders[i].OverlapPoint(position)) {
                    _positionInsideTerrain = true;
                    break;
                }
            }

            if (_positionInsideTerrain) {
                List<Vector2> colliderCLosestPoints = new List<Vector2>();
                for (int i = 0; i < colliders.Length; i++) {
                    Vector2 closestPointOnCollider = CollisionDetection.ClosestPointFromInsideCollider2D(position, colliders[i]);
                    if (closestPointOnCollider != position) {
                        colliderCLosestPoints.Add(closestPointOnCollider);
                    }
                }

                Vector2 closestPoint = Vector2.positiveInfinity;
                float closestPointDistance = float.PositiveInfinity;
                for (int i = 0; i < colliderCLosestPoints.Count; i++) {
                    float distance = Vector2.Distance(position, colliderCLosestPoints[i]);
                    if (closestPointDistance < distance) {
                        continue;
                    }
                    closestPoint = colliderCLosestPoints[i];
                    closestPointDistance = distance;
                }

                _surfacePosition = closestPoint;
                _rotationFromFloor = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, (position - _surfacePosition) * -1));
            }
            else {
                ClosestPointInfo closestPointInfo = CollisionDetection.ClosestPointFromOutside(position, colliders);
                _surfacePosition = closestPointInfo.Position;
                _rotationFromFloor = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, position - _surfacePosition));
            }
        }
    }
}