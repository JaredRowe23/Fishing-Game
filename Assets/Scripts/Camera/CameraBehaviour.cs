// This handles our camera's physics for following the hook around the world

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public HookControl hook;

    [SerializeField] private float followSpeed;
    [SerializeField] private float followThreshold;

    [SerializeField] private float zoomMagnitude;
    [SerializeField] private float maxZoom;
    [SerializeField] private float minZoom;
    
    // Calculate the distance from the camera to our hook, and then
    // exponentially speed our camera towards the hook's x/y coordinates
    // based on that distance value
    void Update()
    {
        float dist = Vector3.Distance(transform.position, hook.transform.position);
        if (dist > followThreshold)
        {
            Vector3 targetPos = new Vector3(hook.transform.position.x, hook.transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Mathf.Pow(followSpeed * Time.deltaTime * dist, 2));
        }

        transform.Translate(0f, 0f, Input.GetAxis("Mouse ScrollWheel") * zoomMagnitude);
        if (Mathf.Abs(transform.position.z) > maxZoom)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -maxZoom);
        }
        else if (Mathf.Abs(transform.position.z) < minZoom)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -minZoom);
        }

    }

    // Function for other scripts to call to see if something is within the frame of the camera
    public bool IsInFrame(Vector3 pos)
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(pos);
        if (viewportPos.x < 0f || viewportPos.x > 1f || viewportPos.y < 0f || viewportPos.y > 1f)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
