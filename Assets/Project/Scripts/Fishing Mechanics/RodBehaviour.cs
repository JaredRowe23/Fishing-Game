using Fishing.Fishables;
using Fishing.FishingMechanics.Minigame;
using Fishing.Inventory;
using Fishing.IO;
using Fishing.PlayerCamera;
using Fishing.PlayerInput;
using Fishing.UI;
using System.Collections.Generic;
using UnityEngine;
using Fishing.Audio;

namespace Fishing.FishingMechanics {
    public class RodBehaviour : MonoBehaviour {
        [SerializeField, Tooltip("Scriptable object that contains the stats for this type of fishing rod.")] private RodScriptable _rodScriptable;
        public RodScriptable RodScriptable { get => _rodScriptable; private set => _rodScriptable = value; }

        private BaitBehaviour _equippedBait;
        public BaitBehaviour EquippedBait { get => _equippedBait; set => _equippedBait = value; }

        [SerializeField, Tooltip("Transform that marks the position the fishing line starts from (the end of the fishing rod).")] private Transform _linePivotPoint;
        public Transform LinePivotPoint { get => _linePivotPoint; set => _linePivotPoint = value; }

        private bool _casted = false;
        public bool Casted { get => _casted; private set => _casted = value; }

        [SerializeField, Tooltip("Game object for the fishing rod's hook.")] private HookBehaviour _hook;
        public HookBehaviour Hook { get => _hook; private set => _hook = value; }

        private Animator _animator;
        private Animator _playerAnimator;

        [SerializeField, Tooltip("Transforms that hold the positions of the fishing line start (end of the rod) in each idle animation frame.")] private List<Transform> _idleAnimationPositions;
        [SerializeField, Tooltip("Transforms that hold the positions of the fishing line start (end of the rod) in each start cast animation frame.")] private List<Transform> _startCastAnimationPositions;
        [SerializeField, Tooltip("Transforms that hold the positions of the fishing line start (end of the rod) in each cast animation frame.")] private List<Transform> _castAnimationPositions;
        [SerializeField, Tooltip("Transforms that hold the positions of the fishing line start (end of the rod) in each reeling animation frame.")] private List<Transform> _reelingAnimationPositions;

        private RodManager _rodManager;
        private BaitManager _baitManager;
        private CameraBehaviour _camera;
        private BucketBehaviour _bucket;
        private ReelingMinigame _reelingMinigame;
        private AudioManager _audioManager;
        private UIManager _UIManager;
        private PowerAndAngle _powerAndAngle;
        private PlayerData _playerData;
        private TooltipSystem _tooltipSystem;
        private TutorialSystem _tutorialSystem;

        private void Awake() {
            _animator = GetComponent<Animator>();
            InputManager.OnCastReel += StartCast;
        }

        void Start() {
            _rodManager = RodManager.Instance;
            _baitManager = BaitManager.Instance;
            _camera = CameraBehaviour.Instance;
            _bucket = BucketBehaviour.Instance;
            _reelingMinigame = ReelingMinigame.Instance;
            _audioManager = AudioManager.Instance;
            _UIManager = UIManager.Instance;
            _powerAndAngle = PowerAndAngle.Instance;
            _playerData = SaveManager.Instance.LoadedPlayerData;
            _tooltipSystem = TooltipSystem.Instance;
            _tutorialSystem = TutorialSystem.Instance;

            Casted = false;
            _playerAnimator = _rodManager.GetComponent<Animator>();

            if (_playerData.HasSeenTutorialData.CastTutorial) {
                return;
            }

            ShowCastingTutorial();
        }

        void Update() {
            if (_animator.GetBool("isReeling")) {
                Reel();
            }
        }

        private void Reel() {
            _audioManager.PlaySound("Reel", true);
            Hook.Reel(RodScriptable.ReelForce);

            Vector2 _waterSurfaceUnderRodPosition = new Vector2(Hook.LinePivotPoint.position.x, 0f);
            if (Vector2.Distance(Hook.transform.position, _waterSurfaceUnderRodPosition) <= RodScriptable.ReeledInDistance) {
                OnReeledIn();
            }
        }

        public void OnReeledIn() {
            AddCatch();

            _reelingMinigame.EndMinigame();

            _audioManager.StopPlaying("Reel");

            _animator.SetBool("isReeling", false);
            _playerAnimator.SetBool("isReeling", false);

            _camera.ReturnHome();

            _UIManager.ShowHUDButtons();

            Casted = false;
            Hook.StartResettingHook();
        }

        private void AddCatch() {
            if (Hook.HookedObject == null) {
                return;
            }
            if (Hook.HookedObject.GetComponent<BaitBehaviour>()) {
                return;
            }

            _bucket.AddToBucket(_rodManager.EquippedRod.Hook.HookedObject.GetComponent<Fishable>());
        }

        private void ShowCastingTutorial() {
            _tutorialSystem.QueueTutorial("Hold the left mouse button to begin casting.");
        }

        public void StartReeling() {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
                return;
            }
            if (!Casted) {
                return;
            }

            if (Hook.transform.position.y <= 0f) {
                _animator.SetBool("isReeling", true);
                _playerAnimator.SetBool("isReeling", true);
            }
        }

        public void StopReeling() {
            if (!_animator.GetBool("isReeling")) {
                return;
            }

            _audioManager.StopPlaying("Reel");

            _animator.SetBool("isReeling", false);
            _playerAnimator.SetBool("isReeling", false);
        }

        private void StartCast() {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
                return;
            }
            if (DisableCastOnHover.IsHoveringUI || _UIManager.IsActiveUI() || _tutorialSystem.TutorialListings.content.gameObject.activeSelf) {
                return;
            }
            if (Casted) {
                return;
            }

            _animator.SetTrigger("startCast");
            _playerAnimator.SetTrigger("startCast");

            _UIManager.HideHUDButtons();

            InputManager.OnCastReel -= StartCast;

            _powerAndAngle.StartAngling();
        }

        public void Cast(float angle, float strength) {
            Hook.Cast(angle, strength);

            _animator.SetTrigger("cast");
            _playerAnimator.SetTrigger("cast");

            _camera.LockPlayerControls = false;
            AddReelInputs();

            Casted = true;
        }

        // Used in animation events to update the line's pivot point throughout keyframes
        public void IdleLineAnchorPosition(int _index) {
            LinePivotPoint.position = _idleAnimationPositions[_index].position;
        }
        public void StartCastLineAnchorPosition(int _index) {
            LinePivotPoint.position = _startCastAnimationPositions[_index].position;
        }
        public void CastLineAnchorPosition(int _index) {
            LinePivotPoint.position = _castAnimationPositions[_index].position;
        }
        public void ReelingLineAnchorPosition(int _index) {
            LinePivotPoint.position = _reelingAnimationPositions[_index].position;
        }

        public bool IsInStartingCastPosition() {
            return LinePivotPoint.position == _startCastAnimationPositions[3].position;
        }

        public void ClearReelInputs() {
            InputManager.OnCastReel -= StartReeling;
            InputManager.ReleaseCastReel -= StopReeling;
        }

        public void AddReelInputs() {
            InputManager.OnCastReel += StartReeling;
            InputManager.ReleaseCastReel += StopReeling;
        }

        public void AddCastInput() {
            InputManager.OnCastReel += StartCast;
        }

        private void OnDestroy()
        {
            //Destroy(hook); // TODO: See if this call is necessary, as the rod behaviour may call the hook's OnDestroy anyway
            InputManager.OnCastReel -= StartCast;
        } 
    }

}