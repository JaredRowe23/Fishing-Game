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

        [SerializeField] private Color minGlowColor = new Color(1, 1, 1, 0);
        [SerializeField] private Color maxGlowColor = new Color(1, 1, 1, 1);

        private RodManager rodManager;

        private void Start()
        {
            rodManager = RodManager.instance;
            glowSprite.color = new Color(1, 1, 1, 0);
        }

        void FixedUpdate()
        {
            glowSprite.flipY = diffuseSprite.flipY;
            float _distance = Vector2.Distance(rodManager.equippedRod.GetHook().transform.position, transform.position);
            if (_distance >= minGlowDistance) glowSprite.color = minGlowColor;
            else if (_distance <= maxGlowDistance) glowSprite.color = maxGlowColor;
            else glowSprite.color = Color.Lerp(minGlowColor, maxGlowColor, Mathf.InverseLerp(minGlowDistance, maxGlowDistance, _distance));
        }
    }
}
