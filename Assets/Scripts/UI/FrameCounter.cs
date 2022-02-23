using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameCounter : MonoBehaviour
{
    private Text text;

    private void Awake() => text = GetComponent<Text>();

    private void Update()
    {
        text.text = "FPS: " + (Mathf.Round(1 / Time.deltaTime * 100) / 100).ToString();
    }
}
