using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.Fishables.Fish;
using Fishing.Fishables;
using Fishing.Inventory;
using Fishing.UI;
using Fishing.IO;
using Fishing.PlayerCamera;

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
        [SerializeField] private float lineRotationAngle = -5f;

        public bool playedSplash;

        private Vector2 _targetPos;
        private RodBehaviour _rod;
        private Rigidbody2D _rb;

        private LineRenderer lineRenderer;

        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        private void Start()
        {
            _rod = transform.parent.GetComponent<RodBehaviour>();
            _rb = this.GetComponent<Rigidbody2D>();

            lineRenderer.SetPosition(0, linePivotPoint.position);
        }

        void Update()
        {
            lineRenderer.SetPosition(0, linePivotPoint.position);
            lineRenderer.SetPosition(1, hookPivotPoint.position);

            if (transform.position.y <= 0f)
            {
                OnSubmerged();
            }
            else
            {
                OnSurfaced();
            }

            if (!_rod.casted)
            {
                _rb.isKinematic = true;
                _rb.velocity = Vector2.zero;
                _targetPos = linePivotPoint.position - new Vector3(0, hookHangHeight, 0);
                transform.position = Vector2.MoveTowards(transform.position, _targetPos, resetSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, _targetPos) == 0f && _rod.isResettingHook)
                {
                    _rod.AddCastInput();
                    _rod.isResettingHook = false;
                }
                return;
            }

            HandlePhysics();
        }

        private void HandlePhysics()
        {
            if (Vector2.Distance(transform.position, linePivotPoint.position) >= _rod.GetLineLength())
            {
                if (transform.position.y < 0f)
                {
                    _rb.gravityScale = 0;
                    _rb.isKinematic = false;
                    if (transform.position.x - linePivotPoint.position.x >= 0f)
                    {
                        transform.RotateAround(linePivotPoint.position, Vector3.forward, lineRotationAngle * Time.deltaTime);
                        transform.rotation = Quaternion.identity;
                    }
                }

                else
                {
                    _rb.velocity = new Vector2(0f, _rb.velocity.y);
                }
            }
            else
            {
                _rb.gravityScale = 1;
                _rb.isKinematic = false;
            }

            CameraBehaviour.instance.SetDesiredPosition(transform.position);
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
            _rb.drag = waterDrag;

            if (PlayerData.instance.hasSeenReelingTut) return;
            TutorialSystem.instance.QueueTutorial("Hold the left mouse button to begin reeling.");
            TutorialSystem.instance.QueueTutorial("Use A and D or the arrow keys to move the hook left and right slightly");
            PlayerData.instance.hasSeenReelingTut = true;
        }
        private void OnSurfaced()
        {
            _rb.drag = drag;
            playedSplash = false;
        }

        public void Cast(float _angle, float _force)
        {
            _rb.isKinematic = false;
            Quaternion rot = Quaternion.AngleAxis(_angle, Vector3.forward);
            _rb.AddForce(rot * Vector2.right * _force);
        }

        public void Reel(float _force) => _rb.AddForce(_force * Time.deltaTime * Vector3.Normalize(linePivotPoint.position - transform.position));

        public Transform GetHookAnchorPoint() => linePivotPoint;

        public void SetHook(Fishable _fishable)
        {
            if (hookedObject != null) return;
            if (_rod.isResettingHook) return;

            _fishable.OnHooked(transform);
            hookedObject = _fishable.gameObject;
            hookedObject.transform.position = transform.position;

            ReelingMinigame.instance.InitiateMinigame(_fishable);

            //if (PlayerData.instance.hasSeenFishTut) return;
            //TutorialSystem.instance.QueueTutorial("You've hooked something! Reel it back in to catch it!");
            //PlayerData.instance.hasSeenFishTut = true;
        }

        public void DespawnHookedObject()
        {
            if (hookedObject != null) hookedObject.GetComponent<IEdible>().Despawn();
        }

        public void AddToBucket()
        {
            if (hookedObject == null) return;

            BucketBehaviour.instance.AddToBucket(hookedObject.GetComponent<Fishable>());

            if (PlayerData.instance.hasSeenBucketTut) return;
            TutorialSystem.instance.QueueTutorial("Press B or click the bucket icon in the top-left corner to access your bucket");
            PlayerData.instance.hasSeenBucketTut = true;
        }

        public bool IsInStartCastPosition() => ((Vector2)transform.position == _targetPos) && _rod.IsInStartingCastPosition();
    }

}