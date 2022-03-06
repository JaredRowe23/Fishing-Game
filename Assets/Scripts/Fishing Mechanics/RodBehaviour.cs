using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing
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
        private FishingRodAnimation _anim;
        private FishingRodStats _stats;

        private void Awake()
        {
            _anim = GetComponent<FishingRodAnimation>();
            _stats = GetComponent<FishingRodStats>();
        }

        void Start()
        {
            casted = false;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_anim.state == FishingRodAnimation.RodState.Resting)
                {
                    if (casted)
                    {
                        if (hook.transform.position.y <= 0f)
                        {
                            _anim.SetState(FishingRodAnimation.RodState.Reeling);
                        }
                    }
                    else if (!GameController.instance.mouseOverUI && !GameController.instance.IsActiveUI())
                    {
                        _anim.SetState(FishingRodAnimation.RodState.CastPull);
                        GameController.instance.bucketMenuButton.gameObject.SetActive(false);
                        GameController.instance.inventoryMenuButton.gameObject.SetActive(false);

                        // Show the power slider and begin the process for charging/angling our cast
                        PowerAndAngle.instance.StartCharging(_stats.GetChargeFrequency(),_stats.GetMinCastStrength(), _stats.GetMaxCastStrength(),
                            _stats.GetMaxCastAngle(), _stats.GetAngleFrequency());
                    }
                }
            }

            // Stop reeling when not holding click
            if (Input.GetMouseButtonUp(0))
            {
                if (_anim.state == FishingRodAnimation.RodState.Reeling)
                {
                    AudioManager.instance.StopPlaying("Reel");
                    _anim.SetState(FishingRodAnimation.RodState.Resting);
                }
            }

            if (_anim.state == FishingRodAnimation.RodState.Reeling)
            {
                AudioManager.instance.PlaySound("Reel", true);
                hook.Reel(_stats.GetReelSpeed());
                if (Vector3.Distance(hook.transform.position, hook.GetHookAnchorPoint().position) <= reeledInDistance)
                {
                    hook.GetComponent<HookBehaviour>().AddToBucket();
                    _anim.SetState(FishingRodAnimation.RodState.Resting);
                    casted = false;
                    GameController.instance.bucketMenuButton.gameObject.SetActive(true);
                    GameController.instance.inventoryMenuButton.gameObject.SetActive(true);
                }
            }
        }


        public void Cast(float _angle, float _strength)
        {
            _anim.SetState(FishingRodAnimation.RodState.CastForward);
            casted = true;
            hook.Cast(_angle, _strength);
        }
        public HookBehaviour GetHook() => hook;
        public float GetLineLength() => _stats.GetLineLength();
        public string GetDescription() => _stats.GetDescription();
    }

}