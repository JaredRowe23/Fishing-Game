// This will be the general script for any trash items
// that just need to fall in a set direction/speed

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkingTrash : MonoBehaviour
{
    [SerializeField] private float sinkSpeed;
    [SerializeField] private Vector3 sinkDirection;
    [SerializeField] private float maximumDepth;

    private FishableItem fishableItem;

    private void Awake() => fishableItem = GetComponent<FishableItem>();

    private void Update()
    {
        if (fishableItem.isHooked) return;

        transform.Translate(sinkDirection * sinkSpeed * Time.deltaTime, Space.World);

        if (transform.parent.position.y - transform.position.y < maximumDepth) return;

        if (Camera.main.GetComponent<CameraBehaviour>().IsInFrame(transform.position)) return;

        transform.parent.GetComponent<SpawnZone>().spawnList.Remove(gameObject);
        Destroy(this.gameObject);
    }
}
