using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.FishingMechanics;

namespace Fishing.Fishables.Fish
{
    public class Fish : MonoBehaviour
    {
        public float maxHomeDistance;
        public float maxHomeDistanceVariation;

        public float swimSpeed;
        public float eatDistance;

        public Vector3 targetPos;
        public FoodSearch foodSearch;
        public SpawnZone spawn;

        private RodManager rodManager;

        private void Start()
        {
            foodSearch = GetComponent<FoodSearch>();
            spawn = transform.parent.GetComponent<SpawnZone>();
            maxHomeDistance += Random.Range(-maxHomeDistanceVariation, maxHomeDistanceVariation);
        }
        public void Eat()
        {
            if (foodSearch.desiredFood.GetComponent<HookBehaviour>())
            {
                foodSearch.desiredFood.GetComponent<HookBehaviour>().SetHook(GetComponent<Fishable>());
                return;
            }

            if (foodSearch.desiredFood.GetComponent<Fishable>())
            {
                if (foodSearch.desiredFood.GetComponent<Fishable>().isHooked)
                {
                    SetThisToHooked();
                    return;
                }
            }
            if (foodSearch.desiredFood.GetComponent<BaitBehaviour>())
            {
                SetThisToHooked();
                return;
            }

            GetComponent<AudioSource>().Play();
            foodSearch.desiredFood.GetComponent<IEdible>().Despawn();
            foodSearch.desiredFood = null;
        }
        public void SetThisToHooked()
        {
            GetComponent<AudioSource>().Play();
            foodSearch.desiredFood.GetComponent<IEdible>().Despawn();
            rodManager.equippedRod.GetHook().hookedObject = null;
            rodManager.equippedRod.GetHook().SetHook(GetComponent<Fishable>());
        }
        public void FaceTarget()
        {
            float angleToTarget = 0f;
            if (targetPos.x < transform.position.x)
            {
                transform.rotation = Quaternion.identity;
                angleToTarget = Vector3.SignedAngle(-transform.right, targetPos - transform.position, Vector3.forward);
            }
            else if (targetPos.x > transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                angleToTarget = -Vector3.SignedAngle(-transform.right, targetPos - transform.position, Vector3.forward);
            }

            transform.Rotate(Vector3.forward, angleToTarget);
        }
        private void OnDrawGizmosSelected()
        {
            if (!GetComponent<Fishable>().isHooked)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.parent.position, maxHomeDistance);
                //if (foodSearch.desiredFood == null)
                //{
                //    Debug.DrawRay(transform.position, targetPos - transform.position, Color.green);
                //}
            }
        }
    }
}
