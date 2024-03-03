using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.FishingMechanics
{
    public class HookTrajectory : MonoBehaviour
    {
        private PowerAndAngle powerAngle;

        [SerializeField] private float trajectoryStep = 0.1f;
        [SerializeField] private int trajectoryMaxSteps = 20;

        [SerializeField] private LineRenderer trajectoryLineRenderer;
        [SerializeField] private LineRenderer minTrajectoryLineRenderer;
        [SerializeField] private LineRenderer maxTrajectoryLineRenderer;

        [SerializeField] private Material trajectoryMat;
        [SerializeField] private Material minTrajectoryMat;
        [SerializeField] private Material maxTrajectoryMat;

        private List<Vector2> trajectoryPoints;
        private List<Vector2> minimumTrajectoryPoints;
        private List<Vector2> maximumTrajectoryPoints;

        private RodBehaviour equippedRod;

        private void Awake()
        {
            powerAngle = PowerAndAngle.instance;

            trajectoryLineRenderer.material = trajectoryMat;
            minTrajectoryLineRenderer.material = minTrajectoryMat;
            maxTrajectoryLineRenderer.material = maxTrajectoryMat;
        }

        private void Update()
        {
            equippedRod = RodManager.instance.equippedRod;

            if ((!powerAngle.GetCharging() && !powerAngle.GetAngling()) || !equippedRod.GetHook().IsInStartCastPosition())
            {
                trajectoryLineRenderer.enabled = false;
                minTrajectoryLineRenderer.enabled = false;
                maxTrajectoryLineRenderer.enabled = false;
                return;
            }

            minimumTrajectoryPoints = GetTrajectoryPoints(equippedRod.scriptable.minCastStrength);
            maximumTrajectoryPoints = GetTrajectoryPoints(equippedRod.scriptable.maxCastStrength);

            PlotTrajectory(minimumTrajectoryPoints, minTrajectoryLineRenderer);
            PlotTrajectory(maximumTrajectoryPoints, maxTrajectoryLineRenderer);

            if (!powerAngle.GetCharging()) return;

            trajectoryPoints = GetTrajectoryPoints(Mathf.Lerp(equippedRod.scriptable.minCastStrength, equippedRod.scriptable.maxCastStrength, powerAngle.GetCharge()));
            PlotTrajectory(trajectoryPoints, trajectoryLineRenderer);
        }

        private void PlotTrajectory(List<Vector2> _points, LineRenderer _lineRenderer)
        {
            _lineRenderer.enabled = true;

            _lineRenderer.positionCount = _points.Count;

            for (int i = 0; i < _points.Count; i++)
            {
                _lineRenderer.SetPosition(i, _points[i]);
            }

            float width = _lineRenderer.startWidth;
            _lineRenderer.material.mainTextureScale = new Vector2(1f / width, 1.0f);
        }

        private List<Vector2> GetTrajectoryPoints(float _force)
        {
            trajectoryPoints = new List<Vector2>();

            Vector2 _launchPos = transform.parent.position;
            float _mass = equippedRod.GetHook().GetComponent<Rigidbody2D>().mass;
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
