using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFilter : MonoBehaviour
{
    [SerializeField] private MeshRenderer mesh;
    private bool toggleFilter;

    void Update()
    {

        if (transform.position.y >= 0f && mesh.enabled == true) toggleFilter = true;
        else if (transform.position.y < 0f && mesh.enabled == false) toggleFilter = true;

        if (toggleFilter)
        {
            mesh.enabled = !mesh.enabled;
            toggleFilter = false;
        }
    }
}
