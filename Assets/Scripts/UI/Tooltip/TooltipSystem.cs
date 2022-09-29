using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI
{
    public class TooltipSystem : MonoBehaviour
    {
        [SerializeField] private GameObject tooltipPrefab;

        public GameObject content;

        public static TooltipSystem instance;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void NewTooltip(float _lifetime, string _tooltipText)
        {
            ToolTip _newToolTip = Instantiate(tooltipPrefab, content.transform).GetComponent<ToolTip>();

            _newToolTip.InitializeToolTip(_lifetime, _tooltipText);
        }
    }
}
