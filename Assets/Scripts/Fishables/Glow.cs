using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.FishingMechanics;

namespace Fishing.Fishables
{
    public class Glow : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer diffuseSprite;
        [SerializeField] private SpriteRenderer glowSprite;
        [SerializeField] private float minGlowDistance;
        [SerializeField] private float maxGlowDistance;

        private HookBehaviour hook;

        private void Start()
        {
            glowSprite.color = new Color(1, 1, 1, 0);
        }

        void FixedUpdate()
        {
            glowSprite.flipY = diffuseSprite.flipY;
            float _distance = Vector2.Distance(RodManager.instance.equippedRod.GetHook().transform.position, transform.position);
            if (_distance >= minGlowDistance) glowSprite.color = new Color(1, 1, 1, 0);
            else if (_distance <= maxGlowDistance) glowSprite.color = new Color(1, 1, 1, 1);
            else glowSprite.color = new Color(1, 1, 1, Mathf.InverseLerp(minGlowDistance, maxGlowDistance, _distance));
        }
    }
}
