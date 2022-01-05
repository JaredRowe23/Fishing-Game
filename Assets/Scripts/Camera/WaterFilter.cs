// Handles the enabling/disabling of an object
// we use as a filter over the camera to mimic
// an underwater look

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFilter : MonoBehaviour
{
    [SerializeField] private MeshRenderer mesh;

    void Update()
    {
        if (transform.position.y >= 0f)
        {
            mesh.enabled = false;
        }
        else
        {
            mesh.enabled = true;
        }
    }
}
