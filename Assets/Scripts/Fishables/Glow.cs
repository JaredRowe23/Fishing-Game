using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.FishingMechanics;
using Fishing.Util;

namespace Fishing.Fishables
{
    public class Glow : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer diffuseSprite;
        [SerializeField] private SpriteRenderer glowSprite;
        [SerializeField] private float minGlowDistance;
        [SerializeField] private float maxGlowDistance;

        private RodManager rodManager;

        private void Start()
        {
            rodManager = RodManager.instance;
        }

        void FixedUpdate()
        {
            glowSprite.flipY = diffuseSprite.flipY;
            float _distance = Vector2.Distance(rodManager.equippedRod.GetHook().transform.position, transform.position);
            if (_distance >= minGlowDistance) glowSprite.color = Utilities.SetTransparency(glowSprite.color, 0);
            else if (_distance <= maxGlowDistance) glowSprite.color = Utilities.SetTransparency(glowSprite.color, 1);
            else glowSprite.color = Utilities.SetTransparency(glowSprite.color, Mathf.InverseLerp(minGlowDistance, maxGlowDistance, _distance));
        }
    }
}
