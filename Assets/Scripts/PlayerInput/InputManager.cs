using System;
using UnityEngine;

namespace Fishing.PlayerInput {
    public class InputManager : MonoBehaviour {
        private static InputManager _instance;
        public static InputManager Instance { get => _instance; private set => _instance = value; }

        public static event Action OnPauseMenu;
        public static event Action OnInventoryMenu;
        public static event Action OnBucketMenu;

        public static event Action OnCastReel;
        public static event Action ReleaseCastReel;

        public static event Action OnZoomIn;
        public static event Action OnZoomOut;

        public static event Action OnMoveUp;
        public static event Action OnMoveDown;
        public static event Action OnMoveLeft;
        public static event Action OnMoveRight;
        public static event Action ReleaseMoveUp;
        public static event Action ReleaseMoveDown;
        public static event Action ReleaseMoveLeft;
        public static event Action ReleaseMoveRight;

        private void Awake() {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update() {
            if (Input.GetMouseButtonDown(1)) {
                OnCastReel?.Invoke();
            }
            if (Input.GetMouseButtonUp(1)) {
                ReleaseCastReel?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.Escape)) {
                OnPauseMenu?.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.I)) {
                OnInventoryMenu?.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.B)) {
                OnBucketMenu?.Invoke();
            }

            if (Input.mouseScrollDelta.y > 0) {
                OnZoomIn?.Invoke();
            }
            if (Input.mouseScrollDelta.y < 0) {
                OnZoomOut?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.W)) {
                OnMoveUp?.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.S)) {
                OnMoveDown?.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.A)) {
                OnMoveLeft?.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.D)) {
                OnMoveRight?.Invoke();
            }
            if (Input.GetKeyUp(KeyCode.W)) {
                ReleaseMoveUp?.Invoke();
            }
            if (Input.GetKeyUp(KeyCode.S)) {
                ReleaseMoveDown?.Invoke();
            }
            if (Input.GetKeyUp(KeyCode.A)) {
                ReleaseMoveLeft?.Invoke();
            }
            if (Input.GetKeyUp(KeyCode.D)) {
                ReleaseMoveRight?.Invoke();
            }
        }

        public static void ClearListeners() {
            OnCastReel = null;
            ReleaseCastReel = null;

            OnInventoryMenu = null;
            OnBucketMenu = null;

            OnZoomIn = null;
            OnZoomOut = null;

            OnMoveUp = null;
            OnMoveDown = null;
            OnMoveLeft = null;
            OnMoveRight = null;
            ReleaseMoveUp = null;
            ReleaseMoveDown = null;
            ReleaseMoveLeft = null;
            ReleaseMoveRight = null;
        }
    }
}
