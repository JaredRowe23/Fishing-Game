using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.Fishables;
using Fishing.IO;
using UnityEngine.UI;
using Fishing.UI;

namespace Fishing.FishingMechanics
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

            fishIcon.InitializeMinigame(_fish);
            reelZone.InitializeMinigame();
            stressBar.InitializeMinigame();
            distanceBar.InitializeMinigame(_fish);

            lineStrengthText.text = "Line STR - " + equippedRod.scriptable.lineStrength.ToString();
            fishNameText.text = _fish.GetName();
            fishStrengthText.text = "Diff - x" + _fish.GetMinigameDifficulty().ToString("F2");

            isReeling = false;
            equippedRod.ClearReelInputs();
            InputManager.onCastReel += StartReeling;
            InputManager.releaseCastReel += EndReeling;

            isInMinigame = true;

            if (PlayerData.instance.hasSeenReelingMinigameTut) return;
            TutorialSystem.instance.QueueTutorial("When something gets caught on your hook, this initiates the reeling minigame.");
            TutorialSystem.instance.QueueTutorial("Your goal is to move the green \"reeling zone\" with the Left Mouse Button to cover the fish icon with it, which will start reeling it in.");
            TutorialSystem.instance.QueueTutorial("Be careful, because your line can only handle so much stress and eventually will snap, losing the fish and resetting you back to casting again. Pay attention to the color of the background!");
            TutorialSystem.instance.QueueTutorial("Reeling will cause some amount of stress, but not as much when the fish is in the reeling zone. The amount of stress is based on the difference between your line's strength, and the strength of the fish.");
            TutorialSystem.instance.QueueTutorial("The fish will try and swim away (shown by the splash icon around it) before entering a rest period. While it's swimming, it'll move further away, and reeling while it's swimming will induce more stress than normal.");
            TutorialSystem.instance.QueueTutorial("If you find a fish too hard to reel in, you may have to either upgrade your line, fishing rod, or try going after a smaller sized fish. Be patient to reel while the fish is resting, and good luck!");
            PlayerData.instance.hasSeenReelingMinigameTut = true;
        }

        private void StartReeling() => isReeling = true;
        private void EndReeling() => isReeling = false;

        public void OnLineSnap()
        {
            equippedRod.GetHook().DespawnHookedObject();
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
