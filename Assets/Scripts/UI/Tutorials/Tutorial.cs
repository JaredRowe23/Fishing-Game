using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI
{
    public class Tutorial : MonoBehaviour
    {
        [SerializeField] private Text tutorialText;
        private bool hasLifetime;
        private float lifetime;

        private void Update() {
            if (!hasLifetime) return;
            lifetime -= Time.deltaTime * (1 / Time.timeScale);
            if (lifetime > 0f) return;
            DestroyTutorial();
        }

        public void InitializeTutorial(string _tutorialText, bool _hasLifetime, float _lifetime) {
            tutorialText.text = _tutorialText;
            hasLifetime = _hasLifetime;
            lifetime = _lifetime;
        }

        public void DestroyTutorial() {
            TutorialSystem.instance.tutorialQueue.Dequeue();
            Destroy(gameObject);
        }
    }
}
