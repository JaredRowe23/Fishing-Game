using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.PlayerCamera;
using Fishing.Fishables.Fish;

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
        [SerializeField] private float stuckDespawnTime = 10f;

        private Fishable fishableItem;
        private Edible edible;
        private CameraBehaviour cam;
        private SpawnZone spawn;
        private PolygonCollider2D floorCol;
        private float stuckCount;
        private bool isStuck;

        private void Awake()
        {
            edible = GetComponent<Edible>();
            fishableItem = GetComponent<Fishable>();
            cam = CameraBehaviour.instance;
            spawn = transform.parent.GetComponent<SpawnZone>();
            floorCol = FindObjectOfType<PolygonCollider2D>();

            sinkSpeed += Random.Range(-1, 1) * speedVariance;
            rotationSpeed += Random.Range(-1, 1) * rotationVariance;
            float _defaultDirection = Mathf.Atan2(sinkDirection.y, sinkDirection.x) * 180 / Mathf.PI;
            _defaultDirection += Random.Range(-1, 1) * directionVariance;
            float _directionInRadians = _defaultDirection * Mathf.Deg2Rad;
            sinkDirection = new Vector2(Mathf.Cos(_directionInRadians), Mathf.Sin(_directionInRadians));
            stuckCount = stuckDespawnTime;
        }

        private void Update()
        {
            if (fishableItem.isHooked) return;

            if (!isStuck)
            {
                if (floorCol.OverlapPoint(transform.position))
                {
                    transform.position = floorCol.ClosestPoint(transform.position);
                    isStuck = true;
                    return;
                }

                transform.Translate(sinkSpeed * Time.deltaTime * sinkDirection, Space.World);
                if (transform.position.y > 0) transform.Translate(Vector3.down * transform.position.y, Space.World);
                transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, transform.parent.transform.position) < maximumDistance) return;
                if (cam.IsInFrame(transform.position)) return;

                Despawn();
            }

            else
            {
                stuckCount -= Time.deltaTime;

                if (cam.IsInFrame(transform.position)) return;

                if (stuckCount <= 0)
                {
                    Despawn();
                }
            }
        }

        public void Despawn()
        {
            spawn.spawnList.Remove(gameObject);
            FoodSearchManager.instance.RemoveFood(edible);
            DestroyImmediate(gameObject);
        }
    }

}