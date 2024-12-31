using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Fishing.NPC {
    public class LeaveStoreButton : MonoBehaviour {
        private void Awake() {
            GetComponent<Button>().onClick.AddListener(delegate { OnLeaveStore(); });
        }

        public void OnLeaveStore() {
            SceneManager.LoadScene(1);
        }
    }
}