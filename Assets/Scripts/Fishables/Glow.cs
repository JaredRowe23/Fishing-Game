using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.FishingMechanics;

namespace Fishing.Fishables
{
    public class Glow : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private float minGlowDistance;
        [SerializeField] private float maxGlowDistance;

        private HookBehaviour hook;

        private void Start()
        {
            hook = RodManager.instance.equippedRod.GetHook();
            sprite.color = new Color(1, 1, 1, 0);
        }

        void FixedUpdate()
        {
            sprite.flipY = GetComponent<SpriteRenderer>().flipY;
            float _distance = Vector2.Distance(hook.transform.position, transform.position);
            if (_distance >= minGlowDistance) sprite.color = new Color(1, 1, 1, 0);
            else if (_distance <= maxGlowDistance) sprite.color = new Color(1, 1, 1, 1);
            else sprite.color = new Color(1, 1, 1, Mathf.InverseLerp(minGlowDistance, maxGlowDistance, _distance));
        }
    }
}
