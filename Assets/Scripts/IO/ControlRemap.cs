using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Fishing.UI;

namespace Fishing.IO
{
    public class ControlRemap : MonoBehaviour
    {
        [SerializeField] private string actionName;

        private Controls _controls;

        private void Awake()
        {
            _controls = TitleMenuManager.instance._controls;
        }

        public void StartRemap()
        {
            _controls = TitleMenuManager.instance._controls;
            _controls.FindAction(actionName).Disable();
            _controls.FindAction(actionName).PerformInteractiveRebinding()
                .OnComplete(callback =>
                {
                    Debug.Log(callback);
                    callback.Dispose();
                    _controls.FindAction(actionName).Enable();
                })
                .Start();
        }
    }
}
