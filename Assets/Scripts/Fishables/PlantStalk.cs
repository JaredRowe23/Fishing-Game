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

        public List<GameObject> fruits;
        public List<int> fruitIndices;
        private float spawnCount;
        private CameraBehaviour playerCam;

        private void Awake()
        {
            playerCam = CameraBehaviour.instance;
        }

        private void Start()
        {
            fruitIndices = new List<int>();
            for (int j = 0; j < fruitPoints.Count; j++)
            {
                fruitIndices.Add(-1);
            }

            int i = 0;
            while (fruitPoints.Count > fruits.Count)
            {
                SpawnFruit(i);
                i++;
            }
            spawnCount = spawnTime;
        }

        private void Update()
        {
            spawnCount -= Time.deltaTime;

            if (spawnCount > 0) return;

            for (int i = 0; i < fruitPoints.Count; i++)
            {
                if (fruitIndices[i] != -1) continue;
                if (playerCam.IsInFrame(fruitPoints[i].position)) continue;
                SpawnFruit(i);
            }

        }

        private void SpawnFruit(int index)
        {
            GameObject newFruit = Instantiate(fruitPrefab, fruitPoints[index].position, Quaternion.identity, this.transform);
            fruits.Insert(index, newFruit);
            fruitIndices[index] = index;
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
                fruits.Remove(fruits[i]);
                fruitIndices[i] = -1;
                return;
            }
        }
    }
}
