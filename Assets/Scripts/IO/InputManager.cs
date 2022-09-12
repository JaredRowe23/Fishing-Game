using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.IO
{
    public class InputManager : MonoBehaviour
    {
        public KeyCode castKey;
        public KeyCode backpackKey;
        public KeyCode bucketKey;
        public KeyCode pauseKey;
        public KeyCode quickSaveKey;
        public KeyCode quickLoadKey;
        public KeyCode reelKey;

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
        }
    }
}
