// Handles script-ran animation, attributes,
// and general behaviour of our fishing rod

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject rodObject;
    public Sprite inventorySprite;

    [Header("Attributes")]
    public string description;
    public float castStrength;
    public float castAngle;
    public float lineLength;
    [SerializeField] private float reelSpeed;
    [SerializeField] private float minCastStrength;
    [SerializeField] private float maxCastStrength;
    [SerializeField] private float maxCastAngle;
    [SerializeField] private float chargeFrequency;
    [SerializeField] private float angleFrequency;

    [Header("Casting and Reeling")]
    public bool isCast = false;
    [SerializeField] private float reeledInDistance = 0.1f;
    [SerializeField] private HookControl hook;

    // For our animation, we're using multiple target rotations
    // and strengths determining how fast to meet said rotations
    // as well as updating the "state" we're currently in
    [Header("Animation Variables")]
    [SerializeField] private float rotateThreshold = 0.1f;
    [SerializeField] private float castPullRot;
    [SerializeField] private float castForwardRot;
    [SerializeField] private float restingRot;
    [SerializeField] private float reelingRot;
    [SerializeField] private float castPullStrength;
    [SerializeField] private float castForwardStrength;
    [SerializeField] private float restStrength;
    [SerializeField] private float reelStrength;
    [SerializeField] private enum RodState { CastPull, CastForward, Reeling, Resting };
    [SerializeField] private RodState state = RodState.Resting;
    private float targetRot;

    void Start()
    {
        state = RodState.Resting;
        targetRot = restingRot;
        isCast = false;
    }
    
    void Update()
    {
        // When we click, determing to cast or reel in
        // based on our current state, whether we're already
        // casted, and whether UI is open or being clicked on
        if (Input.GetMouseButtonDown(0))
        {
            if (state == RodState.Resting)
            {
                if (isCast)
                {
                    if (hook.transform.position.y <= 0f)
                    {
                        state = RodState.Reeling;
                        targetRot = reelingRot;
                    }
                }
                else if (!GameController.instance.mouseOverUI && !GameController.instance.bucketMenu.gameObject.activeSelf && !GameController.instance.inventoryMenu.gameObject.activeSelf && !GameController.instance.pauseMenu.pauseMenu.gameObject.activeSelf)
                {
                    state = RodState.CastPull;
                    targetRot = castPullRot;
                    GameController.instance.bucketMenuButton.gameObject.SetActive(false);
                    GameController.instance.inventoryMenuButton.gameObject.SetActive(false);

                    // Show the power slider and begin the process for charging/angling our cast
                    GameController.instance.powerSlider.transform.SetParent(GameController.instance.rodCanvas.transform);
                    GameController.instance.powerSlider.StartCharging(chargeFrequency, this);
                }
            }
        }

        // Stop reeling when not holding click
        if (Input.GetMouseButtonUp(0))
        {
            if (state == RodState.Reeling)
            {
                AudioManager.instance.StopPlaying("Reel");
                state = RodState.Resting;
                targetRot = restingRot;
            }
        }

        // If we're reeling, call our hook's Reel function
        // and check if we're close enough to catch what
        // we may have hooked and reset our state to resting
        if (state == RodState.Reeling)
        {
            AudioManager.instance.PlaySound("Reel", true);
            hook.Reel(reelSpeed);
            if (Vector3.Distance(hook.transform.position, hook.GetHookAnchorPoint().position) <= reeledInDistance)
            {
                hook.GetComponent<HookObject>().AddToBucket();
                state = RodState.Resting;
                targetRot = restingRot;
                isCast = false;
                GameController.instance.bucketMenuButton.gameObject.SetActive(true);
                GameController.instance.inventoryMenuButton.gameObject.SetActive(true);
            }
        }

        // Update the rod's rotation, running the "animation"
        UpdateRotation();
    }

    // This takes our current state and rotates towards the desired rotation based on a specific strength
    private void UpdateRotation()
    {
        if (Mathf.Abs(targetRot - ((rodObject.transform.rotation.eulerAngles.z + 180) % 360 - 180)) >= rotateThreshold)
        {
            Vector3 targetRotation = new Vector3(rodObject.transform.rotation.x, rodObject.transform.rotation.y, targetRot);
            if (state == RodState.Resting)
            {
                rodObject.transform.rotation = Quaternion.RotateTowards(rodObject.transform.rotation, Quaternion.Euler(targetRotation), restStrength * Time.deltaTime);
            }
            else if (state == RodState.Reeling)
            {
                rodObject.transform.rotation = Quaternion.RotateTowards(rodObject.transform.rotation, Quaternion.Euler(targetRotation), reelStrength * Time.deltaTime);
            }
            else if (state == RodState.CastPull)
            {
                rodObject.transform.rotation = Quaternion.RotateTowards(rodObject.transform.rotation, Quaternion.Euler(targetRotation), castPullStrength * Time.deltaTime);
            }
            else if (state == RodState.CastForward)
            {
                rodObject.transform.rotation = Quaternion.RotateTowards(rodObject.transform.rotation, Quaternion.Euler(targetRotation), castForwardStrength * Time.deltaTime);
            }
        }
        else
        {
            if (state == RodState.CastForward)
            {
                state = RodState.Resting;
                targetRot = restingRot;
                isCast = true;
                hook.Cast(castAngle, castStrength);
            }
        }
    }

    public void BeginCast()
    {
        state = RodState.CastForward;
        targetRot = castForwardRot;
    }

    public float GetMinStrength()
    {
        return minCastStrength;
    }
    public float GetMaxStrength()
    {
        return maxCastStrength;
    }
    public float GetMaxAngle()
    {
        return maxCastAngle;
    }

    public float GetChargeFrequency()
    {
        return chargeFrequency;
    }
    public float GetAngleFrequency()
    {
        return angleFrequency;
    }
}
