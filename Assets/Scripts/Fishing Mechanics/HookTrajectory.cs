using Fishing.PlayerCamera;
using Fishing.Util.Math;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.FishingMechanics {
    public class HookTrajectory : MonoBehaviour {
        private PowerAndAngle powerAngle;

        [SerializeField, Min(0), Tooltip("Time length in seconds for each trajectory simulation step.")] private float _trajectoryStep = 0.1f;
        [SerializeField, Min(0), Tooltip("How many steps the trajectory simulation should generate.")] private int _trajectoryMaxSteps = 20;

        [SerializeField, Tooltip("Line renderer for the main trajectory projection (what the hook will actually follow)")] private LineRenderer _trajectoryLineRenderer;
        [SerializeField, Tooltip("Line renderer for the minimum trajectory projection (if you choose the lowest power)")] private LineRenderer _minTrajectoryLineRenderer;
        [SerializeField, Tooltip("Line renderer for the maximum trajectory projection (if you choose the highest power)")] private LineRenderer _maxTrajectoryLineRenderer;

        [SerializeField, Tooltip("Material to display the main trajectory projection with.")] private Material _trajectoryMat;
        [SerializeField, Tooltip("Material to display the minimum trajectory projection with.")] private Material _minTrajectoryMat;
        [SerializeField, Tooltip("Material to display the maximum trajectory projection with.")] private Material _maxTrajectoryMat;

        [SerializeField, Range(1f, 2f), Tooltip("How much to zoom the camera out compared to the length of the furthest reaching trajectory. 1 captures the full trajectory, higher values zoom out to give extra space.")] private float _trajectoryCameraZoomMagnitude = 1.25f;
        [SerializeField, Range(0f, 1f), Tooltip("Size of the trajectory dots in relation to the camera zoom.")] private float _trajectoryLineWidthZoomModifier = 0.04f;

        private List<Vector2> _trajectoryPoints;
        private List<Vector2> _minimumTrajectoryPoints;
        private List<Vector2> _maximumTrajectoryPoints;

        private RodBehaviour _equippedRod;
        private CameraBehaviour _camera;

        private void Awake() {
            _trajectoryLineRenderer.material = _trajectoryMat;
            _minTrajectoryLineRenderer.material = _minTrajectoryMat;
            _maxTrajectoryLineRenderer.material = _maxTrajectoryMat;
        }

        private void Start() {
            powerAngle = PowerAndAngle.Instance;
            _camera = CameraBehaviour.Instance;
        }

        private void Update() {
            _equippedRod = RodManager.Instance.EquippedRod;

            if (_equippedRod.Casted || !_equippedRod.Hook.IsInStartCastPosition()) {
                HideLineRenderers();
                return;
            }

            if (powerAngle.IsAngling) {
                GenerateAnglingTrajectories();
            }
            else {
                GeneratePowerTrajectories();
            }
        }

        private void HideLineRenderers() {
            _trajectoryLineRenderer.enabled = false;
            _minTrajectoryLineRenderer.enabled = false;
            _maxTrajectoryLineRenderer.enabled = false;
        }

        private void GenerateAnglingTrajectories() {
            _minimumTrajectoryPoints = GetTrajectoryPoints(_equippedRod.RodScriptable.MinCastStrength, powerAngle.CurrentAngle);
            _maximumTrajectoryPoints = GetTrajectoryPoints(_equippedRod.RodScriptable.MaxCastStrength, powerAngle.CurrentAngle);

            PlotTrajectory(_minimumTrajectoryPoints, _minTrajectoryLineRenderer);
            PlotTrajectory(_maximumTrajectoryPoints, _maxTrajectoryLineRenderer);

            HandleCamera(_equippedRod.RodScriptable.MaxCastAngle);
        }

        private void GeneratePowerTrajectories() {
            _trajectoryPoints = GetTrajectoryPoints(powerAngle.Power, powerAngle.CurrentAngle);
            PlotTrajectory(_trajectoryPoints, _trajectoryLineRenderer);

            HandleCamera(powerAngle.CurrentAngle);
        }

        private void PlotTrajectory(List<Vector2> points, LineRenderer lineRenderer) {
            lineRenderer.enabled = true;
            lineRenderer.positionCount = points.Count;
            for (int i = 0; i < points.Count; i++) {
                lineRenderer.SetPosition(i, points[i]);
            }
            lineRenderer.material.mainTextureScale = new Vector2(1f / lineRenderer.startWidth, 1.0f);
        }

        private List<Vector2> GetTrajectoryPoints(float force, float castAngle) {
            _trajectoryPoints = new List<Vector2>();

            Vector2 launchPos = transform.parent.position;
            float angle = 90f - castAngle;
            Vector2 directionVector = MathHelpers.AngleToVector(angle);

            float mass = _equippedRod.Hook.GetComponent<Rigidbody2D>().mass;
            float velocity = force / mass * Time.fixedDeltaTime;

            for (int i = 0; i < _trajectoryMaxSteps; i++) {
                Vector2 calculatedPosition = launchPos + directionVector * velocity * i * _trajectoryStep;
                calculatedPosition.y += Physics2D.gravity.y * 0.5f * Mathf.Pow(i * _trajectoryStep, 2);

                _trajectoryPoints.Add(calculatedPosition);

                if (calculatedPosition.y <= 0) {
                    break;
                }
            }

            return _trajectoryPoints;
        }

        public void HandleCamera(float angle) {
            List<Vector2> trajectoryPoints = GetTrajectoryPoints(_equippedRod.RodScriptable.MaxCastStrength, angle);

            Vector2 closestPoint = trajectoryPoints[0];
            Vector2 furthestPoint = trajectoryPoints[trajectoryPoints.Count - 1];

            Vector2 newPos = (closestPoint + furthestPoint) * 0.5f;
            _camera.DesiredPosition = newPos;

            float distance = furthestPoint.x - closestPoint.x;
            float zoom = _camera.ViewDistanceToCameraZoom(distance) * _trajectoryCameraZoomMagnitude;

            _camera.LockPlayerControls = true;
            _camera.TempZoom = zoom;
            AdjustWidthToCameraZoom(zoom);
        }

        private void AdjustWidthToCameraZoom(float zoom) {
            _trajectoryLineRenderer.widthMultiplier = zoom * _trajectoryLineWidthZoomModifier;
            _minTrajectoryLineRenderer.widthMultiplier = zoom * _trajectoryLineWidthZoomModifier;
            _maxTrajectoryLineRenderer.widthMultiplier = zoom * _trajectoryLineWidthZoomModifier;
        }
    }
}
