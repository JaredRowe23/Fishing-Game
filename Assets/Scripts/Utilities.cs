using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Fishing.Util
{
    public static class Utilities
    {
        /// <summary>
        /// Gives the direction a transform would have to rotate to face towards a target, in the form of an int. Meant to be multiplied with rotation needs.
        /// </summary>
        /// <param name="_transform">Transform to be rotated</param>
        /// <param name="_target">Target position to rotate _transform towards</param>
        /// <returns>0 if already facing _target, 1 is counterclockwise rotation is needed, -1 if clockwise rotation is needed</returns>
        public static int DirectionFromTransformToTarget(Transform _transform, Vector2 _target)
        {
            float _angleToTarget = Vector2.SignedAngle(Vector2.up, _target - (Vector2)_transform.position);
            float _angleDelta = Mathf.DeltaAngle(_transform.rotation.eulerAngles.z, _angleToTarget);
            return _angleDelta == 0 ? 0 : (_angleDelta > 0 ? 1 : -1);
        }

        /// <summary>
        /// Returns the Vector2 for a given Transform scaled in global space (unparented). Set the Transform's scale to the return value.
        /// </summary>
        /// <param name="_transform">Transform to be scaled</param>
        /// <param name="_globalScale">Desired scale</param>
        /// <returns>Vector2 scale</returns>
        public static Vector2 SetGlobalScale(Transform _transform, float _globalScale)
        {
            Transform _parent = _transform.parent;
            _transform.SetParent(null);
            _transform.localScale = Vector2.one * _globalScale;
            _transform.SetParent(_parent);
            return _transform.localScale;
        }
        /// <summary>
        /// Returns a signed angle from an unsigned angle (ex. 270 returns -90)
        /// </summary>
        /// <param name="_angle"></param>
        /// <returns>float from -180 to 180</returns>
        public static float UnsignedToSignedAngle(float _angle)
        {
            if (_angle > 180) _angle = -180 + _angle % 180;
            return _angle;
        }

        /// <summary>
        /// Returns struct with the data on the closest collider, point, and distance from a given position and array of colliders.
        /// </summary>
        /// <param name="_position"></param>
        /// <param name="_colliders"></param>
        /// <returns></returns>
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
