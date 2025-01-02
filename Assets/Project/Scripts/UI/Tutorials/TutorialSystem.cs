using Fishing.Util;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI {
    public class TutorialSystem : MonoBehaviour {
        [SerializeField, Tooltip("Prefab of the tutorial UI to spawn.")] private GameObject _tutorialPrefab;
        [SerializeField, Tooltip("ScrollRect UI that holds the tutorial objects.")] private ScrollRect _tutorialListings;
        public ScrollRect TutorialListings { get { return _tutorialListings; } private set { } }


        [SerializeField, Tooltip("Image UI that covers the entire screen and acts as a filter to focus player attention to the tutorial.")] private Image _filter;

        [SerializeField, Range(0f, 1f), Tooltip("Alpha value to set the filter image to when a tutorial is active.")] private float _fadedAlpha = 0.5f;
        [SerializeField, Range(0f, 1f), Tooltip("Value from 0 to 1 of how much to slow the scene's play speed when a tutorial is active. 1 is normal speed, 0 is stopped completely.")] private float _slowMotionSpeed = 0.1f;
        [SerializeField, Min(0), Tooltip("Amount of seconds it takes to transition to and from the tutorial's active state.")] private float _transitionTime = 2f;

        private Queue<Tutorial> _tutorialQueue;
        public Queue<Tutorial> TutorialQueue { get => _tutorialQueue; private set => _tutorialQueue = value; }

        private bool _isShowingTutorials;

        private GraphicRaycaster _graphicsRaycaster; // TODO: Find out why this was needed.

        private static TutorialSystem _instance;
        public static TutorialSystem Instance { get => _instance; private set => _instance = value; }

        private void Awake() {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            _graphicsRaycaster = GetComponent<GraphicRaycaster>();
            TutorialQueue = new Queue<Tutorial>();
        }

        private IEnumerator Co_ShowTutorials() {
            _isShowingTutorials = true;
            _graphicsRaycaster.enabled = true;
            _tutorialListings.content.gameObject.SetActive(true);
            float transitionProgress = 0f; // Increases towards 1 as transition completes

            while (true) {
                if (PauseMenu.Instance.PauseUI.activeSelf) {
                    yield return null;
                }

                transitionProgress += _transitionTime * Time.unscaledDeltaTime;
                _filter.color = Utilities.SetTransparency(_filter.color, Mathf.Lerp(0f, _fadedAlpha, transitionProgress));
                Time.timeScale = Mathf.Lerp(1f, _slowMotionSpeed, transitionProgress);

                if (transitionProgress >= 1f) {
                    break;
                }
                else {
                    yield return null;
                }
            }
        }

        private IEnumerator Co_HideTutorials() {
            _isShowingTutorials = false;
            _graphicsRaycaster.enabled = false;
            _tutorialListings.content.gameObject.SetActive(false);
            float transitionProgress = 0f; // Increases towards 1 as transition completes

            while (true) {
                if (PauseMenu.Instance.PauseUI.activeSelf) {
                    yield return null;
                }

                transitionProgress += _transitionTime * Time.unscaledDeltaTime;
                _filter.color = Utilities.SetTransparency(_filter.color, Mathf.Lerp(_fadedAlpha, 0f, transitionProgress));
                Time.timeScale = Mathf.Lerp(_slowMotionSpeed, 1f, transitionProgress);

                if (transitionProgress >= 1f) {
                    break;
                }
                else {
                    yield return null;
                }
            }
        }

        public void ShowNextTutorial() {
            if (_tutorialListings.content.transform.childCount != 0) {
                return;
            }
            if (TutorialQueue.Count == 0) {
                StopCoroutine("Co_ShowTutorials");
                StartCoroutine("Co_HideTutorials");
                return;
            }

            TutorialQueue.Peek().transform.SetParent(_tutorialListings.content.transform);
            TutorialQueue.Peek().gameObject.SetActive(true);
            if (TutorialQueue.Peek().Lifetime != 0) {
                StartCoroutine(TutorialQueue.Peek().Co_DestroyTutorial());
            }

            if (!_isShowingTutorials) {
                StartCoroutine("Co_ShowTutorials");
            }
        }

        public void QueueTutorial(string tutorialText) {
            Tutorial newTutorial = Instantiate(_tutorialPrefab).GetComponent<Tutorial>();
            newTutorial.InitializeTutorial(tutorialText);
            newTutorial.gameObject.SetActive(false);
            TutorialQueue.Enqueue(newTutorial);
            ShowNextTutorial();
        }

        public void QueueTutorial(string tutorialText, float lifetime) {
            Tutorial newTutorial = Instantiate(_tutorialPrefab).GetComponent<Tutorial>();
            newTutorial.InitializeTutorial(tutorialText, lifetime);
            newTutorial.gameObject.SetActive(false);
            TutorialQueue.Enqueue(newTutorial);
            ShowNextTutorial();
        }
    }
}