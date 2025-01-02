using UnityEngine;

namespace Fishing.Util {
    public static class Utilities {
        public static void SwapActive(GameObject activeGO, GameObject inactiveGO) {
            activeGO.SetActive(true);
            inactiveGO.SetActive(false);
        }

        /// <summary>
        /// Returns given color with it's transparency set to a given value.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="transparency"></param>
        /// <returns>Color</returns>
        public static Color SetTransparency(Color color, float transparency) {
            return new Color(color.r, color.g, color.b, transparency);
        }

        /// <summary>
        /// Gives the direction a transform would have to rotate to face towards a target, in the form of an int. Meant to be multiplied with rotation needs.
        /// </summary>
        /// <param name="transform">Transform to be rotated</param>
        /// <param name="target">Target position to rotate transform towards</param>
        /// <returns>0 if already facing target, 1 is counterclockwise rotation is needed, -1 if clockwise rotation is needed</returns>
        public static int DirectionFromTransformToTarget(Transform transform, Vector2 target) {
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
        public static Vector2 SetGlobalScale(Transform transform, float globalScale) {
            Transform parent = transform.parent;
            transform.SetParent(null);
            transform.localScale = Vector2.one * globalScale;
            transform.SetParent(parent);
            return transform.localScale;
        }

        /// <summary>
        /// Takes a turn direction value of -1 or 1 and weighs it based on how close a transform is pointed towards the given angle
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="angle"></param>
        /// <param name="objectTransform"></param>
        /// <returns></returns>
        public static float WeighDirection(float direction, float angle) {
            float rads = (Mathf.Abs(angle) + 90f) * Mathf.Deg2Rad; // Converting angle to Vector
                                                                   // We get the absolute value here because the direction of the angle doesn't matter for now and will be corrected using the direction variable
                                                                   // We add 90 as part of the conversion, since Unity's rotation 0 is up whereas radians 0 is right
            Vector3 angleVector = new Vector3(Mathf.Cos(rads), Mathf.Sin(rads), 0f);
            float angleDot = Vector2.Dot(Vector2.up, angleVector); // Get Dot product and clamp values 0 to 1. Using Vector2.up since the angleVector should be relative to facing direction.
            float directionWeight = 1f - Mathf.Clamp(angleDot, 0, 1);
            return direction * directionWeight; // direction value of -1 or 1 is now weighed
        }
    }
}
