using Fishing.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI
{
    public class TutorialSystem : MonoBehaviour
    {
        [SerializeField] private GameObject tutorialPrefab;
        [SerializeField] private Image filter;
        [SerializeField] private float fadedAlpha;
        [Range(0f, 1f)]
        [SerializeField] private float slowMotionSpeed = 0.1f;
        [SerializeField] private float transitionSpeed;

        [SerializeField] private ScrollRect tutorialListings;
        public ScrollRect TutorialListings { get { return tutorialListings; } private set { } }

        public Queue<GameObject> tutorialQueue;

        private GraphicRaycaster gr;

        public static TutorialSystem instance;

        private void Awake() {
            if (instance != null) {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            gr = GetComponent<GraphicRaycaster>();
            tutorialQueue = new Queue<GameObject>();
        }

        private void Update() {
            if (tutorialListings.content.transform.childCount == 0) {
                HideTutorials();
            }
            else {
                ShowTutorials();
            }
        }

        private void ShowTutorials() {
            gr.enabled = true;
            tutorialListings.content.gameObject.SetActive(true);
            filter.color = Utilities.SetTransparency(filter.color, Mathf.Lerp(filter.color.a, fadedAlpha, transitionSpeed * Time.deltaTime));
            filter.color = new Color(filter.color.r, filter.color.g, filter.color.b, Mathf.Lerp(filter.color.a, fadedAlpha, transitionSpeed * Time.deltaTime));
            Time.timeScale = Mathf.Lerp(Time.timeScale, slowMotionSpeed, transitionSpeed * Time.deltaTime);
        }

        private void HideTutorials() {
            gr.enabled = false;
            tutorialListings.content.gameObject.SetActive(false);
            filter.color = Utilities.SetTransparency(filter.color, Mathf.Lerp(filter.color.a, 0f, transitionSpeed * Time.deltaTime));
            if (PauseMenu.instance == null || !PauseMenu.instance.pauseMenu.activeSelf) {
                Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, transitionSpeed * Time.deltaTime);
            }

            if (tutorialQueue.Count == 0) {
                return;
            }

            ShowNextTutorial();
        }

        private void ShowNextTutorial() {
            tutorialQueue.Peek().transform.SetParent(tutorialListings.content.transform);
            tutorialQueue.Peek().SetActive(true);
        }

        public void QueueTutorial(string _tutorialText, bool _hasLifetime = false, float _lifetime = 0f) {
            GameObject _newTutorial = Instantiate(tutorialPrefab);
            _newTutorial.GetComponent<Tutorial>().InitializeTutorial(_tutorialText, _hasLifetime, _lifetime);
            _newTutorial.SetActive(false);
            tutorialQueue.Enqueue(_newTutorial);
        }
    }
}