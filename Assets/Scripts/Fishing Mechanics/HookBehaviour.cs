using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.Fishables.Fish;
using Fishing.Fishables;
using Fishing.Inventory;
using Fishing.UI;
using Fishing.IO;
using Fishing.PlayerCamera;
using Fishing.FishingMechanics.Minigame;

namespace Fishing.FishingMechanics
{
    public class HookBehaviour : MonoBehaviour, IEdible
    {
        public GameObject hookedObject;

        [SerializeField] private Transform linePivotPoint;
        [SerializeField] private Transform hookPivotPoint;
        [SerializeField] private float resetSpeed;
        [SerializeField] private float hookHangHeight;

        [Header("Water Physics")]
        [SerializeField] private float drag;
        [SerializeField] private float waterDrag;

        private bool playedSplash;

        private Vector2 targetPos;
        private RodBehaviour rod;
        private Rigidbody2D rb;
        private CameraBehaviour cam;

        private LineRenderer lineRenderer;

        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        private void Start()
        {
            rod = transform.parent.GetComponent<RodBehaviour>();
            rb = GetComponent<Rigidbody2D>();
            cam = CameraBehaviour.instance;

            lineRenderer.SetPosition(0, linePivotPoint.position);
        }

        void Update()
        {
            SetLineRendererPositions();

            if (transform.position.y <= 0f) OnSubmerged();
            else OnSurfaced();

            if (!rod.casted)
            {
                HandleNotCasted();
                return;
            }

            HandlePhysics();
        }

        private void SetLineRendererPositions()
        {
            lineRenderer.SetPosition(0, linePivotPoint.position);
            lineRenderer.SetPosition(1, hookPivotPoint.position);
        }

        private void HandleNotCasted()
        {
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
            targetPos = linePivotPoint.position - new Vector3(0, hookHangHeight, 0);
            transform.position = Vector2.MoveTowards(transform.position, targetPos, resetSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, targetPos) == 0f && rod.isResettingHook)
            {
                rod.AddCastInput();
                rod.isResettingHook = false;
            }
        }

        private void HandlePhysics()
        {
            float _distanceFromPivot = Vector2.Distance(transform.position, linePivotPoint.position);
            if (_distanceFromPivot >= rod.GetLineLength())
            {
                transform.position += (linePivotPoint.position - transform.position).normalized * (_distanceFromPivot - rod.GetLineLength());
            }
            else
            {
                rb.gravityScale = 1;
                rb.isKinematic = false;
            }

            cam.SetDesiredPosition(transform.position);
        }

        public void Despawn()
        {
            DespawnHookedObject();
            FoodSearchManager.instance.RemoveFood(GetComponent<Edible>());
        }

        private void OnSubmerged()
        {
            if (!playedSplash)
            {
                AudioManager.instance.PlaySound("Hook Splash");
                playedSplash = true;
            }
            rb.drag = waterDrag;

            if (!PlayerData.instance.hasSeenTutorialData.reelingTutorial) ShowReelingTutorial();
        }

        private void ShowReelingTutorial()
        {
            TutorialSystem.instance.QueueTutorial("Hold the left mouse button to begin reeling.");
            TutorialSystem.instance.QueueTutorial("Use A and D or the arrow keys to move the hook left and right slightly");
            PlayerData.instance.hasSeenTutorialData.reelingTutorial = true;
        }

        private void OnSurfaced()
        {
            rb.drag = drag;
            playedSplash = false;
        }

        public void Cast(float _angle, float _force)
        {
            rb.isKinematic = false;
            Quaternion rot = Quaternion.AngleAxis(_angle, Vector3.forward);
            rb.AddForce(rot * Vector2.right * _force);
        }

        public void Reel(float _force) => rb.AddForce(_force * Time.deltaTime * Vector3.Normalize(linePivotPoint.position - transform.position));

        public Transform GetHookAnchorPoint() => linePivotPoint;

        public void SetHook(Fishable _fishable)
        {
            if (hookedObject != null) return;
            if (rod.isResettingHook) return;

            _fishable.OnHooked(transform);
            hookedObject = _fishable.gameObject;
            hookedObject.transform.position = transform.position;

            ReelingMinigame.instance.InitiateMinigame(_fishable);
        }

        public void DespawnHookedObject()
        {
            if (hookedObject != null) hookedObject.GetComponent<IEdible>().Despawn();
        }

        public bool IsInStartCastPosition() => ((Vector2)transform.position == targetPos) && rod.IsInStartingCastPosition();
    }

}