using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.FishingMechanics;
using Fishing.Util;

namespace Fishing.Fishables.Fish
{
    [RequireComponent(typeof(FishMovement))]
    public class Shoal : MonoBehaviour, IMovement, IEdible
    {
        [SerializeField] private float _separationDirWeight = 1f;
        public float SeparationDirWeight { get => _separationDirWeight; set => _separationDirWeight = value; }

        [SerializeField] private float _cohesionDirWeight = 1f;
        public float CohesionDirWeight { get => _cohesionDirWeight; set => _cohesionDirWeight = value; }

        [SerializeField] private float _alignmentDirWeight = 1f;
        public float AlignmentDirWeight { get => _alignmentDirWeight; set => _alignmentDirWeight = value; }

        [Range(-1, 1)] private float _separationDir = 0;
        public float SeparationDir { get => _separationDir; set => _separationDir = value; }

        [Range(-1, 1)] private float _cohesionDir = 0;
        public float CohesionDir { get => _cohesionDir; set => _cohesionDir = value; }

        [Range(-1, 1)] private float _alignmentDir = 0;
        public float AlignmentDir { get => _alignmentDir; set => _alignmentDir = value; }

        private SpawnZone _spawn;

        private FishMovement _movement;
        private FishSchoolBehaviour _school;


        private void Awake()
        {
            _movement = GetComponent<FishMovement>();
            _spawn = transform.parent.GetComponent<SpawnZone>();
            _school = _spawn.GetComponent<FishSchoolBehaviour>();

            _school.Shoals.Add(this);
        }

        public void Movement()
        {
            CalculateAvoidance();
            _movement.targetPos = _spawn.testObject.transform.position;

            float _angleToTargetPos = Vector2.SignedAngle(Vector3.up, _movement.targetPos - (Vector2)transform.position);
            float _targetPosAngleDelta = Mathf.DeltaAngle(transform.rotation.eulerAngles.z, _angleToTargetPos);
            _movement.targetPosDir = Utilities.DirectionFromTransformToTarget(transform, _movement.targetPos);

            float _angleToSchoolCenter = Vector2.SignedAngle(Vector3.up, _school.SchoolCenter - (Vector2)transform.position);
            float _schoolCenterAngleDelta = Mathf.DeltaAngle(transform.rotation.eulerAngles.z, _angleToSchoolCenter);
            CohesionDir = Utilities.DirectionFromTransformToTarget(transform, _school.SchoolCenter);

            float _alignmentAngleDelta = Mathf.DeltaAngle(transform.rotation.eulerAngles.z, _school.AverageAngle);
            AlignmentDir = _alignmentAngleDelta == 0 ? 0 : (_alignmentAngleDelta > 0 ? 1 : -1);

            AlignmentDir = WeighDirection(AlignmentDir, _alignmentAngleDelta);
            _movement.targetPosDir = WeighDirection(_movement.targetPosDir, _targetPosAngleDelta) * Mathf.InverseLerp(0, _movement.GetMaxHomeDistance(), Mathf.Clamp(Vector3.Distance(transform.position, _movement.targetPos), 0, _movement.GetMaxHomeDistance()));
            CohesionDir = WeighDirection(CohesionDir, _schoolCenterAngleDelta) * Mathf.InverseLerp(0, _movement.GetMaxHomeDistance(), Mathf.Clamp(Vector3.Distance(transform.position, _school.SchoolCenter), 0, _movement.GetMaxHomeDistance()));
            float _calculatedDir = (AlignmentDir * AlignmentDirWeight + CohesionDir * CohesionDirWeight + SeparationDir * SeparationDirWeight + _movement.targetPosDir * _movement.targetPosDirWeight) / 4f;

            _movement.rotationDir = Mathf.Clamp(_calculatedDir, -1, 1);
        }

        public void Despawn()
        {
            BaitManager.instance.RemoveFish(GetComponent<FoodSearch>());
            _school.Shoals.Remove(this);
            GetComponent<Edible>().Despawn();
        }

        private void CalculateAvoidance() {
            Shoal closestShoal = null;
            float closestShoalDistance = 0;
            float angleToShoal = 0;

            foreach (Shoal shoal in _school.Shoals) {
                if (shoal == this) {
                    continue;
                }

                float shoalDistance = Vector2.Distance(transform.position, shoal.transform.position);
                if (shoalDistance > _school.SeparationMaxDistance) {
                    continue;
                }

                angleToShoal = Vector2.SignedAngle(transform.up, transform.position - shoal.transform.position);
                if (Mathf.Abs(angleToShoal) > _school.SeparationAngle && shoalDistance > _school.SeparationMaxCloseDistance) {
                    continue;
                }

                if (closestShoal == null) {
                    closestShoal = shoal;
                    closestShoalDistance = shoalDistance;
                    continue;
                }

                if (closestShoalDistance < shoalDistance) {
                    continue;
                }

                closestShoal = shoal;
                closestShoalDistance = shoalDistance;
            }

            float desiredAngle = angleToShoal <= 0 ? -1 : 1;
            desiredAngle *= 1 - Mathf.InverseLerp(0, _school.SeparationMaxDistance, Mathf.Clamp(closestShoalDistance, 0, _school.SeparationMaxDistance));
            SeparationDir = desiredAngle == 0 ? 0 : desiredAngle;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, Quaternion.Euler(0f, 0f, _school.AverageAngle) * Vector3.up);

            Gizmos.color = Color.black;
            Gizmos.DrawRay(transform.position, _school.SchoolCenter - (Vector2)transform.position);

            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, Quaternion.Euler(0f, 0f, _school.SeparationAngle) * transform.up * _school.SeparationMaxDistance);
            Gizmos.DrawRay(transform.position, Quaternion.Euler(0f, 0f, -_school.SeparationAngle) * transform.up * _school.SeparationMaxDistance);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _school.SeparationMaxCloseDistance);
        }

        private float WeighDirection(float _dir, float _angle)
        {
            Vector3 _angleVector = new Vector3(Mathf.Cos((_angle - 90f) * Mathf.Deg2Rad), Mathf.Sin(-(_angle - 90f) * Mathf.Deg2Rad), 0f);
            return _dir * (1f - ((Vector3.Dot(Vector3.up, _angleVector) + 1f) / 2f));
        }
    }
}
