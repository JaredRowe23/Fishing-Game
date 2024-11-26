using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.Util;

namespace Fishing.Fishables.Fish
{
    [RequireComponent(typeof(Fishable))]
    [RequireComponent(typeof(FoodSearch))]
    public class FishMovement : MonoBehaviour
    {
        [SerializeField] private float swimSpeed;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float obstacleAvoidanceDistance;

        public float targetPosDirWeight = 1f;
        public float obstacleAvoidanceDirWeight = 1f;

        [SerializeField] private float baseMaxHomeDistance;
        [SerializeField] private float maxHomeDistanceVariation;

        private SpriteRenderer[] flippableSprites;

        [HideInInspector] public Vector2 targetPos;
        [HideInInspector] [Range(-1, 1)] public float targetPosDir = 0;
        [HideInInspector] [Range(-1, 1)] public float rotationDir = 0;

        [HideInInspector] public GameObject activePredator;
        private FoodSearch foodSearch;
        private SpawnZone spawn;
        private PolygonCollider2D[] floorColliders;
        private float maxHomeDistance;

        private void Awake()
        {
            foodSearch = GetComponent<FoodSearch>();
            spawn = transform.parent.GetComponent<SpawnZone>();
            floorColliders = GameObject.Find("Grid").GetComponentsInChildren<PolygonCollider2D>();
            flippableSprites = GetSpriteRenderers();
        }

        private void Start()
        {
            maxHomeDistance = baseMaxHomeDistance + Random.Range(-maxHomeDistanceVariation, maxHomeDistanceVariation);
        }

        private void Update()
        {
            if (GetComponent<Fishable>().isHooked) return;

            DecideMovementDirection();
            Move();
            FlipSprite();

            if (!activePredator) return;
            if (!activePredator.GetComponent<FoodSearch>()) return;
            if (activePredator.GetComponent<FoodSearch>().DesiredFood == null) activePredator = null;
        }

        private void DecideMovementDirection()
        {
            Vector2 _surfacingCheck = transform.position + (transform.up * obstacleAvoidanceDistance);
            ClosestPointInfo _closestPointInfo = Utilities.ClosestPointFromColliders(transform.position, floorColliders);
            float _distToFloor = Vector2.Distance(transform.position, _closestPointInfo.position);

            if (_surfacingCheck.y >= 0) AvoidSurface();

            else if (_distToFloor < obstacleAvoidanceDistance) AvoidFloor(_closestPointInfo.position);

            else if (activePredator != null) AvoidPredator();

            else if (Vector2.Distance(transform.position, spawn.transform.position) >= maxHomeDistance)
            {
                targetPos = spawn.transform.position;
                CalculateTurnDirection();
            }

            else if (foodSearch.DesiredFood != null)
            {
                targetPos = foodSearch.DesiredFood.transform.position;
                CalculateTurnDirection();
            }

            else GetComponent<IMovement>().Movement();
        }

        private void AvoidSurface() => rotationDir = transform.rotation.eulerAngles.z > 0 ? obstacleAvoidanceDirWeight : -obstacleAvoidanceDirWeight;

        private void AvoidFloor(Vector2 _closestFloorPosition) => rotationDir = Utilities.DirectionFromTransformToTarget(transform, (Vector2)transform.position + ((Vector2)transform.position) - _closestFloorPosition);

        private void AvoidPredator() => rotationDir = Utilities.DirectionFromTransformToTarget(transform, (Vector2)transform.position + ((Vector2)transform.position) - (Vector2)activePredator.transform.position);

        public void CalculateTurnDirection()
        {
            targetPosDir = Utilities.DirectionFromTransformToTarget(transform, targetPos);

            rotationDir = Mathf.Clamp(targetPosDir * targetPosDirWeight, -1, 1);
        }

        private void Move()
        {
            transform.Rotate(0f, 0f, rotationSpeed * rotationDir * Time.deltaTime);
            transform.position = Vector2.MoveTowards(transform.position, transform.up + transform.position, swimSpeed * Time.deltaTime);
        }

        private void FlipSprite()
        {
            if (Utilities.UnsignedToSignedAngle(transform.rotation.eulerAngles.z) < 0) for (int i = 0; i < flippableSprites.Length; i++) flippableSprites[i].flipY = true;
            else for (int i = 0; i < flippableSprites.Length; i++) flippableSprites[i].flipY = false;
        }

        private SpriteRenderer[] GetSpriteRenderers()
        {
            SpriteRenderer[] _sprites = transform.GetComponentsInChildren<SpriteRenderer>();
            int _minimapSpriteIndex = 0;
            for (int i = 0; i < _sprites.Length; i++)
            {
                if (_sprites[i].gameObject.layer == LayerMask.NameToLayer("Minimap")) _minimapSpriteIndex = i;
            }

            for (int i = _minimapSpriteIndex + 1; i < _sprites.Length; i++)
            {
                _sprites[i - 1] = _sprites[i];
            }
            System.Array.Resize(ref _sprites, _sprites.Length - 1);
            return _sprites;
        }

        private void OnDrawGizmosSelected()
        {
            if (!GetComponent<Fishable>().isHooked)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(spawn.transform.position, maxHomeDistance);
                Gizmos.color = new Color(Color.cyan.r, Color.cyan.g, Color.cyan.b, 0.25f);
                Gizmos.DrawWireSphere(spawn.transform.position, baseMaxHomeDistance + maxHomeDistanceVariation);
                Gizmos.DrawWireSphere(spawn.transform.position, baseMaxHomeDistance - maxHomeDistanceVariation);
            }
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(targetPos, 1);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, obstacleAvoidanceDistance);
        }

        public float GetMaxHomeDistance() => maxHomeDistance;
    }
}
