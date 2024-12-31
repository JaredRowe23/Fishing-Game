using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI {
    public class TooltipSystem : MonoBehaviour {
        [SerializeField, Tooltip("Prefab of the tooltip UI that will appear.")] private GameObject _tooltipPrefab;
        [SerializeField, Tooltip("ScrolLRect UI that holds active tooltips.")] private ScrollRect _tooltipListings;

        private const float DEFAULT_LIFETIME = 3f;

        private static TooltipSystem _instance;
        public static TooltipSystem Instance { get => _instance; set => _instance = value; }

        private void Awake() {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void NewTooltip(string tooltipText, float lifetime = DEFAULT_LIFETIME) {
            ToolTip newToolTip = Instantiate(_tooltipPrefab, _tooltipListings.content.transform).GetComponent<ToolTip>();

            newToolTip.InitializeToolTip(lifetime, tooltipText);
        }
    }
}
