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
        [SerializeField] private Vector2 positionOffset;
        [SerializeField] private float scale;
        [SerializeField] private float scaleVariance;

        public List<GameObject> spawnList;
        private PolygonCollider2D[] floorColliders;

        private void Awake()
        {
            floorColliders = GameObject.Find("Grid").GetComponentsInChildren<PolygonCollider2D>();
            spawnList = new List<GameObject>();
        }

        private void Start()
        {
            while (spawnList.Count < spawnMax)
            {
                Spawn();
            }
        }

        public void Spawn()
        {
            int i = 0;
            Vector2 _rand;
            Vector2 _randWorldPosition;
            ClosestPointInfo _closestRandomPositionInfo;
            while (true)
            {
                if (i > spawnAttempts)
                {
                    return;
                }

                _rand = Random.insideUnitCircle * radius;
                i++;

                _randWorldPosition = new Vector2(_rand.x + transform.position.x, _rand.y + transform.position.y);
                _closestRandomPositionInfo = Utilities.ClosestPointFromColliders(_randWorldPosition, floorColliders);
                if (_closestRandomPositionInfo.collider.OverlapPoint(_closestRandomPositionInfo.position)) continue;
                if (_closestRandomPositionInfo.position.y >= 0f) continue;

                break;
            }
            GameObject newObject = Instantiate(prefab, _closestRandomPositionInfo.position - (positionOffset * scale), Quaternion.identity, this.transform);
            newObject.transform.localScale = Vector3.one * (scale + Random.Range(-scaleVariance, scaleVariance));
            float _rotationToFloor = Vector2.SignedAngle(Vector2.up, _randWorldPosition - _closestRandomPositionInfo.position);
            newObject.transform.Rotate(new Vector3(0, 0, _rotationToFloor));
            spawnList.Add(newObject);
        }

        public void RemoveFromList(GameObject _go) => spawnList.Remove(_go);

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
