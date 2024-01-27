using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.IO
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager instance;

        public delegate void OnPauseMenu();
        public static event OnPauseMenu onPauseMenu;
        public delegate void OnInventoryMenu();
        public static event OnInventoryMenu onInventoryMenu;
        public delegate void OnBucketMenu();
        public static event OnBucketMenu onBucketMenu;

        public delegate void OnCastReel();
        public static event OnCastReel onCastReel;
        public delegate void ReleaseCastReel();
        public static event ReleaseCastReel releaseCastReel;

        public delegate void OnZoomIn();
        public static event OnZoomIn onZoomIn;
        public delegate void OnZoomOut();
        public static event OnZoomOut onZoomOut;

        public delegate void OnMoveUp();
        public static event OnMoveUp onMoveUp;
        public delegate void OnMoveDown();
        public static event OnMoveDown onMoveDown;
        public delegate void OnMoveLeft();
        public static event OnMoveLeft onMoveLeft;
        public delegate void OnMoveRight();
        public static event OnMoveRight onMoveRight;
        public delegate void ReleaseMoveUp();
        public static event ReleaseMoveUp releaseMoveUp;
        public delegate void ReleaseMoveDown();
        public static event ReleaseMoveDown releaseMoveDown;
        public delegate void ReleaseMoveLeft();
        public static event ReleaseMoveLeft releaseMoveLeft;
        public delegate void ReleaseMoveRight();
        public static event ReleaseMoveRight releaseMoveRight;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1)) onCastReel?.Invoke();
            if (Input.GetMouseButtonUp(1)) releaseCastReel?.Invoke();

            if (Input.GetKeyDown(KeyCode.Escape)) onPauseMenu?.Invoke();
            if (Input.GetKeyDown(KeyCode.I)) onInventoryMenu?.Invoke();
            if (Input.GetKeyDown(KeyCode.B)) onBucketMenu?.Invoke();

            if (Input.mouseScrollDelta.y > 0) onZoomIn?.Invoke();
            if (Input.mouseScrollDelta.y < 0) onZoomOut?.Invoke();

            if (Input.GetKeyDown(KeyCode.W)) onMoveUp?.Invoke();
            if (Input.GetKeyDown(KeyCode.S)) onMoveDown?.Invoke();
            if (Input.GetKeyDown(KeyCode.A)) onMoveLeft?.Invoke();
            if (Input.GetKeyDown(KeyCode.D)) onMoveRight?.Invoke();
            if (Input.GetKeyUp(KeyCode.W)) releaseMoveUp?.Invoke();
            if (Input.GetKeyUp(KeyCode.S)) releaseMoveDown?.Invoke();
            if (Input.GetKeyUp(KeyCode.A)) releaseMoveLeft?.Invoke();
            if (Input.GetKeyUp(KeyCode.D)) releaseMoveRight?.Invoke();
        }

        public static void ClearListeners()
        {
            onCastReel = null;
            releaseCastReel = null;

            onInventoryMenu = null;
            onBucketMenu = null;

            onZoomIn = null;
            onZoomOut = null;

            onMoveUp = null;
            onMoveDown = null;
            onMoveLeft = null;
            onMoveRight = null;
            releaseMoveUp = null;
            releaseMoveDown = null;
            releaseMoveLeft = null;
            releaseMoveRight = null;
        }
    }
}
