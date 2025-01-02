using Fishing.PlayerInput;
using UnityEngine;

namespace Fishing.UI {
    public class BucketMenuButton : MonoBehaviour {
        private void OnEnable() {
            InputManager.OnBucketMenu += BucketMenu.Instance.ToggleBucketMenu;
        }

        private void OnDisable() {
            InputManager.OnBucketMenu -= BucketMenu.Instance.ToggleBucketMenu;
        }
    }

}