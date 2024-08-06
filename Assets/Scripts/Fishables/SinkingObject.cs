using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.PlayerCamera;
using Fishing.Fishables.Fish;
using Fishing.Util;

namespace Fishing.Fishables
{
    [RequireComponent(typeof(Edible))]
    public class SinkingObject : MonoBehaviour, IEdible
    {
        [SerializeField] private float sinkSpeed;
        [SerializeField] private float speedVariance;
        [SerializeField] private Vector2 sinkDirection;
        [SerializeField] private float directionVariance;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float rotationVariance;
        [SerializeField] private float maximumDistance;
        [SerializeField] private float groundedDespawnTime = 10f;

        private Fishable fishableItem;
        private Edible edible;
        private CameraBehaviour cam;
        private SpawnZone spawn;
        private PolygonCollider2D[] floorColliders;


        private float groundedCount;
        private bool isGrounded;

        private void Awake()
        {
            edible = GetComponent<Edible>();
            fishableItem = GetComponent<Fishable>();
            cam = CameraBehaviour.instance;
            spawn = transform.parent.GetComponent<SpawnZone>();
            floorColliders = GameObject.Find("Grid").GetComponentsInChildren<PolygonCollider2D>();

            sinkSpeed += Random.Range(-1, 1) * speedVariance;
            rotationSpeed += Random.Range(-1, 1) * rotationVariance;
            float _defaultDirection = Mathf.Atan2(sinkDirection.y, sinkDirection.x) * 180 / Mathf.PI;
            _defaultDirection += Random.Range(-1, 1) * directionVariance;
            float _directionInRadians = _defaultDirection * Mathf.Deg2Rad;
            sinkDirection = new Vector2(Mathf.Cos(_directionInRadians), Mathf.Sin(_directionInRadians));
            groundedCount = groundedDespawnTime;
        }

        private void Update()
        {
            if (fishableItem.isHooked) return;

            if (!isGrounded)
            {
                Float();
                if (transform.position.y > 0) HandleGroundedOrSurfaced();
                DespawnFarObjects();
            }
            else HandleGroundedOrSurfaced();
        }

        private void Float()
        {
            transform.Translate(sinkSpeed * Time.deltaTime * sinkDirection.normalized, Space.World);

            CheckIfGrounded();

            if (transform.position.y > 0) transform.Translate(Vector3.down * transform.position.y, Space.World);

            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }

        private void CheckIfGrounded()
        {
            ClosestPointInfo _closestPointInfo = Utilities.ClosestPointFromColliders(transform.position, floorColliders);
            if (!_closestPointInfo.collider.OverlapPoint(transform.position)) return;

            transform.position = _closestPointInfo.position;
            isGrounded = true;
        }

        private void HandleGroundedOrSurfaced()
        {
            groundedCount -= Time.deltaTime;
            if (cam.IsInFrame(transform.position)) return;
            if (groundedCount <= 0) Despawn();
        }

        private void DespawnFarObjects()
        {
            if (Vector3.Distance(transform.position, transform.parent.transform.position) < maximumDistance) return;
            if (cam.IsInFrame(transform.position)) return;

            Despawn();
        }

        public void Despawn()
        {
            spawn.spawnList.Remove(gameObject);
            FoodSearchManager.instance.RemoveFood(edible);
            DestroyImmediate(gameObject);
        }
    }

}