using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.FishingMechanics
{
    [RequireComponent(typeof(FishingRodAnimation))]
    [RequireComponent(typeof(FishingRodStats))]
    public class RodBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject rodObject;
        public Sprite inventorySprite;

        public bool casted = false;
        [SerializeField] private float reeledInDistance = 0.1f;
        [SerializeField] private HookBehaviour hook;
        private FishingRodAnimation anim;
        private FishingRodStats stats;

        private void Awake()
        {
            anim = GetComponent<FishingRodAnimation>();
            stats = GetComponent<FishingRodStats>();
        }

        void Start()
        {
            casted = false;
            GameController.instance.equippedRod = this;
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
                        PowerAndAngle.instance.StartCharging(stats.GetChargeFrequency(),stats.GetMinCastStrength(), stats.GetMaxCastStrength(),
                            stats.GetMaxCastAngle(), stats.GetAngleFrequency());
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
                hook.Reel(stats.GetReelSpeed());
                if (Vector3.Distance(hook.transform.position, hook.GetHookAnchorPoint().position) <= reeledInDistance)
                {
                    AudioManager.instance.StopPlaying("Reel");
                    hook.GetComponent<HookBehaviour>().AddToBucket();
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
        public float GetLineLength() => stats.GetLineLength();
        public string GetDescription() => stats.GetDescription();

        private void OnDestroy() => hook.Despawn();
    }

}