using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.FishingMechanics;

namespace Fishing.Fishables.Fish
{
    [RequireComponent(typeof(Edible))]
    [RequireComponent(typeof(FoodSearch))]
    [RequireComponent(typeof(Fishable))]
    [RequireComponent(typeof(Fish))]
    public class Fish2Behaviour : MonoBehaviour, IEdible
    {
        private Fish fish;
        private FoodSearch foodSearch;
        private SpawnZone spawn;
        private FishSchoolBehaviour school;
        private FishSchoolManager manager;

        public float rotationSpeed;

        public float separationDirWeight = 1f;
        public float cohesionDirWeight = 1f;
        public float alignmentDirWeight = 1f;
        public float targetPosDirWeight = 1f;

        [Range(-1, 1)]
        public float separationDir = 0;
        [Range(-1, 1)]
        public float cohesionDir = 0;
        [Range(-1, 1)]
        public float alignmentDir = 0;
        [Range(-1, 1)]
        public float targetPosDir = 0;
        [Range(-1, 1)]
        public float rotationDir = 0;

        private void Awake()
        {
            foodSearch = GetComponent<FoodSearch>();
            fish = GetComponent<Fish>();
            spawn = transform.parent.GetComponent<SpawnZone>();
            school = spawn.GetComponent<FishSchoolBehaviour>();
            manager = spawn.GetComponent<FishSchoolManager>();

            school.fish.Add(GetComponent<Fishable>());
            manager.AddFish(fish);
        }

        private void Update()
        {
            if (GetComponent<Fishable>().isHooked) return;

            if (foodSearch.desiredFood == null) fish.targetPos = spawn.testObject.transform.position;
            else fish.targetPos = foodSearch.desiredFood.transform.position;

            AssignDirections();
            MoveTowardsTarget();
            FlipSprite();
        }

        private void AssignDirections()
        {
            float _calculatedDir;
            float _trueRotation = (360 - transform.rotation.eulerAngles.z + 270) % 360;

            float _angleToTargetPos = (360 - SignedToUnsignedAngle(Vector2.SignedAngle(-Vector3.up, (Vector2)transform.position - (Vector2)fish.targetPos)));
            float _targetPosAngleDelta = Mathf.DeltaAngle(_trueRotation, _angleToTargetPos);
            targetPosDir = _targetPosAngleDelta < 180 && _targetPosAngleDelta > 0 ? 1 : -1;

            if (foodSearch.desiredFood == null)
            {
                float _angleToSchoolCenter = (360 - SignedToUnsignedAngle(Vector2.SignedAngle(-Vector3.up, (Vector2)transform.position - school.schoolCenter)));
                float _schoolCenterAngleDelta = Mathf.DeltaAngle(_trueRotation, _angleToSchoolCenter);
                cohesionDir = _schoolCenterAngleDelta < 180 && _schoolCenterAngleDelta > 0 ? 1 : -1;

                float _alignmentAngleDelta = Mathf.DeltaAngle(_trueRotation, school.averageAngle);
                alignmentDir = _alignmentAngleDelta < 180 && _alignmentAngleDelta > 0 ? 1 : -1;

                alignmentDir = WeighDirection(alignmentDir, _alignmentAngleDelta);
                targetPosDir = WeighDirection(targetPosDir, _targetPosAngleDelta) * Mathf.InverseLerp(0, fish.maxHomeDistance, Mathf.Clamp(Vector3.Distance(transform.position, fish.targetPos), 0, fish.maxHomeDistance));
                cohesionDir = WeighDirection(cohesionDir, _schoolCenterAngleDelta) * Mathf.InverseLerp(0, fish.maxHomeDistance, Mathf.Clamp(Vector3.Distance(transform.position, school.schoolCenter), 0, fish.maxHomeDistance));
                _calculatedDir = (alignmentDir * alignmentDirWeight + cohesionDir * cohesionDirWeight + separationDir * separationDirWeight + targetPosDir * targetPosDirWeight) / 4f;
            }
            else _calculatedDir = targetPosDir * targetPosDirWeight;

            rotationDir = Mathf.Clamp(_calculatedDir, -1, 1);
        }

        private void MoveTowardsTarget()
        {
            transform.Rotate(0f, 0f, -rotationSpeed * rotationDir * Time.deltaTime);
            transform.position = Vector2.MoveTowards(transform.position, -transform.right + transform.position, fish.swimSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, fish.targetPos) <= fish.eatDistance)
            {
                if (foodSearch.desiredFood) fish.Eat();
            }
            //fish.FaceTarget();
        }

        public void Despawn()
        {
            spawn.spawnList.Remove(gameObject);
            FoodSearchManager.instance.RemoveFish(GetComponent<FoodSearch>());
            FoodSearchManager.instance.RemoveFood(GetComponent<Edible>());
            manager.RemoveFish(fish);
            school.fish.Remove(GetComponent<Fishable>());
            BaitManager.instance.RemoveFish(GetComponent<FoodSearch>());
            DestroyImmediate(gameObject);
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

        private void FlipSprite()
        {
            if ((360 - transform.rotation.eulerAngles.z + 270) % 360 < 180)
            {
                foreach (Transform child in transform)
                {
                    if (child.GetComponent<SpriteRenderer>() != null && child.gameObject.layer != LayerMask.NameToLayer("Minimap"))
                    {
                        child.GetComponent<SpriteRenderer>().flipY = true;
                    }
                }
            }
            else
            {
                foreach (Transform child in transform)
                {
                    if (child.GetComponent<SpriteRenderer>() != null && child.gameObject.layer != LayerMask.NameToLayer("Minimap"))
                    {
                        child.GetComponent<SpriteRenderer>().flipY = false;
                    }
                }
            }
        }
    }
}
