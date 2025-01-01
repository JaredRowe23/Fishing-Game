using Fishing.PlayerCamera;
using Fishing.Util.Collision;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.Fishables {
    public class SpawnBed : MonoBehaviour, ISpawn {
        [SerializeField, Tooltip("Prefab of the thing to spawn.")] private GameObject _prefab;
        [SerializeField, Min(0), Tooltip("Radius of the area for spawning to occur in.")] private float _radius = 25f;
        [SerializeField, Min(0), Tooltip("Maximum amount of prefabs that can exist under this spawner.")] private int _spawnMax = 10;
        [SerializeField, Min(0), Tooltip("Amount of time in seconds in between object spawns. Cannot change after start. 0 means no spawning will occur after start.")] private float _spawnTime;
        [SerializeField, Min(0), Tooltip("The height above the floor this should spawn at to avoid clipping. Should match the default world Y size of the object.")] private float _spawnHeightFromFloor = 0.4f;
        [SerializeField, Tooltip("Whether to randomize object's scale. Primarily for static scene dressing objects.")] private bool _randomizeScale = false;
        [SerializeField, Min(0), Tooltip("Base scale to spawn prefab at. Meant for non fishable objects who don't manage their own scaling.")] private float _scale = 1f;
        [SerializeField, Min(0), Tooltip("Amount of variance in the spawn scale.")] private float _scaleVariance = 1f;

        private List<GameObject> _spawnList;
        private PolygonCollider2D[] _floorColliders;
        private CameraBehaviour _playerCam;

        [Header("Gizmos")]
        [SerializeField] private bool _drawSpawnRadiusGizmo = false;
        [SerializeField] private Color _spawnRadiusGizmoColor = Color.yellow;

        [SerializeField] private bool _debugging = false;

        private void OnValidate() {
            if (_scaleVariance > _scale) {
                _scaleVariance = _scale;
            }
        }

        private void Awake() {
            _floorColliders = GameObject.FindGameObjectWithTag("Fishing Level Terrain").GetComponentsInChildren<PolygonCollider2D>(); // TODO: Replace with static instance for FishingLevelTerrain script.
            _spawnList = new List<GameObject>();
            _playerCam = CameraBehaviour.Instance;
        }

        private void Start() {
            for (int i = 0; i < _spawnMax; i++) {
                Spawn();
            }
            if (_spawnTime != 0) {
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
                yield return new WaitForSeconds(_spawnTime);
            }
        }

        private void Spawn() {
            SurfacePositionInfo spawnInfo = GenerateSpawnPositionInfo();
            GameObject newObject = Instantiate(_prefab, spawnInfo.SurfacePosition, Quaternion.Euler(spawnInfo.RotationFromFloor), transform);

            if (_randomizeScale) {
                float randomScale = _scale + Random.Range(-_scaleVariance, _scaleVariance);
                newObject.transform.position += newObject.transform.up * (_spawnHeightFromFloor * randomScale * 0.5f);
                newObject.transform.localScale = Vector3.one * randomScale;
            }
            else {
                newObject.transform.position += newObject.transform.up * _spawnHeightFromFloor;
            }

            _spawnList.Add(newObject);
        }

        private SurfacePositionInfo GenerateSpawnPositionInfo() {
            Vector2 spawnPosition = Random.insideUnitCircle * _radius + (Vector2)transform.position;

            if (spawnPosition.y > 0f) {
                spawnPosition.y = 0f;
            }

            SurfacePositionInfo spawnInfo = new SurfacePositionInfo(spawnPosition, _floorColliders);
            if (spawnInfo.SurfacePosition == spawnPosition) {
                Debug.Log($"Could not find surface point at {spawnPosition}", this);
                spawnInfo.SurfacePosition = transform.position;
            }
            return spawnInfo;
        }

        public void RemoveFromSpawner(GameObject go) {
            _spawnList.Remove(go);
        }

        private void OnDrawGizmosSelected() {
            if (_drawSpawnRadiusGizmo) {
                DrawSpawnRadiusGizmo();
            }

            if (_debugging) {
                SurfacePositionInfo spawnFloorInfo = new SurfacePositionInfo(transform.position, _floorColliders);
                if (spawnFloorInfo.SurfacePosition == (Vector2)transform.position) {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireSphere(transform.position, 5f);
                    return;
                }
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, spawnFloorInfo.SurfacePosition);
                Gizmos.DrawWireSphere(spawnFloorInfo.SurfacePosition, 1f);

                Debug.Log(spawnFloorInfo.PositionInsideTerrain);
            }
        }

        private void DrawSpawnRadiusGizmo() {
            Gizmos.color = _spawnRadiusGizmoColor;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}
