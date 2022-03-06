using System.Collections;
using UnityEngine;

namespace Fishing
{
    public class FishingRodAnimation : MonoBehaviour
    {
        [SerializeField] private GameObject targetAnimationObject;

        [SerializeField] private float rotateThreshold = 0.1f;
        [SerializeField] private float castPullRot = 50f;
        [SerializeField] private float castForwardRot = -70f;
        [SerializeField] private float restingRot = -50f;
        [SerializeField] private float reelingRot = -20f;
        [SerializeField] private float castPullStrength = 200f;
        [SerializeField] private float castForwardStrength = 500f;
        [SerializeField] private float restStrength = 100f;
        [SerializeField] private float reelStrength = 150f;
        public enum RodState { CastPull, CastForward, Reeling, Resting };
        public RodState state = RodState.Resting;
        private float targetRot;

        private RodBehaviour rod;

        private void Awake()
        {
            rod = GetComponent<RodBehaviour>();
        }

        private void Start()
        {
            SetState(RodState.Resting);
        }

        private void Update()
        {
            UpdateRotation();
        }

        public void SetState(RodState _state)
        {
            state = _state;
            if (state == RodState.CastForward)
            {
                targetRot = castForwardRot;
            }
            else if (state == RodState.CastPull)
            {
                targetRot = castPullRot;
            }
            else if (state == RodState.Reeling)
            {
                targetRot = reelingRot;
            }
            else if (state == RodState.Resting)
            {
                targetRot = restingRot;
            }
        }

        public void UpdateRotation()
        {
            if (Mathf.Abs(targetRot - ((targetAnimationObject.transform.rotation.eulerAngles.z + 180) % 360 - 180)) >= rotateThreshold)
            {
                Vector3 targetRotation = new Vector3(targetAnimationObject.transform.rotation.x, targetAnimationObject.transform.rotation.y, targetRot);

                switch (state)
                {
                    case RodState.Resting:
                        targetAnimationObject.transform.rotation = Quaternion.RotateTowards(targetAnimationObject.transform.rotation, Quaternion.Euler(targetRotation), restStrength * Time.deltaTime);
                        break;
                    case RodState.Reeling:
                        targetAnimationObject.transform.rotation = Quaternion.RotateTowards(targetAnimationObject.transform.rotation, Quaternion.Euler(targetRotation), reelStrength * Time.deltaTime);
                        break;
                    case RodState.CastPull:
                        targetAnimationObject.transform.rotation = Quaternion.RotateTowards(targetAnimationObject.transform.rotation, Quaternion.Euler(targetRotation), castPullStrength * Time.deltaTime);
                        break;
                    case RodState.CastForward:
                        targetAnimationObject.transform.rotation = Quaternion.RotateTowards(targetAnimationObject.transform.rotation, Quaternion.Euler(targetRotation), castForwardStrength * Time.deltaTime);
                        break;
                }
            }
            else
            {
                if (state == RodState.CastForward)
                {
                    SetState(RodState.Resting);
                }
            }
        }
    }
}