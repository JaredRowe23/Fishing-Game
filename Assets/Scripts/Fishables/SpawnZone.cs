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

        private CameraBehaviour playerCam;

        private void Awake()
        {
            spawnTimer = new WaitForSeconds(spawnTimeSpacing);
            playerCam = CameraBehaviour.instance;
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
            Vector2 _rand;
            while (true)
            {
                if (i > spawnAttempts)
                {
                    return;
                }

                _rand = Random.insideUnitCircle * radius;
                i++;

                if (_rand.y + transform.position.y >= 0f)
                {
                    continue;
                }

                if (playerCam.IsInFrame(new Vector2(_rand.x + transform.position.x, _rand.y + transform.position.y)))
                {
                    continue;
                }

                break;
            }

            Vector2 spawnPos = new Vector2(_rand.x + transform.position.x, _rand.y + transform.position.y);
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