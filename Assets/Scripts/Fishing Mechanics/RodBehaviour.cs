using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.Fishables;
using Fishing.IO;
using Fishing.UI;
using Fishing.PlayerCamera;

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

        public bool isResettingHook = false;

        private RodManager rodManager;
        private CameraBehaviour cam;

        private void Awake()
        {
            rodManager = RodManager.instance;
            anim = GetComponent<Animator>();
            cam = CameraBehaviour.instance;

            InputManager.onCastReel += StartCast;
        }

        void Start()
        {
            casted = false;
            playerAnim = rodManager.GetComponent<Animator>();

            if (PlayerData.instance.hasSeenCastTut) return;
            TutorialSystem.instance.QueueTutorial("Hold the left mouse button to begin casting.");
        }

        void Update()
        {
            if (anim.GetBool("isReeling"))
            {
                AudioManager.instance.PlaySound("Reel", true);
                hook.Reel(scriptable.reelSpeed);
                if (Vector2.Distance(hook.transform.position, hook.GetHookAnchorPoint().position) <= reeledInDistance) OnReeledIn();
            }
        }

        public void StartReeling()
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")) return;
            if (!casted) return;

            if (hook.transform.position.y <= 0f)
            {
                anim.SetBool("isReeling", true);
                playerAnim.SetBool("isReeling", true);
            }
        }

        public void StopReeling()
        {
            if (!anim.GetBool("isReeling")) return;

            AudioManager.instance.StopPlaying("Reel");
            anim.SetBool("isReeling", false);
            playerAnim.SetBool("isReeling", false);
        }

        private void StartCast()
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")) return;
            if (UIManager.instance.mouseOverUI || UIManager.instance.IsActiveUI() || TutorialSystem.instance.content.activeSelf) return;
            if (casted) return;

            anim.SetTrigger("startCast");
            playerAnim.SetTrigger("startCast");
            UIManager.instance.bucketMenuButton.gameObject.SetActive(false);
            UIManager.instance.inventoryMenuButton.SetActive(false);
            InputManager.onCastReel -= StartCast;
            PowerAndAngle.instance.StartAngling();
        }

        public void Cast(float _angle, float _strength)
        {
            anim.SetTrigger("cast");
            playerAnim.SetTrigger("cast");
            casted = true;
            hook.Cast(_angle, _strength);
            cam.SetToPlayerZoom();
            InputManager.onCastReel += StartReeling;
            InputManager.releaseCastReel += StopReeling;
        }

        public void OnReeledIn()
        {
            AudioManager.instance.StopPlaying("Reel");
            if (hook.hookedObject != null)
            {
                if (hook.hookedObject.GetComponent<BaitBehaviour>() == null)
                {
                    hook.AddToBucket();
                    for (int i = 0; i < PlayerData.instance.fishingRods.Count; i++)
                    {
                        if (PlayerData.instance.fishingRods[i] != scriptable.rodName) continue;
                        if (PlayerData.instance.equippedBaits[i] == "") break;

                        if (PlayerData.instance.baitCounts[i] <= 0)
                        {
                            TooltipSystem.instance.NewTooltip(3, "Out of bait: " + PlayerData.instance.equippedBaits[i]);
                            PlayerData.instance.equippedBaits[i] = "";
                            PlayerData.instance.bait.RemoveAt(i);
                            PlayerData.instance.baitCounts.RemoveAt(i);
                        }
                        else
                        {
                            PlayerData.instance.baitCounts[i]--;
                            rodManager.SpawnBait();
                        }
                        break;
                    }
                }
            }
            ReelingMinigame.instance.EndMinigame();
            anim.SetBool("isReeling", false);
            playerAnim.SetBool("isReeling", false);
            casted = false;
            cam.ReturnHome();
            isResettingHook = true;
            UIManager.instance.bucketMenuButton.gameObject.SetActive(true);
            UIManager.instance.inventoryMenuButton.SetActive(true);
        }

        public void IdleLineAnchorPosition(int _index) => linePivotPoint.position = idleAnimationPositions[_index].position;
        public void StartCastLineAnchorPosition(int _index) => linePivotPoint.position = startCastAnimationPositions[_index].position;
        public void CastLineAnchorPosition(int _index) => linePivotPoint.position = castAnimationPositions[_index].position;
        public void ReelingLineAnchorPosition(int _index) => linePivotPoint.position = reelingAnimationPositions[_index].position;

        public HookBehaviour GetHook() => hook;
        public Transform GetLinePivotPoint() => linePivotPoint;
        public float GetLineLength() => scriptable.lineLength;
        public string GetDescription() => scriptable.description;
        public float GetReeledInDistance() => reeledInDistance;
        public bool IsInStartingCastPosition() => linePivotPoint.position == startCastAnimationPositions[3].position;
        public void ClearReelInputs()
        {
            InputManager.onCastReel -= StartReeling;
            InputManager.releaseCastReel -= StopReeling;
        }

        public void AddReelInputs()
        {
            InputManager.onCastReel += StartReeling;
            InputManager.releaseCastReel += StopReeling;
        }

        public void AddCastInput()
        {
            InputManager.onCastReel += StartCast;
        }

        private void OnDestroy()
        {
            hook.Despawn();
            InputManager.onCastReel -= StartCast;
        } 
    }

}