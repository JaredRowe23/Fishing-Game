using Fishing.FishingMechanics;
using Fishing.Util;
using UnityEngine;

namespace Fishing.Fishables.Fish {
    [RequireComponent(typeof(FishMovement))]
    public class Shoal : MonoBehaviour, IMovement, IEdible {
        [Header("Turn Factor Weights")]
        [SerializeField, Range(0, 1), Tooltip("Weight on how much this shoal should prioritize avoidance.")] private float _avoidanceDirWeight = 1f;
        [SerializeField, Range(0, 1), Tooltip("Weight on how much this shoal should prioritize cohesion.")] private float _cohesionDirWeight = 1f;
        [SerializeField, Range(0, 1), Tooltip("Weight on how much this shoal should prioritize alignment.")] private float _alignmentDirWeight = 1f;
        [SerializeField, Range(0, 1), Tooltip("Weight on how much this shoal should prioritize moving towards the target.")] private float _targetPosWeight = 1f;

        [Header("Avoidance")]
        [SerializeField, Range(0, 360), Tooltip("Angle from this shoal's front a shoal must be within for avoidance to begin")] private float _avoidanceAngle;
        [SerializeField, Min(0), Tooltip("Distance for avoidance to begin.")] private float _avoidanceMaxDistance;
        [SerializeField, Min(0), Tooltip("Distance for this shoal to ignore other turning factors other than avoidance.")] private float _avoidanceMaxCloseDistance;

        private float _avoidanceDir = 0;
        private float _cohesionDir = 0;
        private float _alignmentDir = 0;

        private float _avoidanceTurn = 0;
        private float _cohesionTurn = 0;
        private float _alignmentTurn = 0;
        private float _targetTurn = 0;

        private SpawnZone _spawn;

        [Header("Gizmos")]
        #region
        [SerializeField] private bool _drawCohesionGizmo = false;
        [SerializeField] private Color _cohesionGizmoColor = Color.white;
        [SerializeField] private bool _drawAlignmentGizmo = false;
        [SerializeField] private Color _alignmentGizmoColor = Color.white;
        [SerializeField] private bool _drawAvoidanceGizmo = false;
        [SerializeField] private Color _avoidanceGizmoColor = Color.white;

        [SerializeField] private bool _drawDirectionGizmos = false;
        [SerializeField, Tooltip("Length of the line gizmo depicting each turn factor")] private float _directionGizmoLength = 1f;
        [SerializeField, Tooltip("Distance below the shoal that turn factor gizmos should be drawn.")] private float _directionSpace = 0.5f;
        [SerializeField, Tooltip("Padding between each turn factor gizmo.")] private float _directionPadding = 0.1f;
        [SerializeField] private Color _targetGizmoColor = Color.white;
        #endregion

        private FishMovement _movement;
        private FishSchoolBehaviour _school;

        private void OnValidate() {
            ValidateMaxCloseDistance();
            ValidateWeights();
        }

        private void ValidateMaxCloseDistance() {
            if (_avoidanceMaxCloseDistance > _avoidanceMaxDistance) {
                _avoidanceMaxCloseDistance = _avoidanceMaxDistance;
            }
        }

        private void ValidateWeights() {
            float totalWeight = _avoidanceDirWeight + _cohesionDirWeight + _alignmentDirWeight + _targetPosWeight;
            if (totalWeight <= 0f) {
                _avoidanceDirWeight = 0.25f;
                _cohesionDirWeight = 0.25f;
                _alignmentDirWeight = 0.25f;
                _targetPosWeight = 0.25f;
                return;
            }
            _avoidanceDirWeight = _avoidanceDirWeight / totalWeight;
            _cohesionDirWeight = _cohesionDirWeight / totalWeight;
            _alignmentDirWeight = _alignmentDirWeight / totalWeight;
            _targetPosWeight = _targetPosWeight / totalWeight;
        }

        private void Awake() {
            _movement = GetComponent<FishMovement>();
            _spawn = transform.parent.GetComponent<SpawnZone>();
            _school = _spawn.GetComponent<FishSchoolBehaviour>();

            _school.Shoals.Add(this);
        }

        public void Movement() {
            _movement.TargetPos = _spawn.testObject.transform.position; // TODO: Remove this in place of the spawn location, or 
            CalculateAvoidanceTurning();
            CalculateTargetTurning();
            CalculateCohesionTurning();
            CalculateAlignmentTurning();

            CalculateFinalTurnValue();
        }

        public void Despawn() {
            BaitManager.instance.RemoveFish(GetComponent<FoodSearch>());
            _school.Shoals.Remove(this);
            GetComponent<Edible>().Despawn();
        }

        private void CalculateAvoidanceTurning() {
            Shoal closestShoal = null;
            float closestShoalDistance = 0;
            float angleToClosestShoal = 0;

            foreach (Shoal shoal in _school.Shoals) {
                if (shoal == this) {
                    continue;
                }

                float shoalDistance = Vector2.Distance(transform.position, shoal.transform.position);
                if (shoalDistance > _avoidanceMaxDistance) {
                    continue;
                }

                float angleToShoal = Vector2.SignedAngle(transform.up, shoal.transform.position - transform.position);
                if (Mathf.Abs(angleToShoal) > _avoidanceAngle * 0.5f && shoalDistance > _avoidanceMaxCloseDistance) {
                    continue;
                }

                if (closestShoal == null) {
                    closestShoal = shoal;
                    closestShoalDistance = shoalDistance;
                    angleToClosestShoal = angleToShoal;
                    continue;
                }

                if (closestShoalDistance < shoalDistance) {
                    continue;
                }

                closestShoal = shoal;
                closestShoalDistance = shoalDistance;
                angleToClosestShoal = angleToShoal;
            }

            if (closestShoal == null) {
                _avoidanceDir = 0f;
                return;
            }

            float desiredAngle = angleToClosestShoal > 0f ? -1f : 1f;
            desiredAngle *= Mathf.InverseLerp(_avoidanceMaxDistance, 0f, closestShoalDistance);
            _avoidanceDir = desiredAngle;
        }

        private void CalculateTargetTurning() {
            float angleToTargetPos = Vector2.SignedAngle(Vector2.up, _movement.TargetPos - (Vector2)transform.position);
            float targetPosAngleDelta = Mathf.DeltaAngle(transform.rotation.eulerAngles.z, angleToTargetPos);

            float targetTurnDirection = Utilities.DirectionFromTransformToTarget(transform, _movement.TargetPos);
            float weighedTurnDirection = Utilities.WeighDirection(targetTurnDirection, targetPosAngleDelta);

            float targetDistance = Vector2.Distance(transform.position, _movement.TargetPos);
            float targetDistanceWeight = Mathf.InverseLerp(0f, _movement.MaxHomeDistance, targetDistance); // Since shoals will never leave home, we use MaxHomeDistance as a value for how much to weigh movement towards target, even though target isn't necessarily home

            _movement.TargetPosDir = weighedTurnDirection * targetDistanceWeight;
        }

        private void CalculateCohesionTurning() {
            float angleToSchoolCenter = Vector2.SignedAngle(Vector2.up, _school.SchoolCenter - (Vector2)transform.position);
            float schoolCenterAngleDelta = Mathf.DeltaAngle(transform.rotation.eulerAngles.z, angleToSchoolCenter);

            float cohesionTurnDirection = Utilities.DirectionFromTransformToTarget(transform, _school.SchoolCenter);
            float weighedCohesionDirection = Utilities.WeighDirection(cohesionTurnDirection, schoolCenterAngleDelta);

            float schoolCenterDistance = Vector3.Distance(transform.position, _school.SchoolCenter);
            float schoolCenterDistanceWeight = Mathf.InverseLerp(0, _movement.MaxHomeDistance, schoolCenterDistance);

            _cohesionDir = weighedCohesionDirection * schoolCenterDistanceWeight;
        }

        private void CalculateAlignmentTurning() {
            float alignmentAngleDelta = Mathf.DeltaAngle(transform.rotation.eulerAngles.z, _school.AverageRotation);
            float alignmentDirection = alignmentAngleDelta == 0 ? 0 : (alignmentAngleDelta > 0 ? 1 : -1);
            float weighedAlignmentDirection = Utilities.WeighDirection(alignmentDirection, alignmentAngleDelta);
            _alignmentDir = weighedAlignmentDirection;
        }

        private void CalculateFinalTurnValue() {
            float calculatedDir;

            if (_avoidanceDir == 0) { // If _avoidanceDir == 0, it had nothing to avoid and shouldn't be factored into the turning calculation
                _targetTurn = _movement.TargetPosDir * (_targetPosWeight + _avoidanceDirWeight * _targetPosWeight); // When _avoidanceDirWeight is not needed, we split it by each turn's weight
                _alignmentTurn = _alignmentDir * (_alignmentDirWeight + _avoidanceDirWeight * _alignmentDirWeight);
                _cohesionTurn = _cohesionDir * (_cohesionDirWeight + _avoidanceDirWeight * _cohesionDirWeight);
                _avoidanceTurn = 0;

                calculatedDir = Mathf.Clamp(_alignmentTurn + _cohesionTurn + _targetTurn, -1, 1);
            }
            else {
                _targetTurn = _movement.TargetPosDir * _targetPosWeight;
                _alignmentTurn = _alignmentDir * _alignmentDirWeight;
                _cohesionTurn = _cohesionDir * _cohesionDirWeight;
                _avoidanceTurn = _avoidanceDir * _avoidanceDirWeight;

                calculatedDir = Mathf.Clamp(_alignmentTurn + _cohesionTurn + _avoidanceTurn + _targetTurn, -1, 1);
            }

            _movement.RotationDir = Mathf.Clamp(calculatedDir, -1, 1);
        }

        private void OnDrawGizmosSelected() {
            if (_drawCohesionGizmo) {
                DrawCohesionGizmo();
            }
            if (_drawAlignmentGizmo) {
                DrawAlignmentGizmo();
            }
            if (_drawAvoidanceGizmo) {
                DrawSeparationGizmo();
            }
            if (_drawDirectionGizmos) {
                DrawDirectionGizmos();
            }
        }

        private void DrawCohesionGizmo() {
            Gizmos.color = _cohesionGizmoColor;
            Gizmos.DrawRay(transform.position, _school.SchoolCenter - (Vector2)transform.position);
        }

        private void DrawAlignmentGizmo() {
            Gizmos.color = _alignmentGizmoColor;
            Gizmos.DrawRay(transform.position, Quaternion.Euler(0f, 0f, _school.AverageRotation) * Vector3.up);
        }

        private void DrawSeparationGizmo() {
            Gizmos.color = _avoidanceGizmoColor;
            Gizmos.DrawWireSphere(transform.position, _avoidanceMaxCloseDistance);
            Gizmos.DrawRay(transform.position, Quaternion.Euler(0f, 0f, _avoidanceAngle * 0.5f) * transform.up * _avoidanceMaxDistance);
            Gizmos.DrawRay(transform.position, Quaternion.Euler(0f, 0f, -_avoidanceAngle * 0.5f) * transform.up * _avoidanceMaxDistance);
        }

        private void DrawDirectionGizmos() {
            Vector2 rayLength = Vector2.left * _directionGizmoLength;
            Vector2 cohesionRay = rayLength * _cohesionTurn;
            Vector2 alignmentRay = rayLength * _alignmentTurn;
            Vector2 avoidanceRay = rayLength * _avoidanceTurn;
            Vector2 targetRay = rayLength * _targetTurn;
            Vector2 finalRotationRay = rayLength * _movement.RotationDir;

            float spacing = _directionSpace;
            Vector3 padding = Vector3.down * spacing;
            Vector3 startSection = padding;
            Gizmos.color = _targetGizmoColor;
            Gizmos.DrawRay(transform.position + padding, targetRay);

            spacing += _directionPadding;
            padding = Vector3.down * spacing;
            Gizmos.color = _cohesionGizmoColor;
            Gizmos.DrawRay(transform.position + padding, cohesionRay);

            spacing += _directionPadding;
            padding = Vector3.down * spacing;
            Gizmos.color = _alignmentGizmoColor;
            Gizmos.DrawRay(transform.position + padding, alignmentRay);

            spacing += _directionPadding;
            padding = Vector3.down * spacing;
            Gizmos.color = _avoidanceGizmoColor;
            Gizmos.DrawRay(transform.position + padding, avoidanceRay);

            spacing += _directionPadding;
            padding = Vector3.down * spacing;
            float sectionLength = spacing - _directionSpace;
            Gizmos.color = new Color(1f, 1f, 1f, 0.5f);
            Gizmos.DrawRay(transform.position + padding, finalRotationRay);

            Gizmos.color = new Color(1f, 1f, 1f, 0.5f);
            Gizmos.DrawRay(transform.position + startSection, Vector3.down * sectionLength);
        }
    }
}
