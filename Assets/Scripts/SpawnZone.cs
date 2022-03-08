using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.PlayerCamera;

namespace Fishing
{
    public class SpawnZone : MonoBehaviour
    {
        [SerializeField] private bool continueSpawning;
        [SerializeField] private float radius;
        [SerializeField] private GameObject prefab;
        [SerializeField] private int spawnMax;
        public List<GameObject> spawnList;
        [SerializeField] private float spawnTimeSpacing;
        private WaitForSeconds spawnTimer;
        [SerializeField] private int spawnAttempts;

        private void Awake()
        {
            spawnTimer = new WaitForSeconds(spawnTimeSpacing);
        }

        private void Start()
        {
            while (spawnList.Count < spawnMax)
            {
                Spawn();
            }
            StartCoroutine(Co_Spawn());
        }

        IEnumerator Co_Spawn()
        {
            while (true)
            {
                if (spawnList.Count < spawnMax && continueSpawning)
                {
                    Spawn();
                }

                yield return spawnTimer;
            }
        }

        private void Spawn()
        {
            int i = 0;
            Vector2 rand;
            while (true)
            {
                if (i > spawnAttempts)
                {
                    return;
                }

                rand = Random.insideUnitCircle * radius;
                i++;

                if (rand.y + transform.position.y >= 0f)
                {
                    continue;
                }

                if (Camera.main.GetComponent<CameraBehaviour>().IsInFrame(new Vector3(rand.x + transform.position.x, rand.y + transform.position.y, transform.position.z)))
                {
                    continue;
                }

                break;
            }

            Vector3 spawnPos = new Vector3(rand.x + transform.position.x, rand.y + transform.position.y, transform.position.z);
            GameObject newFish = Instantiate(prefab, spawnPos, Quaternion.identity, this.transform);
            spawnList.Add(newFish);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}