using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.Fishables;
using Fishing.IO;
using UnityEngine.UI;

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
