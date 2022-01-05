// This will be the general script for any trash items
// that just need to fall in a set direction/speed

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkingTrash : MonoBehaviour
{
    [SerializeField] private float sinkSpeed;
    [SerializeField] private Vector3 sinkDirection;
    [SerializeField] private float maximumDistance;

    private void Update()
    {
        if (transform.parent.GetComponent<SpawnZone>())
        {
            if (transform.parent.position.y - transform.position.y >= maximumDistance)
            {
                if (!Camera.main.GetComponent<CameraBehaviour>().IsInFrame(transform.position))
                {
                    transform.parent.GetComponent<SpawnZone>().spawnList.Remove(gameObject);
                    Destroy(this.gameObject);
                }
            }
            transform.Translate(sinkDirection * sinkSpeed * Time.deltaTime, Space.World);
        }
    }
}
