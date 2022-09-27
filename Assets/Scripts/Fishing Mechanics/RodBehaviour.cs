using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.Fishables;
using UnityEngine.InputSystem;
using Fishing.IO;

namespace Fishing.FishingMechanics
{
    [RequireComponent(typeof(RodScriptable))]
    public class RodBehaviour : MonoBehaviour
    {
        public Sprite inventorySprite;
        public RodScriptable scriptable;

        public BaitBehaviour equippedBait;

        [SerializeField] private Transform linePivotPoint;
        public bool casted = false;
        [SerializeField] private float reeledInDistance = 0.1f;
        [SerializeField] private HookBehaviour hook;
        private Animator anim;
        [SerializeField] private Animator playerAnim;

        [SerializeField] private List<Transform> idleAnimationPositions;
        [SerializeField] private List<Transform> startCastAnimationPositions;
        [SerializeField] private List<Transform> castAnimationPositions;
        [SerializeField] private List<Transform> reelingAnimationPositions;

        private Controls _controls;

        private RodManager rodManager;

        private void Awake()
        {
            rodManager = RodManager.instance;
            anim = GetComponent<Animator>();

            _controls = new Controls();
            _controls.FishingLevelInputs.Enable();
            _controls.FishingLevelInputs.StartCast.performed += StartCast;
            _controls.FishingLevelInputs.StartReeling.performed += StartReeling;
            _controls.FishingLevelInputs.StopReeling.performed += StopReeling;
        }

        void Start()
        {
            casted = false;
            playerAnim = rodManager.GetComponent<Animator>();
        }

        void Update()
        {
            if (anim.GetBool("isReeling"))
            {
                AudioManager.instance.PlaySound("Reel", true);
                hook.Reel(scriptable.reelSpeed);
                if (Vector2.Distance(hook.transform.position, hook.GetHookAnchorPoint().position) <= reeledInDistance)
                {
                    AudioManager.instance.StopPlaying("Reel");
                    if (hook.hookedObject != null)
                    {
                        if (hook.hookedObject.GetComponent<BaitBehaviour>() == null)
                        {
                            hook.AddToBucket();
                        }
                    }
                    anim.SetBool("isReeling", false);
                    playerAnim.SetBool("isReeling", false);
                    casted = false;
                    UIManager.instance.bucketMenuButton.gameObject.SetActive(true);
                    UIManager.instance.inventoryMenuButton.SetActive(true);
                }
            }
        }

        private void StartReeling(InputAction.CallbackContext _context)
        {
            if (!_context.performed) return;
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")) return;
            if (!casted) return;

            if (hook.transform.position.y <= 0f)
            {
                anim.SetBool("isReeling", true);
                playerAnim.SetBool("isReeling", true);
            }
        }

        private void StopReeling(InputAction.CallbackContext _context)
        {
            if (!_context.performed) return;
            if (!anim.GetBool("isReeling")) return;

            AudioManager.instance.StopPlaying("Reel");
            anim.SetBool("isReeling", false);
            playerAnim.SetBool("isReeling", false);
        }

        private void StartCast(InputAction.CallbackContext _context)
        {
            if (!_context.performed) return;
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")) return;
            if (UIManager.instance.mouseOverUI || UIManager.instance.IsActiveUI()) return;
            if (casted) return;

            anim.SetTrigger("startCast");
            playerAnim.SetTrigger("startCast");
            UIManager.instance.bucketMenuButton.gameObject.SetActive(false);
            UIManager.instance.inventoryMenuButton.SetActive(false);
            PowerAndAngle.instance.StartCharging(scriptable.chargeFrequency, scriptable.minCastStrength, scriptable.maxCastStrength,
                scriptable.maxCastAngle, scriptable.angleFrequency);
        }

        public void Cast(float _angle, float _strength)
        {
            anim.SetTrigger("cast");
            playerAnim.SetTrigger("cast");
            casted = true;
            hook.Cast(_angle, _strength);
        }

        public void IdleLineAnchorPosition(int _index)
        {
            linePivotPoint.position = idleAnimationPositions[_index].position;
        }
        public void StartCastLineAnchorPosition(int _index)
        {
            linePivotPoint.position = startCastAnimationPositions[_index].position;
        }
        public void CastLineAnchorPosition(int _index)
        {
            linePivotPoint.position = castAnimationPositions[_index].position;
        }
        public void ReelingLineAnchorPosition(int _index)
        {
            Debug.Log("test");
            linePivotPoint.position = reelingAnimationPositions[_index].position;
        }

        public HookBehaviour GetHook() => hook;
        public float GetLineLength() => scriptable.lineLength;
        public string GetDescription() => scriptable.description;

        private void OnDestroy() => hook.Despawn();

        private void OnEnable()
        {
            _controls.FishingLevelInputs.StartCast.performed += StartCast;
            _controls.FishingLevelInputs.StartReeling.performed += StartReeling;
            _controls.FishingLevelInputs.StopReeling.performed += StopReeling;
        }

        private void OnDisable()
        {
            _controls.FishingLevelInputs.StartCast.performed -= StartCast;
            _controls.FishingLevelInputs.StartReeling.performed -= StartReeling;
            _controls.FishingLevelInputs.StopReeling.performed -= StopReeling;
        }
    }

}