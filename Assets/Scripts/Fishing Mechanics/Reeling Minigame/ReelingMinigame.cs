using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.Fishables;
using Fishing.IO;
using UnityEngine.UI;
using Fishing.UI;
using Fishing.PlayerInput;

namespace Fishing.FishingMechanics.Minigame
{
    public class ReelingMinigame : MonoBehaviour
    {
        [SerializeField] private float reelBarMaxX = 1124f;

        [SerializeField] private GameObject reelingBarOutline;

        [SerializeField] private Text lineStrengthText;
        [SerializeField] private Text fishNameText;
        [SerializeField] private Text fishStrengthText;

        private LineStress stressBar;
        private ReelZone reelZone;
        private MinigameFish fishIcon;
        private DistanceBar distanceBar;

        private bool isReeling;

        private bool isInMinigame = false;

        private RodBehaviour equippedRod;
        private Fishable fish;

        public static ReelingMinigame instance;

        private ReelingMinigame() => instance = this;

        private void Awake()
        {
            stressBar = LineStress.instance;
            reelZone = ReelZone.instance;
            fishIcon = MinigameFish.instance;
            distanceBar = DistanceBar.instance;
        }

        private void Start()
        {
            isInMinigame = false;
            ShowMinigame(false);
        }

        public void InitiateMinigame(Fishable _fish)
        {
            ShowMinigame(true);

            equippedRod = RodManager.instance.equippedRod;
            fish = _fish;

            InitializeMinigameScripts();

            PopulateUIText();

            SetInputs();

            isInMinigame = true;

            if (!PlayerData.instance.hasSeenTutorialData.reelingMinigameTutorial) HandleTutorial();
        }

        private void InitializeMinigameScripts()
        {
            fishIcon.InitializeMinigame(fish);
            reelZone.InitializeMinigame();
            stressBar.InitializeMinigame();
            distanceBar.InitializeMinigame(fish);
        }

        private void PopulateUIText()
        {
            lineStrengthText.text = "Line STR - " + equippedRod.scriptable.lineStrength.ToString();
            fishNameText.text = fish.ItemName;
            fishStrengthText.text = "Diff - x" + fish.GetComponent<MinigameStats>().MinigameDifficulty.ToString("F2");
        }

        private void SetInputs()
        {
            isReeling = false;
            equippedRod.ClearReelInputs();
            InputManager.onCastReel += StartReeling;
            InputManager.releaseCastReel += EndReeling;
        }

        private void HandleTutorial()
        {
            TutorialSystem.instance.QueueTutorial("When you hook something, the reeling minigame starts. Move the green \"reeling zone\" with the Left Mouse Button to cover the fish icon to start reeling it in.");
            TutorialSystem.instance.QueueTutorial("Your line can snap under too much stress, so pay attention to the bar's color!");
            TutorialSystem.instance.QueueTutorial("Reeling will cause some stress, but not as much when the fish is in the reeling zone. Stronger lines can handle stronger fish.");
            TutorialSystem.instance.QueueTutorial("The fish will try and swim away (splash icon) before entering a rest period. Reeling while it's swimming will induce more stress.");
            TutorialSystem.instance.QueueTutorial("If you find a fish too difficult, upgrade your line, fishing rod, or try going after a smaller sized fish. Reel while the fish is resting, and good luck!");
            PlayerData.instance.hasSeenTutorialData.reelingMinigameTutorial = true;
        }

        private void StartReeling() => isReeling = true;
        private void EndReeling() => isReeling = false;

        public void OnLineSnap()
        {
            equippedRod.GetHook().DestroyHookedObject();
            equippedRod.GetHook().hookedObject = null;
            equippedRod.StopReeling();

            InputManager.onCastReel -= StartReeling;
            InputManager.releaseCastReel -= EndReeling;
            equippedRod.OnReeledIn();

            EndMinigame();
        }


        private void ShowMinigame(bool _show)
        {
            reelingBarOutline.SetActive(_show);
            stressBar.gameObject.SetActive(_show);
            reelZone.gameObject.SetActive(_show);
            fishIcon.gameObject.SetActive(_show);
            distanceBar.gameObject.SetActive(_show);
            lineStrengthText.gameObject.SetActive(_show);
            fishNameText.gameObject.SetActive(_show);
            fishStrengthText.gameObject.SetActive(_show);
        }

        public void EndMinigame()
        {
            isInMinigame = false;
            ShowMinigame(false);
        }


        public bool IsReeling() => isReeling;
        public bool IsInMinigame() => isInMinigame;
        public float GetXAxisMax() => reelBarMaxX;
    }
}
