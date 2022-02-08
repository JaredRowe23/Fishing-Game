// This script controls the Angle Arrow we use to
// change the angle we cast at

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AngleArrow : MonoBehaviour
{
    private bool isAngling;
    private float targetAngle;
    private float maxAngle;
    private float angleFrequency;
    [SerializeField] private float threshold;
    private RectTransform rect;
    private RodBehaviour castingRod;

    private float currentAngle;

    private void Start()
    {
        rect = this.GetComponent<RectTransform>();
        transform.SetParent(GameController.instance.transform);
    }
    
    void Update()
    {
        if (isAngling)
        {
            currentAngle += (((targetAngle * 2 / maxAngle) - 1) * Time.deltaTime / angleFrequency) * maxAngle;

            if (currentAngle >= maxAngle - threshold && (targetAngle * 2 / maxAngle) - 1 > 0f)
            {
                currentAngle = maxAngle;
                targetAngle = 0f;
            }
            else if (currentAngle <= 0f + threshold && (targetAngle * 2 / maxAngle) - 1 < 0f)
            {
                currentAngle = 0f;
                targetAngle = maxAngle;
            }

            rect.rotation = Quaternion.Euler(0f, 0f, currentAngle);
            AudioManager.instance.GetSource("Power Audio").pitch = Mathf.InverseLerp(0f, maxAngle, currentAngle) + AudioManager.instance.GetSound("Power Audio").pitch;
            print("Angle: " + currentAngle.ToString());
            print("Lerped Angle: " + Mathf.InverseLerp(0f, maxAngle, currentAngle).ToString());

            if (Input.GetMouseButtonDown(0))
            {
                AudioManager.instance.StopPlaying("Power Audio");
                castingRod.castAngle = currentAngle;
                isAngling = false;
                GameController.instance.powerSlider.transform.SetParent(GameController.instance.transform);
                transform.SetParent(GameController.instance.transform);
                castingRod.BeginCast();
            }
        }
    }

    public void StartAngling(float angle, float frequency, RodBehaviour rod)
    {
        currentAngle = 0f;
        castingRod = rod;
        maxAngle = angle;
        targetAngle = maxAngle;
        angleFrequency = frequency;
        rect.rotation = Quaternion.identity;
        isAngling = true;
    }
}
