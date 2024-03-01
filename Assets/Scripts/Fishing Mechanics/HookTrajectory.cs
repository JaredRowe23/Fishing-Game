using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.FishingMechanics
{
    public class HookTrajectory : MonoBehaviour
    {
        private LineRenderer lineRenderer;
        private PowerAndAngle powerAngle;

        [SerializeField] private float trajectoryStep = 0.1f;
        [SerializeField] private int trajectoryMaxSteps = 20;

        private List<Vector2> trajectoryPoints;

        private RodBehaviour equippedRod;

        private void Awake()
        {
            powerAngle = PowerAndAngle.instance;
        }

        private void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            if (!powerAngle.GetCharging())
            {
                lineRenderer.enabled = false;
                return;
            }

            lineRenderer.enabled = true;

            equippedRod = RodManager.instance.equippedRod;

            trajectoryPoints = SimulateTrajectory();
            lineRenderer.positionCount = trajectoryPoints.Count;
            for (int i = 0; i < trajectoryPoints.Count; i++)
            {
                lineRenderer.SetPosition(i, trajectoryPoints[i]);
            }

            float width = lineRenderer.startWidth;
            lineRenderer.material.mainTextureScale = new Vector2(1f / width, 1.0f);
        }

        private List<Vector2> SimulateTrajectory()
        {
            trajectoryPoints = new List<Vector2>();

            Vector2 _launchPos = transform.parent.position;
            float _mass = equippedRod.GetHook().GetComponent<Rigidbody2D>().mass;
            float _force = Mathf.Lerp(equippedRod.scriptable.minCastStrength, equippedRod.scriptable.maxCastStrength, powerAngle.GetCharge());
            float _angle = 90f - powerAngle.GetAngle();
            Vector2 _directionVector = new Vector2(Mathf.Sin(_angle * Mathf.Deg2Rad), Mathf.Cos(_angle * Mathf.Deg2Rad));

            float _vel = _force / _mass * Time.fixedDeltaTime;

            for (int i = 0; i < trajectoryMaxSteps; i++)
            {
                Vector2 _calculatedPosition = _launchPos + _directionVector * _vel * i * trajectoryStep;
                _calculatedPosition.y += Physics2D.gravity.y / 2 * Mathf.Pow(i * trajectoryStep, 2);

                trajectoryPoints.Add(_calculatedPosition);

                if (_calculatedPosition.y <= 0) break;
            }

            return trajectoryPoints;
        }
    }
}
