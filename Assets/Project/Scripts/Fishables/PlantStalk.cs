using Fishing.PlayerCamera;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Fishing.Fishables {
    public class PlantStalk : MonoBehaviour, ISpawn {
        [SerializeField, Min(0), Tooltip("Amount of time in seconds it takes for a fruit spawn to be attempted.")] private float _spawnTime;
        [SerializeField, Tooltip("Prefab of the fruit to spawn.")] private GameObject _fruitPrefab;
        [SerializeField, Tooltip("Transforms for the positions of where to spawn fruit at.")] private List<Transform> _fruitSpawnPoints;

        private GameObject[] _fruits;
        private CameraBehaviour _playerCam;

        private void Awake() {
            _playerCam = CameraBehaviour.Instance;
        }

        private void Start() {
            _fruits = new GameObject[_fruitSpawnPoints.Count];

            for (int i = 0; i < _fruits.Length; i++) {
                SpawnAtIndex(i); 
            }

            StartCoroutine(Co_SpawnFruit());
        }

        private IEnumerator Co_SpawnFruit() {
            while (true) {
                int i = Random.Range(0, _fruitSpawnPoints.Count);

                if (_playerCam.IsInFrame(_fruitSpawnPoints[i].position)) {
                    yield return null;
                    continue;
                }

                SpawnAtIndex(Random.Range(0, _fruitSpawnPoints.Count));

                yield return new WaitForSeconds(_spawnTime);
            }
        }

        private void SpawnAtIndex(int index) {
            if (_fruits[index] != null) { 
                return; 
            }

            GameObject newFruit = Instantiate(_fruitPrefab, _fruitSpawnPoints[index].position, Quaternion.identity, transform);
            _fruits[index] = newFruit;
        }

        public void RemoveFromSpawner(GameObject fruit) {
            int fruitIndex = System.Array.IndexOf(_fruits, fruit);
            _fruits[fruitIndex] = null;
        }
    }
}
