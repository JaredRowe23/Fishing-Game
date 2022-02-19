using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private GameObject prefab;
    [SerializeField] private int spawnMax;
    public List<GameObject> spawnList;
    [SerializeField] private float spawnTimeSpacing;
    private WaitForSeconds spawnTimer;
    private FoodSearchManager foodSearchManager;

    private void Awake()
    {
        spawnTimer = new WaitForSeconds(spawnTimeSpacing);
        foodSearchManager = GameController.instance.GetComponent<FoodSearchManager>();
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
            if (spawnList.Count < spawnMax)
            {
                Spawn();
            }

            yield return spawnTimer;
        }
    }

    private void Spawn()
    {
        Vector2 rand = Random.insideUnitCircle * radius;
        while (rand.y + transform.position.y >= 0f || Camera.main.GetComponent<CameraBehaviour>().IsInFrame(new Vector3(rand.x + transform.position.x, rand.y + transform.position.y, transform.position.z)))
        {
            rand = Random.insideUnitCircle * radius;
        }
        Vector3 spawnPos = new Vector3(rand.x + transform.position.x, rand.y + transform.position.y, transform.position.z);
        GameObject newFish = Instantiate(prefab, spawnPos, Quaternion.identity, this.transform);
        spawnList.Add(newFish);
        GameController.instance.foodTransforms.Add(newFish.transform);
        if (newFish.GetComponent<FoodSearch>()) foodSearchManager.fish.Add(newFish.GetComponent<FoodSearch>());
        if (newFish.GetComponent<FishableItem>()) foodSearchManager.fishableItems.Add(newFish.GetComponent<FishableItem>());
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}