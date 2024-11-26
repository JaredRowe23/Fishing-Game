using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Fishing.Util
{
    public static class Utilities
    {
        /// <summary>
        /// Oscillates a value between two other values and returns an OscillateInfo object that contains the new value and what the new target direction should be.
        /// </summary>
        /// <param name="minValue">Minimum value to lerp</param>
        /// <param name="maxValue">Maximum value to lerp</param>
        /// <param name="currentValue">Current value to lerp</param>
        /// <param name="oscillationSpeed">Speed to lerp, in leu of having Time.deltaTime or frequency set in the inspector</param>
        /// <param name="target">Current target value to oscillate towards</param>
        /// <returns>OscillateInfo</returns>
        public static OscillateInfo OscillateFloat(float minValue, float maxValue, float currentValue, float oscillationSpeed, float target)
        {
            float _valueDelta = Mathf.Lerp(minValue, maxValue, oscillationSpeed) - minValue;
            currentValue = target == maxValue ? currentValue + _valueDelta : currentValue - _valueDelta;

            if (target == maxValue && currentValue >= maxValue)
            {
                currentValue = maxValue;
                target = minValue;
            }
            else if (target == minValue && currentValue <= minValue)
            {
                currentValue = minValue;
                target = maxValue;
            }

            OscillateInfo _info = new OscillateInfo(currentValue, target);
            return _info;
        }
        /// <summary>
        /// Returns given color with it's transparency set to a given value.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="transparency"></param>
        /// <returns>Color</returns>
        public static Color SetTransparency(Color color, float transparency) => new Color(color.r, color.g, color.b, transparency);

        /// <summary>
        /// Converts angle to a directional vector.
        /// </summary>
        /// <param name="angle"></param>
        /// <returns>Vector2 direction</returns>
        public static Vector2 AngleToVector(float angle) => new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));


        /// <summary>
        /// Gives the direction a transform would have to rotate to face towards a target, in the form of an int. Meant to be multiplied with rotation needs.
        /// </summary>
        /// <param name="transform">Transform to be rotated</param>
        /// <param name="target">Target position to rotate transform towards</param>
        /// <returns>0 if already facing target, 1 is counterclockwise rotation is needed, -1 if clockwise rotation is needed</returns>
        public static int DirectionFromTransformToTarget(Transform transform, Vector2 target)
        {
            float _angleToTarget = Vector2.SignedAngle(Vector2.up, target - (Vector2)transform.position);
            float _angleDelta = Mathf.DeltaAngle(transform.rotation.eulerAngles.z, _angleToTarget);
            return _angleDelta == 0 ? 0 : (_angleDelta > 0 ? 1 : -1);
        }

        /// <summary>
        /// Returns the Vector2 for a given Transform scaled in global space (unparented). Set the Transform's scale to the return value.
        /// </summary>
        /// <param name="transform">Transform to be scaled</param>
        /// <param name="globalScale">Desired scale</param>
        /// <returns>Vector2 scale</returns>
        public static Vector2 SetGlobalScale(Transform transform, float globalScale)
        {
            Transform parent = transform.parent;
            transform.SetParent(null);
            transform.localScale = Vector2.one * globalScale;
            transform.SetParent(parent);
            return transform.localScale;
        }
        /// <summary>
        /// Returns a signed angle from an unsigned angle (ex. 270 returns -90)
        /// </summary>
        /// <param name="angle"></param>
        /// <returns>float from -180 to 180</returns>
        public static float UnsignedToSignedAngle(float angle)
        {
            if (angle > 180) angle = -180 + angle % 180;
            return angle;
        }

        /// <summary>
        /// Returns struct with the data on the closest collider, point, and distance from a given position and array of colliders.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="colliders"></param>
        /// <returns></returns>
        public static ClosestPointInfo ClosestPointFromColliders(Vector2 position, PolygonCollider2D[] colliders)
        {
            if (colliders.Length <= 0)
            {
                Debug.LogWarning("ClosestPointFromColliders called with no provided colliders! Returning empty ClosestPointInfo");
                return new ClosestPointInfo(Vector2.zero, 0, null);
            }

            Vector2 initialPoint = colliders[0].ClosestPoint(position);
            float initialPointDistance = Vector2.Distance(position, initialPoint);
            ClosestPointInfo _closestPoint = new ClosestPointInfo(initialPoint, initialPointDistance, colliders[0]);

            for (int i = 1; i < colliders.Length; i++)
            {
                Vector2 pointToEvaluate = colliders[i].ClosestPoint(position);
                float distanceToEvaluate = Vector2.Distance(position, pointToEvaluate);

                if (distanceToEvaluate >= _closestPoint.distance) continue;

                _closestPoint = new ClosestPointInfo(pointToEvaluate, distanceToEvaluate, colliders[i]);
            }

            return _closestPoint;
        }
        public static bool IsWithinAngleOfDirection(Vector2 sourcePosition, Vector2 targetPosition, Vector2 direction, float angle) {
            float dot = Vector2.Dot(direction, Vector3.Normalize(targetPosition - sourcePosition));
            float dotAngle = Mathf.Acos(dot) * 180 * 0.3183098861928886f;
            if (dotAngle < angle) return true;
            return false;
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

    public struct OscillateInfo
    {
        public float value;
        public float newTarget;

        public OscillateInfo(float _value, float _newTarget)
        {
            value = _value;
            newTarget = _newTarget;
        }
    }
}
