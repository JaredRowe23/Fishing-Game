using System.Collections;
using UnityEngine;

namespace Fishing.NPC {
    public class StorePanelTransition : MonoBehaviour {
        [SerializeField, Tooltip("Target Y position in pixels for this panel when moving up.")] private float _upOffset = 1080f;
        [SerializeField, Tooltip("Target Y position in pixels for this panel when moving down.")] private float _downOffset = 0f;
        [SerializeField, Min(0), Tooltip("Amount of time in seconds for this to make it's transition moving up or down.")] private float _transitionSeconds = 1f;
        [SerializeField, Min(0), Tooltip("Distance in pixels from this panel's current position to the target position that is considered \"close enough\" for this transition to complete.")] private float _transitionThreshold = 5f;

        private enum Direction { Up, Down };
        private Direction _transitionDirection = Direction.Down;

        private bool _transitioning = false;
        private RectTransform _panelRect;

        private void Awake() {
            _panelRect = GetComponent<RectTransform>();
        }

        public void PanelUp() {
            if (_transitioning) {
                return;
            }

            _transitioning = true;
            _transitionDirection = Direction.Up;
            StartCoroutine(Co_Transition(_upOffset));
        }

        public void PanelDown() {
            if (_transitioning) {
                return;
            }

            _transitioning = true;
            _transitionDirection = Direction.Down;
            StartCoroutine(Co_Transition(_downOffset));
        }

        private IEnumerator Co_Transition(float targetPos) {
            int transitionDir = _transitionDirection == Direction.Down ? -1 : 1;
            float moveDistance = Mathf.Abs(_panelRect.offsetMin.y - targetPos);
            float moveStep = moveDistance / _transitionSeconds * transitionDir;

            while (true) {
                MoveRectOffset(moveStep * Time.deltaTime);
                if (_transitionDirection == Direction.Down && _panelRect.offsetMin.y <= targetPos + _transitionThreshold) {
                    EndTransition(targetPos);
                    break;
                }
                else if (_transitionDirection == Direction.Up && _panelRect.offsetMin.y >= targetPos - _transitionThreshold) {
                    EndTransition(targetPos);
                    break;
                }

                yield return null;
            }
        }

        private void MoveRectOffset(float offset) {
            _panelRect.offsetMin = new Vector2(_panelRect.offsetMin.x, _panelRect.offsetMin.y + offset);
            _panelRect.offsetMax = new Vector2(_panelRect.offsetMax.x, _panelRect.offsetMin.y + offset);
        }

        private void EndTransition(float endPosition) {
            SetRectOffset(endPosition);
            _transitioning = false;
        }

        private void SetRectOffset(float offset) {
            _panelRect.offsetMin = new Vector2(_panelRect.offsetMin.x, offset);
            _panelRect.offsetMax = new Vector2(_panelRect.offsetMax.x, offset);
        }
    }
}
