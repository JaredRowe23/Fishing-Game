using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.Util;

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
            sprite.color = Utilities.SetTransparency(sprite.color, Mathf.InverseLerp(minDepth, maxDepth, -transform.position.y) * maxAlpha);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(new Vector3(-1000, -minDepth, 0), new Vector3(1000, -minDepth, 0));
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(-1000, -maxDepth, 0), new Vector3(1000, -maxDepth, 0));
        }
    }
}
