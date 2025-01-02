using Fishing.Fishables;
using Fishing.Fishables.Fish;
using Fishing.IO;
using Fishing.PlayerInput;
using Fishing.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.FishingMechanics.Minigame {
    public class ReelingMinigame : MonoBehaviour {
        [SerializeField, Min(0), Tooltip("Represents the maximum X value in pixels that the reel bar encapsulates.")] private float _reelBarMaxX = 1124f; // TODO: Automatically determine this off of the side of the UI element's bounds.
        public float ReelBarMaxX { get => _reelBarMaxX; private set => _reelBarMaxX = value; }

        [SerializeField, Tooltip("Text UI element that shows the line's strength.")] private Text _lineStrengthText;
        [SerializeField, Tooltip("Text UI element that shows the name of the fish.")] private Text _fishNameText;
        [SerializeField, Tooltip("Text UI element taht shows the strength of the fishable.")] private Text _fishStrengthText;

        [SerializeField, Tooltip("Game object of the reeling bar sprite.")] private GameObject _reelingBarOutline;
        private LineStress _stressBar;
        private ReelZone _reelZone;
        private MinigameFish _fishIcon;
        private DistanceBar _distanceBar;

        private bool _isReeling;
        public bool IsReeling { get => _isReeling; private set => _isReeling = value; }

        private bool _isInMinigame = false;
        public bool IsInMinigame { get => _isInMinigame; private set => _isInMinigame = value; }

        private RodBehaviour _minigameRod;
        public RodBehaviour MinigameRod { get => _minigameRod; private set { _minigameRod = value; } }

        private RodScriptable _minigameRodScriptable;
        public RodScriptable MinigameRodScriptable { get => _minigameRodScriptable;  private set { _minigameRodScriptable = value; } }

        private Fishable _hookedFishable;
        public Fishable HookedFishable { get => _hookedFishable; private set { _hookedFishable = value; } }

        private FishableScriptable _hookedFishableSO;
        public FishableScriptable HookedFishableSO { get => _hookedFishableSO; private set { _hookedFishableSO = value; } }

        private RodManager _rodManager;

        private static ReelingMinigame _instance;
        public static ReelingMinigame Instance { get => _instance; private set => _instance = value; }

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            _stressBar = LineStress.Instance;
            _reelZone = ReelZone.Instance;
            _fishIcon = MinigameFish.Instance;
            _distanceBar = DistanceBar.Instance;

            _rodManager = RodManager.Instance;

            IsInMinigame = false;
            ShowMinigame(false);
        }

        public void InitiateMinigame(Fishable fishable) {
            MinigameRod = _rodManager.EquippedRod;
            MinigameRodScriptable = MinigameRod.RodScriptable;
            HookedFishable = fishable;
            HookedFishableSO = HookedFishable.FishableScriptable;

            ShowMinigame(true);
            InitializeMinigameScripts();
            PopulateUIText();
            SetInputs();
            IsInMinigame = true;

            if (!SaveManager.Instance.LoadedPlayerData.HasSeenTutorialData.ReelingMinigameTutorial) {
                HandleTutorial();
            }
        }

        private void InitializeMinigameScripts() {
            _fishIcon.InitializeMinigame();
            _reelZone.InitializeMinigame();
            _stressBar.InitializeMinigame();
            _distanceBar.InitializeMinigame();
        }

        private void PopulateUIText() {
            _lineStrengthText.text = $"Line STR - {MinigameRod.RodScriptable.LineStrength.ToString()}";
            _fishNameText.text = HookedFishable.FishableScriptable.ItemName;
            _fishStrengthText.text = $"Diff - x{HookedFishable.Difficulty.ToString("F2")}";
        }

        private void SetInputs() {
            IsReeling = false;
            MinigameRod.ClearReelInputs();
            InputManager.OnCastReel += StartReeling;
            InputManager.ReleaseCastReel += EndReeling;
        }

        private void HandleTutorial() {
            TutorialSystem.Instance.QueueTutorial("When you hook something, the reeling minigame starts. Move the green \"reeling zone\" with the Left Mouse Button to cover the fish icon to start reeling it in.");
            TutorialSystem.Instance.QueueTutorial("Your line can snap under too much stress, so pay attention to the bar's color!");
            TutorialSystem.Instance.QueueTutorial("Reeling will cause some stress, but not as much when the fish is in the reeling zone. Stronger lines can handle stronger fish.");
            TutorialSystem.Instance.QueueTutorial("The fish will try and swim away (splash icon) before entering a rest period. Reeling while it's swimming will induce more stress.");
            TutorialSystem.Instance.QueueTutorial("If you find a fish too difficult, upgrade your line, fishing rod, or try going after a smaller sized fish. Reel while the fish is resting, and good luck!");
            SaveManager.Instance.LoadedPlayerData.HasSeenTutorialData.ReelingMinigameTutorial = true;
        }

        private void StartReeling() {
            IsReeling = true;
        }

        private void EndReeling() {
            IsReeling = false;
        }

        public void OnLineSnap() {
            MinigameRod.Hook.DestroyHookedObject(); // TODO: Remove this Destroy call after checking to see if fishables are functional when released from the hook
            MinigameRod.Hook.HookedObject = null;
            MinigameRod.StopReeling();

            InputManager.OnCastReel -= StartReeling;
            InputManager.ReleaseCastReel -= EndReeling;
            MinigameRod.OnReeledIn();

            EndMinigame();
        }


        private void ShowMinigame(bool _show) { // TODO: Remove this code after making this script no longer required to be active in order to start the minigame.
            _reelingBarOutline.SetActive(_show);
            _stressBar.gameObject.SetActive(_show);
            _reelZone.gameObject.SetActive(_show);
            _fishIcon.gameObject.SetActive(_show);
            _distanceBar.gameObject.SetActive(_show);
            _lineStrengthText.gameObject.SetActive(_show);
            _fishNameText.gameObject.SetActive(_show);
            _fishStrengthText.gameObject.SetActive(_show);
        }

        public void EndMinigame() {
            IsInMinigame = false;
            ShowMinigame(false);
        }
    }
}
