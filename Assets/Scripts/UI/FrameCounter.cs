using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing.UI
{
    public class FrameCounter : MonoBehaviour
    {
        [SerializeField] private int framesPerUpdate = 15;
        private int frameCount;
        private float frameUpdateDeltaTime;
        private Text text;

        private void Awake() => text = GetComponent<Text>();
        private void Start() {
            frameUpdateDeltaTime = 0;
            frameCount = framesPerUpdate;
        }

        private void Update() {
            frameCount--;
            frameUpdateDeltaTime += Time.deltaTime;
            if (frameCount <= 0)
            {
                text.text = $"FPS: {(Mathf.Round(1 / (frameUpdateDeltaTime / framesPerUpdate) * 100) / 100).ToString("0.00")}";
                frameCount = framesPerUpdate;
                frameUpdateDeltaTime = 0;
            }
        }
    }

}