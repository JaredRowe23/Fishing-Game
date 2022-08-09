using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.Fishables;

namespace Fishing.FishingMechanics
{
    [RequireComponent(typeof(FishingRodAnimation))]
    [RequireComponent(typeof(RodScriptable))]
    public class RodBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject rodObject;
        public Sprite inventorySprite;
        public RodScriptable scriptable;

        public BaitBehaviour equippedBait;

        public bool casted = false;
        [SerializeField] private float reeledInDistance = 0.1f;
        [SerializeField] private HookBehaviour hook;
        private FishingRodAnimation anim;
        //private FishingRodStats stats;

        private RodManager rodManager;

        private void Awake()
        {
            anim = GetComponent<FishingRodAnimation>();
            //stats = GetComponent<FishingRodStats>();
            rodManager = RodManager.instance;
        }

        void Start()
        {
            casted = false;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (anim.state == FishingRodAnimation.RodState.Resting)
                {
                    if (casted)
                    {
                        if (hook.transform.position.y <= 0f)
                        {
                            anim.SetState(FishingRodAnimation.RodState.Reeling);
                        }
                    }
                    else if (!UIManager.instance.mouseOverUI && !UIManager.instance.IsActiveUI())
                    {
                        anim.SetState(FishingRodAnimation.RodState.CastPull);
                        UIManager.instance.bucketMenuButton.gameObject.SetActive(false);
                        UIManager.instance.inventoryMenuButton.SetActive(false);

                        // Show the power slider and begin the process for charging/angling our cast
                        PowerAndAngle.instance.StartCharging(scriptable.chargeFrequency, scriptable.minCastStrength, scriptable.maxCastStrength,
                            scriptable.maxCastAngle, scriptable.angleFrequency);
                    }
                }
            }

            // Stop reeling when not holding click
            if (Input.GetMouseButtonUp(0))
            {
                if (anim.state == FishingRodAnimation.RodState.Reeling)
                {
                    AudioManager.instance.StopPlaying("Reel");
                    anim.SetState(FishingRodAnimation.RodState.Resting);
                }
            }

            if (anim.state == FishingRodAnimation.RodState.Reeling)
            {
                AudioManager.instance.PlaySound("Reel", true);
                hook.Reel(scriptable.reelSpeed);
                if (Vector2.Distance(hook.transform.position, hook.GetHookAnchorPoint().position) <= reeledInDistance)
                {
                    AudioManager.instance.StopPlaying("Reel");
                    if (hook.GetComponent<HookBehaviour>().hookedObject.GetComponent<BaitBehaviour>() == null)
                    {
                        hook.GetComponent<HookBehaviour>().AddToBucket();
                    }
                    anim.SetState(FishingRodAnimation.RodState.Resting);
                    casted = false;
                    UIManager.instance.bucketMenuButton.gameObject.SetActive(true);
                    UIManager.instance.inventoryMenuButton.SetActive(true);
                }
            }
        }


        public void Cast(float _angle, float _strength)
        {
            anim.SetState(FishingRodAnimation.RodState.CastForward);
            casted = true;
            hook.Cast(_angle, _strength);
        }
        public HookBehaviour GetHook() => hook;
        public float GetLineLength() => scriptable.lineLength;
        public string GetDescription() => scriptable.description;

        private void OnDestroy() => hook.Despawn();
    }

}