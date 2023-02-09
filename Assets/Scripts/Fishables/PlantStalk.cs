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

        public GameObject[] fruits;
        public int[] fruitIndices;
        private float spawnCount;
        private CameraBehaviour playerCam;

        private void Awake()
        {
            playerCam = CameraBehaviour.instance;
        }

        private void Start()
        {
            fruitIndices = new int[fruitPoints.Count];
            fruits = new GameObject[fruitPoints.Count];
            for (int j = 0; j < fruitPoints.Count; j++)
            {
                fruitIndices[j] = -1;
            }

            for (int i = 0; i < fruits.Length; i++) SpawnFruit(i);
            spawnCount = spawnTime;
        }

        private void Update()
        {
            spawnCount -= Time.deltaTime;

            if (spawnCount > 0) return;

            List<int> spawns = new List<int>();
            foreach (int index in fruitIndices) spawns.Add(index);

            for (int i = 0; i < fruitIndices.Length; i++)
            {
                int j = Random.Range(0, spawns.Count);
                if (fruitIndices[j] != -1)
                {
                    spawns.RemoveAt(j);
                    continue;
                }
                if (playerCam.IsInFrame(fruitPoints[j].position))
                {
                    spawns.RemoveAt(j);
                    continue;
                }
                SpawnFruit(j);
                break;
            }

        }

        private void SpawnFruit(int index)
        {
            GameObject newFruit = Instantiate(fruitPrefab, fruitPoints[index].position, Quaternion.identity, this.transform);
            fruits[index] = newFruit;
            fruitIndices[index] = index;
            spawnCount = spawnTime;
        }

        public void RemoveFruit(GameObject _fruit)
        {
            int i = 0;
            foreach(GameObject _f in fruits)
            {
                if (_fruit != _f)
                {
                    i++;
                    continue;
                }
                fruits[i] = null;
                fruitIndices[i] = -1;
                return;
            }
        }
    }
}
