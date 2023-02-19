using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        private PolygonCollider2D floorCol;

        private void Awake()
        {
            floorCol = FindObjectOfType<PolygonCollider2D>();
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
            Vector2 newPosition;
            while (true)
            {
                if (i > spawnAttempts)
                {
                    return;
                }

                _rand = Random.insideUnitCircle * radius;
                i++;

                _randWorldPosition = new Vector2(_rand.x + transform.position.x, _rand.y + transform.position.y);
                newPosition = floorCol.ClosestPoint(_randWorldPosition);
                if (floorCol.OverlapPoint(newPosition)) continue;
                if (newPosition.y >= 0f) continue;

                break;
            }
            GameObject newObject = Instantiate(prefab, newPosition - (positionOffset * scale), Quaternion.identity, this.transform);
            newObject.transform.localScale = Vector3.one * (scale + Random.Range(-scaleVariance, scaleVariance));
            float _rotationToFloor = Vector2.SignedAngle(Vector2.up, _randWorldPosition - floorCol.ClosestPoint(_randWorldPosition));
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
