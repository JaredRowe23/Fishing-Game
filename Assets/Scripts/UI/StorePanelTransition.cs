using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.NPC
{
    public class StorePanelTransition : MonoBehaviour
    {
        private enum Direction { Up, Down };
        [SerializeField] private Direction transitionDirection = Direction.Down;

        [SerializeField] private float upOffset = 1080f;
        [SerializeField] private float downOffset = 0f;
        [SerializeField] private float transitionSeconds = 1f;
        [SerializeField] private float transitionThreshold = 5f;

        private bool transitioning;
        private RectTransform panelRect;


        private void Awake()
        {
            panelRect = GetComponent<RectTransform>();
        }

        public void PanelUp()
        {
            if (transitioning) return;
            transitioning = true;
            transitionDirection = Direction.Up;
            StartCoroutine(Co_Transition(upOffset));
        }

        public void PanelDown()
        {
            if (transitioning) return;
            transitioning = true;
            transitionDirection = Direction.Down;
            StartCoroutine(Co_Transition(downOffset));
        }

        private IEnumerator Co_Transition(float _targetPos)
        {
            int _transitionDir = 1;
            if (transitionDirection == Direction.Down) _transitionDir = -1;

            float _moveDistance = Mathf.Abs(panelRect.offsetMin.y - _targetPos);

            while (true)
            {
                MoveRectOffset(_transitionDir * _moveDistance / transitionSeconds * Time.deltaTime);
                if (transitionDirection == Direction.Down && panelRect.offsetMin.y <= _targetPos + transitionThreshold)
                {
                    EndTransition(_targetPos);
                    break;
                }
                else if (transitionDirection == Direction.Up && panelRect.offsetMin.y >= _targetPos - transitionThreshold)
                {
                    EndTransition(_targetPos);
                    break;
                }

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

        private void EndTransition(float _endPosition)
        {
            SetRectOffset(_endPosition);
            transitioning = false;
        }
    }
}
