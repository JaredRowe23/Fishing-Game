using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.PlayerCamera
{
    public class WaterFilter : MonoBehaviour
    {
        [SerializeField] private MeshRenderer mesh;

        void Update()
        {
            if (transform.position.y >= 0f && mesh.enabled == true) mesh.enabled = false;
            else if (transform.position.y < 0f && mesh.enabled == false) mesh.enabled = true;
        }
    }
}
