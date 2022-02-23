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
    private CameraBehaviour cam;
    private SpawnZone spawn;

    private void Awake()
    {
        fishableItem = GetComponent<FishableItem>();
        cam = Camera.main.GetComponent<CameraBehaviour>();
        spawn = transform.parent.GetComponent<SpawnZone>();
    }

    private void Update()
    {
        if (fishableItem.isHooked) return;

        transform.Translate(sinkSpeed * Time.deltaTime * sinkDirection, Space.World);

        if (transform.localPosition.y > -maximumDepth) return;

        if (cam.IsInFrame(transform.position)) return;

        spawn.spawnList.Remove(gameObject);
        Destroy(this.gameObject);
    }
}
