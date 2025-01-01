using Fishing.Fishables.Fish;
using Fishing.PlayerCamera;
using Fishing.Util.Collision;
using UnityEngine;
using Fishing.Fishables.FishGrid;
using System.Collections;

namespace Fishing.Fishables {
    public class SinkingObject : MonoBehaviour {
        [SerializeField, Min(0), Tooltip("Base speed in m/s the object sinks (or floats) at.")] private float _baseSinkSpeed = 0.5f;
        [SerializeField, Min(0), Tooltip("Amount of variance in sink speed.")] private float _speedVariance = 0.1f;
        private float _sinkSpeed;

        [SerializeField, Tooltip("Direction for the object to sink towards.")] private Vector2 _sinkDirection = new Vector2(-1f, -0.5f);
        [SerializeField, Min(0), Tooltip("Amount of variance in degrees for object's sink direction.")] private float _directionVariance = 10f;

        [SerializeField, Tooltip("Base rotational speed in degrees per second for the object to rotate at. Negative values reverse rotation direction.")] private float _baseRotationSpeed = 10f;
        [SerializeField, Min(0), Tooltip("Amount of variance in degrees for rotation speed.")] private float _rotationVariance = 5f;
        private float _rotationSpeed;

        [SerializeField, Min(0), Tooltip("Maximum distance this object can travel before attempts at destroying it are made.")] private float _maximumDistance = 25f;
        [SerializeField, Min(0), Tooltip("Amount of time to wait after this object has collided with the ground for attempts at destroying it to be made.")] private float _groundedDespawnTime = 10f;

        private Fishable _fishable;
        private CameraBehaviour _camera;
        private SpawnZone _spawn;
        private PolygonCollider2D[] _floorColliders;
        private FishableGrid _fishableGrid;

        private float _groundedCount;
        private bool _isGrounded;

        private void OnValidate() {
            if (_speedVariance > _baseSinkSpeed) {
                _speedVariance = _baseSinkSpeed;
            }
        }

        private void Awake() {
            _fishable = GetComponent<Fishable>();
            _camera = CameraBehaviour.Instance;
            _spawn = transform.parent.GetComponent<SpawnZone>();
            _floorColliders = GameObject.FindGameObjectWithTag("Fishing Level Terrain").GetComponentsInChildren<PolygonCollider2D>(); // TODO: Replace this with a static instance reference to a FishingLevelTerrain script.
            _fishableGrid = FishableGrid.instance;
        }

        private void Start() {
            _sinkSpeed = _baseSinkSpeed + Random.Range(-1, 1) * _speedVariance;
            _rotationSpeed = _baseRotationSpeed + Random.Range(-1, 1) * _rotationVariance;
            float defaultDirection = Mathf.Atan2(_sinkDirection.y, _sinkDirection.x) * 180 / Mathf.PI;
            defaultDirection += Random.Range(-1, 1) * _directionVariance;
            float directionInRadians = defaultDirection * Mathf.Deg2Rad;
            _sinkDirection = new Vector2(Mathf.Cos(directionInRadians), Mathf.Sin(directionInRadians));
            _groundedCount = _groundedDespawnTime;
        }

        private void FixedUpdate() {
            if (_fishable.IsHooked) { 
                return; 
            }

            if (!_isGrounded) {
                Float();
                if (transform.position.y > 0) {
                    Invoke(nameof(StartDestroyWhenOffScreen), _groundedDespawnTime);
                }
                DestroyIfTooFar();
            }
            else {
                Invoke(nameof(StartDestroyWhenOffScreen), _groundedDespawnTime);
            }
        }

        private void Float() {
            transform.Translate(_sinkSpeed * Time.fixedDeltaTime * _sinkDirection.normalized, Space.World);

            CheckIfGrounded();

            if (transform.position.y > 0) { 
                transform.Translate(Vector3.down * transform.position.y, Space.World); 
            }

            transform.Rotate(Vector3.forward, _rotationSpeed * Time.fixedDeltaTime);
        }

        private void CheckIfGrounded() {
            if (!_fishableGrid.IsNearbyTerrainGrid(_fishable.GridSquare[0], _fishable.GridSquare[1], _sinkSpeed)) {
                return;
            }

            SurfacePositionInfo surfacePositionInfo = new SurfacePositionInfo(transform.position, _floorColliders);
            if (!surfacePositionInfo.PositionInsideTerrain) { 
                return; 
            }

            transform.position = surfacePositionInfo.SurfacePosition;
            _isGrounded = true;
        }

        private void StartDestroyWhenOffScreen() {
            StartCoroutine(Co_DestroyWhenOffScreen());
        }

        private IEnumerator Co_DestroyWhenOffScreen() {
            while (true) {
                if (_camera.IsInFrame(transform.position)) {
                    yield return null;
                    continue;
                }
                Destroy(gameObject);
                yield return null;
            }
        }

        private void DestroyIfTooFar() {
            if (Vector3.Distance(transform.position, transform.parent.transform.position) < _maximumDistance) { 
                return; 
            }
            if (_camera.IsInFrame(transform.position)) { 
                return; 
            }

            Destroy(gameObject);
        }

        private void OnDestroy() {
            _spawn.RemoveFromSpawner(gameObject);
        }
    }

}