using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.Util;

namespace Fishing
{
    public class SpawnBed : MonoBehaviour, ISpawn
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private float radius;
        [SerializeField] private int spawnMax;
        [SerializeField] private int spawnAttempts;
        [SerializeField] private float spawnHeightFromFloor;
        [SerializeField] private float scale;
        [SerializeField] private float scaleVariance;

        private List<GameObject> spawnList;
        private PolygonCollider2D[] floorColliders;

        private void Awake()
        {
            floorColliders = GameObject.Find("Grid").GetComponentsInChildren<PolygonCollider2D>();
            spawnList = new List<GameObject>();
        }

        private void Start()
        {
            for (int i = 0; i < spawnMax; i++) Spawn();
        }

        public void Spawn()
        {

            SpawnFloorInfo _spawnInfo = GenerateSpawnPositionInfo();

            GameObject _newObject = Instantiate(prefab, _spawnInfo.surfacePosition, Quaternion.Euler(_spawnInfo.rotationFromFloor), this.transform);
            _newObject.transform.position += _newObject.transform.up * (spawnHeightFromFloor * scale);
            _newObject.transform.localScale = Vector3.one * (scale + Random.Range(-scaleVariance, scaleVariance));
            spawnList.Add(_newObject);
        }

        private SpawnFloorInfo GenerateSpawnPositionInfo()
        {
            int i = 0;
            Vector2 _randomCirclePosition;
            Vector2 _randomWorldPosition;

            while (true)
            {
                _randomCirclePosition = Random.insideUnitCircle * radius;
                _randomWorldPosition = (Vector2)transform.position + _randomCirclePosition;

                if (i > spawnAttempts) return new SpawnFloorInfo(_randomWorldPosition, floorColliders);
                i++;

                ClosestPointInfo _closestPointInfo = Utilities.ClosestPointFromColliders(_randomWorldPosition, floorColliders);
                if (_closestPointInfo.collider.OverlapPoint(_randomWorldPosition)) continue;
                if (_closestPointInfo.position.y >= 0f) continue;

                return new SpawnFloorInfo(_randomWorldPosition, floorColliders);
            }
        }

        public void RemoveFromList(GameObject _go) => spawnList.Remove(_go);

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }

    public struct SpawnFloorInfo
    {
        public Vector2 surfacePosition;
        public Vector3 rotationFromFloor;

        public SpawnFloorInfo(Vector2 _position, PolygonCollider2D[] _colliders)
        {
            surfacePosition = Utilities.ClosestPointFromColliders(_position, _colliders).position;
            rotationFromFloor = new Vector3(0, 0, Vector2.Angle(Vector2.up, _position - surfacePosition));
        }
    }
}
