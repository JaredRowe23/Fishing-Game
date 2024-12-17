using Fishing.Fishables;
using Fishing.IO;
using Fishing.PlayerCamera;
using Fishing.UI;
using UnityEngine;

namespace Fishing.FishingMechanics {
    public class HookBehaviour : MonoBehaviour {
        private GameObject _hookedObject;
        public GameObject HookedObject { get => _hookedObject; set => _hookedObject = value; }

        [SerializeField, Tooltip("Transform indicating the position of the start of the fishing line.")] private Transform _linePivotPoint;
        public Transform LinePivotPoint { get => _linePivotPoint; set => _linePivotPoint = value; }

        [SerializeField, Tooltip("Transform indicating the position of the end of the fishing line.")] private Transform _hookPivotPoint;
        [SerializeField, Min(0), Tooltip("Time in seconds the hook takes to reset to its hang height below the fishing rod.")] private float _resetTime = 1f;
        [SerializeField, Min(0), Tooltip("Length in meters below the end of the fishing rod the fishing hook hangs.")] private float _hookHangHeight;

        [Header("Water Physics")]
        [SerializeField, Min(0), Tooltip("Drag force the hook experiences while in the air.")] private float _airDrag;
        [SerializeField, Min(0), Tooltip("Drag force the hook experiences while in the water.")] private float _waterDrag;

        private bool _isResetting;
        private bool _hasPlayedSplashAudio;

        private Vector2 _targetPos;
        private float _distanceFromTarget;
        private RodBehaviour _rod;
        private Rigidbody2D _rigidbody;
        private CameraBehaviour _camera;

        private LineRenderer _lineRenderer;


        private void Awake() {
            _lineRenderer = GetComponent<LineRenderer>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _rod = transform.parent.GetComponent<RodBehaviour>();
        }

        private void Start() {
            _camera = CameraBehaviour.Instance;
            _lineRenderer.SetPosition(0, LinePivotPoint.position);
            _rigidbody.isKinematic = true;
            _rigidbody.velocity = Vector3.zero;
        }

        void Update() {
            SetLineRendererPositions();

            if (transform.position.y <= 0f) { 
                OnSubmerged(); 
            }
            else { 
                OnSurfaced(); 
            }

            if (!_rod.Casted) {
                _targetPos = LinePivotPoint.position - new Vector3(0, _hookHangHeight, 0);

                if (!_isResetting) {
                    transform.position = _targetPos;
                }
                else {
                    HandleNotCasted();
                }
            }
            else {
                HandlePhysics();
            }
        }

        private void SetLineRendererPositions() {
            _lineRenderer.SetPosition(0, LinePivotPoint.position);
            _lineRenderer.SetPosition(1, _hookPivotPoint.position);
        }

        private void OnSubmerged() {
            if (!_hasPlayedSplashAudio) {
                AudioManager.instance.PlaySound("Hook Splash");
                _hasPlayedSplashAudio = true;
            }
            _rigidbody.drag = _waterDrag;

            if (!SaveManager.Instance.LoadedPlayerData.HasSeenTutorialData.ReelingTutorial) {
                ShowReelingTutorial();
            }
        }

        private void ShowReelingTutorial() {
            TutorialSystem.instance.QueueTutorial("Hold the left mouse button to begin reeling.");
            TutorialSystem.instance.QueueTutorial("Use A and D or the arrow keys to move the hook left and right slightly");
            SaveManager.Instance.LoadedPlayerData.HasSeenTutorialData.ReelingTutorial = true;
        }

        private void OnSurfaced() {
            _rigidbody.drag = _airDrag;
            _hasPlayedSplashAudio = false;
        }

        private void HandleNotCasted() {
            transform.position = Vector2.MoveTowards(transform.position, _targetPos, _distanceFromTarget / _resetTime * Time.deltaTime);
            float distance = Vector2.Distance(transform.position, _targetPos);
            if (distance == 0f && _isResetting) {
                _rod.AddCastInput();
                _isResetting = false;
            }
        }

        private void HandlePhysics() {
            float _distanceFromPivot = Vector2.Distance(transform.position, LinePivotPoint.position);
            if (_distanceFromPivot >= _rod.Scriptable.lineLength) {
                transform.position += (LinePivotPoint.position - transform.position).normalized * (_distanceFromPivot - _rod.Scriptable.lineLength);
            }
            //else {
                // _rigidbody.gravityScale = 1; May be pointless, as nothing changes this?
                // _rigidbody.isKinematic = false; May also be pointless, as this should already be set to false when casted and only reset when the hook is reeled in or the line's snapped
            //}

            _camera.DesiredPosition = transform.position;
        }

        public void StartResettingHook() {
            _isResetting = true;

            _rigidbody.isKinematic = true;
            _rigidbody.velocity = Vector2.zero;
            _distanceFromTarget = Vector2.Distance(transform.position, LinePivotPoint.position);
        }

        public void Cast(float _angle, float _force) {
            _rigidbody.isKinematic = false;
            Quaternion rot = Quaternion.AngleAxis(_angle, Vector3.forward);
            _rigidbody.AddForce(rot * Vector2.right * _force);
        }

        public void Reel(float _force) {
            _rigidbody.AddForce(_force * Time.deltaTime * Vector3.Normalize(LinePivotPoint.position - transform.position));
        }

        public void SetHook(Fishable _fishable) {
            if (HookedObject != null && !HookedObject.TryGetComponent(out BaitBehaviour _)) { 
                return; 
            }
            if (_isResetting || !_rod.Casted) { 
                return; 
            }

            _fishable.OnHooked();
            HookedObject = _fishable.gameObject;
            HookedObject.transform.position = transform.position;
        }

        public void DestroyHookedObject() {
            if (HookedObject != null) {
                Destroy(HookedObject);
            }
        }

        public bool IsInStartCastPosition() {
            return ((Vector2)transform.position == _targetPos) && _rod.IsInStartingCastPosition();
        }

        /*
        private void OnDestroy() {
            DestroyHookedObject(); // TODO: See if this call is necessary, as the hook behaviour may call the hooked object's OnDestroy anyway
        }
        */
    }

}