using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.NPC
{
    public class StorePanelTransition : MonoBehaviour
    {
        [SerializeField] private float verticalOffset;
        [SerializeField] private float transitionSeconds;
        [SerializeField] private float transitionThreshold;
        [SerializeField] private bool downwardsTransition;
        private bool transitioning;
        private RectTransform panelRect;

        private void Awake()
        {
            panelRect = GetComponent<RectTransform>();
        }

        public void HidePanel()
        {
            if (transitioning) return;
            transitioning = true;
            StartCoroutine(Co_Transition(verticalOffset));
        }

        public void ShowPanel()
        {
            if (transitioning) return;
            transitioning = true;
            StartCoroutine(Co_Transition(0f));
        }

        private IEnumerator Co_Transition(float _targetPos)
        {
            int _transitionDir = 1;
            if (_targetPos < panelRect.offsetMin.y) _transitionDir = -1;
            if (downwardsTransition) _transitionDir *= -1;

            while (true)
            {
                MoveRectOffset(_transitionDir * verticalOffset / transitionSeconds * Time.deltaTime);
                if (Mathf.Abs(panelRect.offsetMin.y - _targetPos) <= transitionThreshold)
                {
                    SetRectOffset(_targetPos);
                    transitioning = false;
                    break;
                }
                //Work on fixing overshooting issue
                //else if (!downwardsTransition && panelRect.offsetMin.y < _targetPos * _transitionDir)
                //{
                //    Debug.Log("test too far: " + gameObject.name);
                //}
                //else if (downwardsTransition && panelRect.offsetMin.y > _targetPos * _transitionDir)
                //{
                //    Debug.Log("test too far: " + gameObject.name);
                //}

                yield return null;
            }
        }
        private void SetRectOffset(float _offset)
        {
            panelRect.offsetMin = new Vector2(panelRect.offsetMin.x, _offset);
            panelRect.offsetMax = new Vector2(panelRect.offsetMax.x, _offset);
        }
        private void MoveRectOffset(float _offset)
        {
            panelRect.offsetMin = new Vector2(panelRect.offsetMin.x, panelRect.offsetMin.y + _offset);
            panelRect.offsetMax = new Vector2(panelRect.offsetMax.x, panelRect.offsetMin.y + _offset);
        }
    }
}
