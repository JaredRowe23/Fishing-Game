using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI {
    public class FrameCounter : MonoBehaviour {
        [SerializeField, Min(0), Tooltip("Amount of seconds to wait before updating the frame counter. Higher values trade recency for readability.")] private float _secondsPerUpdate = 15;
        private Text _frameText;

        private void Awake() {
            _frameText = GetComponent<Text>();
        }

        private void Start() {
            StartCoroutine(Co_UpdateFrameCounter());
        }

        private IEnumerator Co_UpdateFrameCounter() {
            while (true) {
                double seconds = Time.timeAsDouble;
                int frames = Time.frameCount;
                yield return new WaitForSecondsRealtime(_secondsPerUpdate);
                double updateTime = Time.timeAsDouble - seconds;
                int updateFrames = Time.frameCount - frames;
                _frameText.text = $"FPS: {(updateFrames / updateTime).ToString("F2")}";
            }
        }
    }
}