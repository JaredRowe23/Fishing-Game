using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.Fishables;
using Fishing.IO;
using Fishing.UI;
using Fishing.PlayerCamera;
using Fishing.FishingMechanics.Minigame;
using Fishing.Inventory;

namespace Fishing.FishingMechanics
{
    //[RequireComponent(typeof(RodScriptable))]
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
        private BucketBehaviour bucket;

        private void Awake()
        {
            rodManager = RodManager.instance;
            anim = GetComponent<Animator>();
            cam = CameraBehaviour.Instance;
            bucket = BucketBehaviour.instance;

            InputManager.onCastReel += StartCast;
        }

        void Start()
        {
            casted = false;
            playerAnim = rodManager.GetComponent<Animator>();

            if (PlayerData.instance.hasSeenTutorialData.castTutorial) return;
            ShowCastingTutorial();
        }

        void Update()
        {
            if (anim.GetBool("isReeling"))
            {
                AudioManager.instance.PlaySound("Reel", true);
                hook.Reel(scriptable.reelSpeed);

                Vector2 _waterSurfaceUnderRodPosition = new Vector2(hook.GetHookAnchorPoint().position.x, 0f);
                if (Vector2.Distance(hook.transform.position, _waterSurfaceUnderRodPosition) <= reeledInDistance) OnReeledIn();
            }
        }

        private void ShowCastingTutorial()
        {
            TutorialSystem.instance.QueueTutorial("Hold the left mouse button to begin casting.");
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
            if (UIManager.instance.mouseOverUI || UIManager.instance.IsActiveUI() || TutorialSystem.instance.TutorialListings.content.gameObject.activeSelf) return;
            if (casted) return;

            anim.SetTrigger("startCast");
            playerAnim.SetTrigger("startCast");

            UIManager.instance.HideHUDButtons();

            InputManager.onCastReel -= StartCast;

            PowerAndAngle.instance.StartAngling();
        }

        public void Cast(float _angle, float _strength)
        {
            hook.Cast(_angle, _strength);

            anim.SetTrigger("cast");
            playerAnim.SetTrigger("cast");

            cam.EnablePlayerControls();
            InputManager.onCastReel += StartReeling;
            InputManager.releaseCastReel += StopReeling;

            casted = true;
        }

        public void OnReeledIn()
        {
            AddCatch();

            ReelingMinigame.instance.EndMinigame();

            AudioManager.instance.StopPlaying("Reel");

            anim.SetBool("isReeling", false);
            playerAnim.SetBool("isReeling", false);

            cam.ReturnHome();

            UIManager.instance.ShowHUDButtons();

            casted = false;
            isResettingHook = true;
        }

        private void AddCatch()
        {
            if (hook.hookedObject == null) return;
            if (hook.hookedObject.GetComponent<BaitBehaviour>()) return;

            bucket.AddToBucket(rodManager.equippedRod.GetHook().hookedObject.GetComponent<Fishable>());
        }

        public void ReEquipBait()
        {
            if (PlayerData.instance.equippedRod.equippedBait == null) return;
            if (PlayerData.instance.equippedRod.equippedBait.baitName == null) return;

            if (PlayerData.instance.equippedRod.equippedBait.amount <= 0)
            {
                TooltipSystem.instance.NewTooltip(3, "Out of bait: " + PlayerData.instance.equippedRod.equippedBait.baitName);

                for (int i = 0; i < PlayerData.instance.baitSaveData.Count; i++)
                {
                    if (PlayerData.instance.baitSaveData[i].baitName != PlayerData.instance.equippedRod.equippedBait.baitName) continue;

                    PlayerData.instance.baitSaveData.RemoveAt(i);
                    break;
                }
                PlayerData.instance.equippedRod.equippedBait = null;
            }
            else
            {
                PlayerData.instance.equippedRod.equippedBait.amount--;
                rodManager.SpawnBait();
            }
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