using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.FishingMechanics;

namespace Fishing.Fishables.Fish
{
    [RequireComponent(typeof(FishMovement))]
    public class Shoal : MonoBehaviour, IMovement, IEdible
    {
        private SpawnZone spawn;

        private FishMovement movement;
        private FishSchoolBehaviour school;
        private FishSchoolManager manager;

        public float separationDirWeight = 1f;
        public float cohesionDirWeight = 1f;
        public float alignmentDirWeight = 1f;

        [Range(-1, 1)]
        public float separationDir = 0;
        [Range(-1, 1)]
        public float cohesionDir = 0;
        [Range(-1, 1)]
        public float alignmentDir = 0;

        private void Awake()
        {
            movement = GetComponent<FishMovement>();
            spawn = transform.parent.GetComponent<SpawnZone>();
            school = spawn.GetComponent<FishSchoolBehaviour>();
            manager = spawn.GetComponent<FishSchoolManager>();

            school.fish.Add(GetComponent<Fishable>());
            manager.AddShoal(this);
        }

        public void Movement()
        {
            movement.targetPos = spawn.testObject.transform.position;

            float _calculatedDir;
            float _trueRotation = (360 - transform.rotation.eulerAngles.z + 270) % 360;

            float _angleToTargetPos = (360 - SignedToUnsignedAngle(Vector2.SignedAngle(-Vector3.up, (Vector2)transform.position - (Vector2)movement.targetPos)));
            float _targetPosAngleDelta = Mathf.DeltaAngle(_trueRotation, _angleToTargetPos);
            movement.targetPosDir = _targetPosAngleDelta < 180 && _targetPosAngleDelta > 0 ? 1 : -1;

            float _angleToSchoolCenter = (360 - SignedToUnsignedAngle(Vector2.SignedAngle(-Vector3.up, (Vector2)transform.position - school.schoolCenter)));
            float _schoolCenterAngleDelta = Mathf.DeltaAngle(_trueRotation, _angleToSchoolCenter);
            cohesionDir = _schoolCenterAngleDelta < 180 && _schoolCenterAngleDelta > 0 ? 1 : -1;

            float _alignmentAngleDelta = Mathf.DeltaAngle(_trueRotation, school.averageAngle);
            alignmentDir = _alignmentAngleDelta < 180 && _alignmentAngleDelta > 0 ? 1 : -1;

            alignmentDir = WeighDirection(alignmentDir, _alignmentAngleDelta);
            movement.targetPosDir = WeighDirection(movement.targetPosDir, _targetPosAngleDelta) * Mathf.InverseLerp(0, movement.GetMaxHomeDistance(), Mathf.Clamp(Vector3.Distance(transform.position, movement.targetPos), 0, movement.GetMaxHomeDistance()));
            cohesionDir = WeighDirection(cohesionDir, _schoolCenterAngleDelta) * Mathf.InverseLerp(0, movement.GetMaxHomeDistance(), Mathf.Clamp(Vector3.Distance(transform.position, school.schoolCenter), 0, movement.GetMaxHomeDistance()));
            _calculatedDir = (alignmentDir * alignmentDirWeight + cohesionDir * cohesionDirWeight + separationDir * separationDirWeight + movement.targetPosDir * movement.targetPosDirWeight) / 4f;

            movement.rotationDir = Mathf.Clamp(_calculatedDir, -1, 1);
        }


        public void Despawn()
        {
            FoodSearchManager.instance.RemoveFish(GetComponent<FoodSearch>());
            BaitManager.instance.RemoveFish(GetComponent<FoodSearch>());
            manager.RemoveShoal(this);
            school.fish.Remove(GetComponent<Fishable>());
            GetComponent<Edible>().Despawn();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, new Vector3(Mathf.Sin((school.averageAngle) * Mathf.Deg2Rad), Mathf.Cos((school.averageAngle) * Mathf.Deg2Rad), 0f));

            Gizmos.color = Color.black;
            Gizmos.DrawRay(transform.position, school.schoolCenter - (Vector2)transform.position);

            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, Quaternion.Euler(0f, 0f, school.separationAngle) * -transform.right * school.separationMaxDistance);
            Gizmos.DrawRay(transform.position, Quaternion.Euler(0f, 0f, -school.separationAngle) * -transform.right * school.separationMaxDistance);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, school.separationMaxCloseDistance);
        }
        private float SignedToUnsignedAngle(float _angle)
        {
            if (_angle < 0) _angle += 360f;
            return _angle % 360;
        }
        private float WeighDirection(float _dir, float _angle)
        {
            Vector3 _angleVector = new Vector3(Mathf.Cos((_angle - 90f) * Mathf.Deg2Rad), Mathf.Sin(-(_angle - 90f) * Mathf.Deg2Rad), 0f);
            return _dir * (1f - ((Vector3.Dot(Vector3.up, _angleVector) + 1f) / 2f));
        }
    }
}
