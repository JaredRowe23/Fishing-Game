using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.PlayerCamera;
using Fishing.Fishables.Fish;

namespace Fishing.Fishables
{
    [RequireComponent(typeof(Edible))]
    public class SinkingTrash : MonoBehaviour, IEdible
    {
        [SerializeField] private float sinkSpeed;
        [SerializeField] private Vector3 sinkDirection;
        [SerializeField] private float maximumDepth;

        private FishableItem fishableItem;
        private Edible edible;
        private CameraBehaviour cam;
        private SpawnZone spawn;

        private void Awake()
        {
            edible = GetComponent<Edible>();
            fishableItem = GetComponent<FishableItem>();
            cam = CameraBehaviour.instance;
            spawn = transform.parent.GetComponent<SpawnZone>();
        }

        private void Update()
        {
            if (fishableItem.isHooked) return;

            transform.Translate(sinkSpeed * Time.deltaTime * sinkDirection, Space.World);

            if (transform.localPosition.y > -maximumDepth) return;

            if (cam.IsInFrame(transform.position)) return;

            Despawn();
        }

        public void Despawn()
        {
            spawn.spawnList.Remove(gameObject);
            FoodSearchManager.instance.RemoveFood(edible);
            DestroyImmediate(gameObject);
        }
    }

}