﻿// Here we control the physics of the hook,
// from how it's cast, how it moves in the water
// and how it's reeled in

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookControl : MonoBehaviour
{
    [SerializeField] private Transform hookAnchorPoint;
    [SerializeField] private float followSpeed;
    [SerializeField] private float hookHangHeight;

    [Header("Water Physics")]
    [SerializeField] private float drag;
    [SerializeField] private float waterDrag;
    [SerializeField] private float lineRotationAngle = -5f;

    private Vector3 targetPos;
    private RodBehaviour rod;
    private Rigidbody rb;

    private void Start()
    {
        rod = transform.parent.GetComponent<RodBehaviour>();
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // When we aren't casting, move the hook
        // towards where it should hang under the rod
        if (!rod.isCast)
        {
            rb.isKinematic = true;
            targetPos = new Vector3(hookAnchorPoint.position.x, hookAnchorPoint.position.y - hookHangHeight, hookAnchorPoint.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, followSpeed * Time.deltaTime);
        }

        // Once we've casted, we use a combination of useGravity, isKinematic,
        // and rotating the hook about it's anchor point on the rod to mimic
        // proper physics for it
        else
        {
            if (Vector3.Distance(transform.position, hookAnchorPoint.position) >= rod.lineLength)
            {
                if (transform.position.y < 0f)
                {
                    rb.useGravity = false;
                    rb.isKinematic = false;
                    if (transform.position.x - hookAnchorPoint.position.x >= 0f)
                    {
                        transform.RotateAround(hookAnchorPoint.position, Vector3.forward, lineRotationAngle * Time.deltaTime);
                        transform.rotation = Quaternion.identity;
                    }
                }

                // If we're above the water by the time we hit the end of the line
                // just stop the hook's velocity and let it fall
                else
                {
                    rb.velocity = new Vector3(0f, rb.velocity.y, rb.velocity.z);
                }
            }
            else
            {
                rb.useGravity = true;
                rb.isKinematic = false;
            }
        }

        // Once the hook is in the water, change it's drag to simulate it being in water
        if (transform.position.y <= 0f)
        {
            rb.drag = waterDrag;
        }
        else
        {
            rb.drag = drag;
        }
    }

    // When we cast, enable physics and add the proper amount of force at the correct angle
    public void Cast(float angle, float force)
    {
        rb.isKinematic = false;
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
        rb.AddForce(rot * Vector3.right * force);
    }

    // Pull towards the anchor point when reeling in
    public void Reel(float force)
    {
        rb.AddForce(Vector3.Normalize(hookAnchorPoint.position - transform.position) * force);
    }

    public Transform GetHookAnchorPoint()
    {
        return hookAnchorPoint;
    }
}