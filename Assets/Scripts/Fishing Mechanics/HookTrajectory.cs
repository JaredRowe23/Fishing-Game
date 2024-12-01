using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.PlayerCamera;
using Fishing.Util;

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

        [Range(1.0f, 2.0f)]
        [SerializeField] private float trajectoryCameraZoomMagnitude = 1.25f;
        [Range(0f, 1.0f)]
        [SerializeField] private float trajectoryLineWidthZoomModifier = 0.04f;

        private List<Vector2> trajectoryPoints;
        private List<Vector2> minimumTrajectoryPoints;
        private List<Vector2> maximumTrajectoryPoints;

        private RodBehaviour equippedRod;
        private CameraBehaviour cam;

        public static HookTrajectory instance;

        private HookTrajectory() => instance = this;

        private void Awake()
        {
            powerAngle = PowerAndAngle.instance;
            cam = CameraBehaviour.Instance;

            trajectoryLineRenderer.material = trajectoryMat;
            minTrajectoryLineRenderer.material = minTrajectoryMat;
            maxTrajectoryLineRenderer.material = maxTrajectoryMat;
        }

        private void Update()
        {
            equippedRod = RodManager.instance.equippedRod;

            if (equippedRod.casted || !equippedRod.GetHook().IsInStartCastPosition())
            {
                HideLineRenderers();
                return;
            }

            if (powerAngle.IsAngling()) GenerateAnglingTrajectories();
            else GeneratePowerTrajectories();
        }

        private void HideLineRenderers()
        {
            trajectoryLineRenderer.enabled = false;
            minTrajectoryLineRenderer.enabled = false;
            maxTrajectoryLineRenderer.enabled = false;
        }

        private void GenerateAnglingTrajectories()
        {
            minimumTrajectoryPoints = GetTrajectoryPoints(equippedRod.scriptable.minCastStrength, powerAngle.GetAngle());
            maximumTrajectoryPoints = GetTrajectoryPoints(equippedRod.scriptable.maxCastStrength, powerAngle.GetAngle());

            PlotTrajectory(minimumTrajectoryPoints, minTrajectoryLineRenderer);
            PlotTrajectory(maximumTrajectoryPoints, maxTrajectoryLineRenderer);

            HandleCamera(equippedRod.scriptable.maxCastAngle);
        }

        private void GeneratePowerTrajectories()
        {
            trajectoryPoints = GetTrajectoryPoints(powerAngle.GetCharge(), powerAngle.GetAngle());
            PlotTrajectory(trajectoryPoints, trajectoryLineRenderer);

            HandleCamera(powerAngle.GetAngle());
        }

        private void PlotTrajectory(List<Vector2> _points, LineRenderer _lineRenderer)
        {
            _lineRenderer.enabled = true;
            _lineRenderer.positionCount = _points.Count;
            for (int i = 0; i < _points.Count; i++) _lineRenderer.SetPosition(i, _points[i]);
            _lineRenderer.material.mainTextureScale = new Vector2(1f / _lineRenderer.startWidth, 1.0f);
        }

        private List<Vector2> GetTrajectoryPoints(float _force, float _castAngle)
        {
            trajectoryPoints = new List<Vector2>();

            Vector2 _launchPos = transform.parent.position;
            float _angle = 90f - _castAngle;
            Vector2 _directionVector = Utilities.AngleToVector(_angle);

            float _mass = equippedRod.GetHook().GetComponent<Rigidbody2D>().mass;
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

        public void HandleCamera(float _angle)
        {
            List<Vector2> _trajectoryPoints = GetTrajectoryPoints(equippedRod.scriptable.maxCastStrength, _angle);

            Vector2 _closestPoint = _trajectoryPoints[0];
            Vector2 _furthestPoint = _trajectoryPoints[_trajectoryPoints.Count - 1];

            Vector2 _newPos = (_closestPoint + _furthestPoint) * 0.5f;
            cam.DesiredPosition = _newPos;

            float _distance = _furthestPoint.x - _closestPoint.x;
            float _zoom = cam.ViewDistanceToCameraZoom(_distance) * trajectoryCameraZoomMagnitude;

            cam.LockPlayerControls = true;
            cam.TempZoom = _zoom;
            AdjustWidthToCameraZoom(_zoom);
        }

        private void AdjustWidthToCameraZoom(float _zoom)
        {
            trajectoryLineRenderer.widthMultiplier = _zoom * trajectoryLineWidthZoomModifier;
            minTrajectoryLineRenderer.widthMultiplier = _zoom * trajectoryLineWidthZoomModifier;
            maxTrajectoryLineRenderer.widthMultiplier = _zoom * trajectoryLineWidthZoomModifier;
        }
    }
}
