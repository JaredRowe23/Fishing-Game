using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.PlayerCamera;
using Fishing.Util;

namespace Fishing
{
    public class SpawnZone : MonoBehaviour, ISpawn
    {
        [SerializeField] private bool continueSpawning;
        [SerializeField] private float radius;
        [SerializeField] private GameObject prefab;
        [SerializeField] private int spawnMax;
        public List<GameObject> spawnList;
        [SerializeField] private float spawnTimeSpacing;
        private WaitForSeconds spawnTimer;
        [SerializeField] private int spawnAttempts;

        public GameObject testObject;

        private CameraBehaviour playerCam;
        private PolygonCollider2D[] floorColliders;

        private void Awake()
        {
            spawnTimer = new WaitForSeconds(spawnTimeSpacing);
            playerCam = CameraBehaviour.Instance;
            floorColliders = GameObject.Find("Grid").GetComponentsInChildren<PolygonCollider2D>();
        }

        private void Start()
        {
            for (int i = 0; i < spawnMax; i++) Spawn();
            StartCoroutine(Co_Spawn());
        }

        IEnumerator Co_Spawn()
        {
            while (true)
            {
                if (spawnList.Count < spawnMax && continueSpawning) Spawn();

                yield return spawnTimer;
            }
        }

        public void Spawn()
        {
            Vector2 spawnPos = GenerateSpawnPosition();
            GameObject newFish = Instantiate(prefab, spawnPos, Quaternion.identity, transform);
            spawnList.Add(newFish);
        }

        private Vector2 GenerateSpawnPosition()
        {
            int i = 0;
            Vector2 _randomCircle;
            Vector2 _randomWorldPosition;
            while (true)
            {
                _randomCircle = Random.insideUnitCircle * radius;
                _randomWorldPosition = (Vector2)transform.position + _randomCircle;

                if (i > spawnAttempts)
                {
                    if (_randomWorldPosition.y > 0) _randomWorldPosition.y = 0;
                    return _randomWorldPosition;
                }
                i++;

                if (_randomCircle.y + transform.position.y >= 0f) continue;
                if (playerCam.IsInFrame(new Vector2(_randomCircle.x + transform.position.x, _randomCircle.y + transform.position.y))) continue;
                if (Utilities.ClosestPointFromColliders(_randomWorldPosition, floorColliders).collider.OverlapPoint(_randomWorldPosition)) continue;

                return _randomWorldPosition;
            }
        }

        public void RemoveFromList(GameObject _go) => spawnList.Remove(_go);

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}