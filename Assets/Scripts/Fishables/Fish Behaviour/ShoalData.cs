using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.Fishables.Fish
{
    public struct ShoalData
    {
        public readonly Vector2 thisPos;
        public readonly Vector2 thisForward;
        public readonly float separationAngle;
        public readonly float separationMaxDistance;
        public readonly float separationMaxCloseDistance;

        public Vector2 shoalmatePos;

        public float closestObstacleDistance;

        public float desiredAngle;

        public ShoalData(Vector2 _thisPos, Vector2 _thisForward, float _separationAngle, float _separationMaxDistance, float _separationMaxCloseDistance)
        {
            thisPos = _thisPos;
            thisForward = _thisForward;
            separationAngle = _separationAngle;
            separationMaxDistance = _separationMaxDistance;
            separationMaxCloseDistance = _separationMaxCloseDistance;
            shoalmatePos = Vector2.zero;
            closestObstacleDistance = 0f;
            desiredAngle = 0f;
        }

        public void FixedUpdate()
        {
            float _distanceToShoalmate = Vector2.Distance(thisPos, shoalmatePos);
            if (_distanceToShoalmate == 0) return;
            if (closestObstacleDistance != 0 && _distanceToShoalmate > closestObstacleDistance) return;
            if (_distanceToShoalmate > separationMaxDistance) return;

            float _angleToShoalmate = Vector2.SignedAngle(thisForward, thisPos - shoalmatePos);
            if (Mathf.Abs(_angleToShoalmate) >= separationAngle && _distanceToShoalmate >= separationMaxCloseDistance) return;

            desiredAngle = _angleToShoalmate <= 0 ? -1 : 1;
            desiredAngle *= 1 - Mathf.InverseLerp(0, separationMaxDistance, Mathf.Clamp(_distanceToShoalmate, 0, separationMaxDistance));
            closestObstacleDistance = _distanceToShoalmate;
        }
    }
}
