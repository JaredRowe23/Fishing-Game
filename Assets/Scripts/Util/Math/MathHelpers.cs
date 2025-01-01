using UnityEngine;

namespace Fishing.Util.Math {
    public static class MathHelpers {
        /// <summary>
        /// Returns a signed angle from an unsigned angle (ex. 270 returns -90)
        /// </summary>
        /// <param name="angle"></param>
        /// <returns>float from -180 to 180</returns>
        public static float UnsignedToSignedAngle(float angle) { // TODO: DOES NOT WORK, FIX THIS! Check to see if an if statement to get this working is less intensive than a Vector2.UnsignedAngle.
            return angle - ((int)(angle / 360) * 360f);
            //angle = angle % 360;
            //if (angle > 180) angle = -180 + angle % 180;
            //return angle;
        }

        /// <summary>
        /// Converts angle to a directional vector.
        /// </summary>
        /// <param name="angle"></param>
        /// <returns>Vector2 direction</returns>
        public static Vector2 AngleToVector(float angle) {
            return new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
        }

        /// <summary>
        /// Oscillates a value between two other values and returns an OscillateInfo object that contains the new value and what the new target direction should be.
        /// </summary>
        /// <param name="minValue">Minimum value to lerp</param>
        /// <param name="maxValue">Maximum value to lerp</param>
        /// <param name="currentValue">Current value to lerp</param>
        /// <param name="oscillationSpeed">Speed to lerp, in leu of having Time.deltaTime or frequency set in the inspector</param>
        /// <param name="target">Current target value to oscillate towards</param>
        /// <returns>OscillateInfo</returns>
        public static OscillateInfo OscillateFloat(float minValue, float maxValue, float currentValue, float oscillationSpeed, float target) {
            float _valueDelta = Mathf.Lerp(minValue, maxValue, oscillationSpeed) - minValue;
            currentValue = target == maxValue ? currentValue + _valueDelta : currentValue - _valueDelta;

            if (target == maxValue && currentValue >= maxValue) {
                currentValue = maxValue;
                target = minValue;
            }
            else if (target == minValue && currentValue <= minValue) {
                currentValue = minValue;
                target = maxValue;
            }

            OscillateInfo _info = new OscillateInfo(currentValue, target);
            return _info;
        }

        public static bool IsWithinAngleOfDirection(Vector2 sourcePosition, Vector2 targetPosition, Vector2 direction, float angle) {
            float dot = Vector2.Dot(direction, Vector3.Normalize(targetPosition - sourcePosition));
            float dotAngle = Mathf.Acos(dot) * 180 * 0.3183098861928886f;
            if (dotAngle < angle) return true;
            return false;
        }
    }
}