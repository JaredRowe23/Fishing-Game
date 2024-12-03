using Fishing.PlayerCamera;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.Fishables {
    public class PlantStalk : MonoBehaviour, ISpawn {
        [SerializeField] private float spawnTime;
        [SerializeField] private GameObject fruitPrefab;
        [SerializeField] private List<Transform> fruitPoints;

        [SerializeField] private GameObject[] fruits;
        private float spawnCount;
        private CameraBehaviour playerCam;

        private void Awake() {
            playerCam = CameraBehaviour.Instance;
        }

        private void Start() {
            fruits = new GameObject[fruitPoints.Count];

            for (int i = 0; i < fruits.Length; i++) {
                SpawnAtIndex(i); 
            }
            spawnCount = spawnTime;
        }

        private void Update() {
            spawnCount -= Time.deltaTime;

            if (spawnCount > 0) { 
                return; 
            }
            int i = Random.Range(0, fruitPoints.Count);
            if (playerCam.IsInFrame(fruitPoints[i].position)) { 
                return; 
            }
            SpawnAtIndex(Random.Range(0, fruitPoints.Count));
        }

        private void SpawnAtIndex(int index) {
            if (fruits[index] != null) { 
                return; 
            }

            GameObject newFruit = Instantiate(fruitPrefab, fruitPoints[index].position, Quaternion.identity, this.transform);
            fruits[index] = newFruit;
            spawnCount = spawnTime;
        }

        public void RemoveFromSpawner(GameObject _fruit) {
            fruits[System.Array.IndexOf(fruits, _fruit)] = null;
        }
    }
}
