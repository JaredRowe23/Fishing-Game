using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Fishing.IO
{
    public class InputManager : MonoBehaviour
    {
        public InputControl titleInputs;
        public InputControl fishingLevelInputs;
        public InputControl storeMenuInputs;
        public InputControl pauseMenuInputs;

        public static InputManager instance;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            Controls _controls = new Controls();
            _controls.TitleMenuInput.Enable();
        }
    }
}
