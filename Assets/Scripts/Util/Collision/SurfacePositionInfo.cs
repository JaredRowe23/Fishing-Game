using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

namespace Fishing.Util.Collision {
    public struct SurfacePositionInfo {
        public Vector2 surfacePosition;
        public Vector3 rotationFromFloor;
        public bool positionInsideTerrain;

        public SurfacePositionInfo(Vector2 position, PolygonCollider2D[] colliders) {
            positionInsideTerrain = false;
            for (int i = 0; i < colliders.Length; i++) {
                if (colliders[i].OverlapPoint(position)) {
                    positionInsideTerrain = true;
                    break;
                }
            }

            if (positionInsideTerrain) {
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

                surfacePosition = closestPoint;
                rotationFromFloor = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, (position - surfacePosition) * -1));
            }
            else {
                ClosestPointInfo closestPointInfo = CollisionDetection.ClosestPointFromOutside(position, colliders);
                surfacePosition = closestPointInfo.position;
                rotationFromFloor = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, position - surfacePosition));
            }
        }
    }
}