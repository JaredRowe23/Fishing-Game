using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.PlayerCamera
{
    public class DepthFilter : MonoBehaviour
    {
        [SerializeField] private float maxAlpha;
        [SerializeField] private float minDepth;
        [SerializeField] private float maxDepth;
        private SpriteRenderer sprite;

        private void Start()
        {
            sprite = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            if (-transform.position.y < minDepth) return;
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.g, Mathf.InverseLerp(minDepth, maxDepth, (-transform.position.y - minDepth)) * maxAlpha);
        }
    }
}
