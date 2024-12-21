using Fishing.PlayerInput;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Fishing.UI {
    public class BucketMenuButton : MonoBehaviour {
        private void Start() {
            InputManager.OnBucketMenu += BucketMenu.Instance.ToggleBucketMenu;
        }
    }

}