using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI
{
    public class ToolTip : MonoBehaviour
    {
        public float lifetime;

        [SerializeField] private Text tipText;

        private float lifetimeCount;

        private void Update()
        {
            lifetimeCount -= Time.deltaTime;
            if (lifetimeCount <= 0f)
            {
                Destroy(gameObject);
            }
        }

        public void InitializeToolTip(float _lifetime, string _tipText)
        {
            lifetime = _lifetime;
            lifetimeCount = lifetime;
            tipText.text = _tipText;
        }
    }
}
