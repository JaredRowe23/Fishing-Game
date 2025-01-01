using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI {
    public class Tutorial : MonoBehaviour {
        [SerializeField, Tooltip("Text UI that displays the tutorial's message.")] private Text _tutorialText;
        [SerializeField, Tooltip("Button UI that closes the tutorial.")] private Button _closeButton;
        private float _lifetime;
        public float Lifetime { get => _lifetime; private set { _lifetime = value; } }

        public void InitializeTutorial(string tutorialText) {
            _tutorialText.text = tutorialText;
            Lifetime = 0f;
            _closeButton.gameObject.SetActive(true);
            _closeButton.onClick.AddListener(delegate { DestroyTutorial(); });
        }

        public void InitializeTutorial(string tutorialText, float lifetime) {
            _tutorialText.text = tutorialText;
            Lifetime = lifetime;
            _closeButton.gameObject.SetActive(false);
        }

        public void DestroyTutorial() {
            TutorialSystem.Instance.TutorialQueue.Dequeue();
            transform.SetParent(null);
            TutorialSystem.Instance.ShowNextTutorial();
            Destroy(gameObject);
        }

        public IEnumerator Co_DestroyTutorial() {
            while (true) {
                yield return new WaitForSecondsRealtime(_lifetime);
                DestroyTutorial();
                break;
            }
        }
    }
}
