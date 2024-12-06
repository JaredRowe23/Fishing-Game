using Fishing.Fishables.FishGrid;
using Fishing.PlayerCamera;
using Fishing.Util.Collision;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing {
    public class SpawnZone : MonoBehaviour, ISpawn {
        [SerializeField, Tooltip("Prefab of the object to spawn.")] private GameObject _prefab;
        [SerializeField, Min(0), Tooltip("Radius in meters for the area to spawn in.")] private float _radius = 20f;
        [SerializeField, Min(0), Tooltip("Maximum amount of objects that can exist at once under this spawner.")] private int _spawnMax = 10;
        [SerializeField, Min(0), Tooltip("Time in seconds in between spawning new objects. Cannot change after start. 0 means no spawning will occur after start.")] private float _spawnTimeSpacing = 15f;

        private List<GameObject> _spawnList;

        public GameObject _testObject;

        private CameraBehaviour _playerCam;
        private PolygonCollider2D[] _floorColliders;
        private FishableGrid _fishGrid;

        [SerializeField] private bool _drawSpawnRadiusGizmo = false;
        [SerializeField] private Color _spawnRadiusGizmoColor = Color.yellow;

        private void Awake() {
            _floorColliders = GameObject.FindGameObjectWithTag("Fishing Level Terrain").GetComponentsInChildren<PolygonCollider2D>(); // TODO: Replace with searching for a static instance of a FishingLevelTerrain script
        }

        private void Start() {
            _playerCam = CameraBehaviour.Instance;
            _fishGrid = FishableGrid.instance;

            _spawnList = new List<GameObject>();

            for (int i = 0; i < _spawnMax; i++) {
                Spawn();
            }
            if (_spawnTimeSpacing != 0) {
                StartCoroutine(Co_Spawn());
            }
        }

        private IEnumerator Co_Spawn() {
            while (true) {
                if (_spawnList.Count >= _spawnMax) {
                    yield return null;
                    continue;
                }
                Vector2 directionToPlayerCam = (_playerCam.transform.position - transform.position).normalized;
                if (_playerCam.IsInFrame((Vector2)transform.position + directionToPlayerCam * _radius)) {
                    yield return null;
                    continue;
                }

                Spawn();
                yield return new WaitForSeconds(_spawnTimeSpacing);
            }
        }

        public void Spawn() {
            Vector2 spawnPos = GenerateSpawnPosition();
            GameObject newFish = Instantiate(_prefab, spawnPos, Quaternion.identity, transform);
            _spawnList.Add(newFish);
        }

        private Vector2 GenerateSpawnPosition() {
            Vector2 randomWorldPosition = Random.insideUnitCircle * _radius + (Vector2)transform.position;

            if (randomWorldPosition.y >= 0f) {
                randomWorldPosition.y = 0f;
            }

            int[] spawnCoordinates = _fishGrid.Vector2ToGrid(randomWorldPosition);
            if (!_fishGrid.GridSquares[spawnCoordinates[0]][spawnCoordinates[1]].IsCollidingWithTerrain) {
                return randomWorldPosition;
            }

            SurfacePositionInfo spawnInfo = new SurfacePositionInfo(randomWorldPosition, _floorColliders);
            if (spawnInfo.surfacePosition == randomWorldPosition) {
                Debug.Log($"Could not find surface point at {randomWorldPosition}", this);
                spawnInfo.surfacePosition = transform.position;
            }
            if (!spawnInfo.positionInsideTerrain) {
                return randomWorldPosition;
            }
            else {
                return spawnInfo.surfacePosition;
            }
        }

        public void RemoveFromSpawner(GameObject go) {
            _spawnList.Remove(go);
        }

        private void OnDrawGizmosSelected() {
            if (_drawSpawnRadiusGizmo) {
                DrawSpawnRadiusGizmo();
            }
        }

        private void DrawSpawnRadiusGizmo() {
            Gizmos.color = _spawnRadiusGizmoColor;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}