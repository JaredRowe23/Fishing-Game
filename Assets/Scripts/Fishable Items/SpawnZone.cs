// Behaviour for SpawnZone gameobjects,
// which will act as configurable spawners
// for any fishable items

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
    private float spawnTimeCount;

    private void Start()
    {
        while(spawnList.Count < spawnMax)
        {
            Spawn();
        }
    }

    private void Update()
    {
        // If we haven't met the spawn limit, generate random coords within our spawn radius
        // until it's not above water and isn't in the camera frame and spawn the item there
        if (spawnList.Count < spawnMax && spawnTimeCount <= 0f)
        {
            Spawn();
            spawnTimeCount = spawnTimeSpacing;
        }

        spawnTimeCount -= Time.deltaTime;
        if (spawnTimeCount < 0f)
        {
            spawnTimeCount = 0f;
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
        spawnList.Add(Instantiate(prefab, spawnPos, Quaternion.identity, this.transform));
    }
    
    // Draw a circle (sphere's easier, but you get the point) to indicate our radius in engine
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
        Debug.DrawRay(transform.position + new Vector3(1f, 0f, 0f), Vector3.up * spawnTimeCount / spawnTimeSpacing * radius, Color.green);
    }
}