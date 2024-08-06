using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.PlayerCamera;

namespace Fishing.Fishables
{
    public class PlantStalk : MonoBehaviour
    {
        [SerializeField] private float spawnTime;
        [SerializeField] private GameObject fruitPrefab;
        [SerializeField] private List<Transform> fruitPoints;

        [SerializeField] private GameObject[] fruits;
        private float spawnCount;
        private CameraBehaviour playerCam;

        private void Awake()
        {
            playerCam = CameraBehaviour.instance;
        }

        private void Start()
        {
            fruits = new GameObject[fruitPoints.Count];

            for (int i = 0; i < fruits.Length; i++) SpawnFruit(i);
            spawnCount = spawnTime;
        }

        private void Update()
        {
            spawnCount -= Time.deltaTime;

            if (spawnCount > 0) return;
            int i = Random.Range(0, fruitPoints.Count);
            if (playerCam.IsInFrame(fruitPoints[i].position)) return;
            SpawnFruit(Random.Range(0, fruitPoints.Count));
        }

        private void SpawnFruit(int index)
        {
            if (fruits[index] != null) return;

            GameObject newFruit = Instantiate(fruitPrefab, fruitPoints[index].position, Quaternion.identity, this.transform);
            fruits[index] = newFruit;
            spawnCount = spawnTime;
        }

        public void RemoveFruit(GameObject _fruit)
        {
            Debug.Log(System.Array.IndexOf(fruits, _fruit));
            fruits[System.Array.IndexOf(fruits, _fruit)] = null;
        }
    }
}
