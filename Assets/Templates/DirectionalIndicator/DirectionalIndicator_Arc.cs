using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectionalIndicator_Arc : MonoBehaviour
{
    [SerializeField] private Slider rollBar;
    [SerializeField] private RectTransform pinContainer;
    [SerializeField] private float step;
    [SerializeField] private float smoothStep;

    private float smoothedAmount;
    private Vector3 resultRotation = Vector3.zero;

    private void Update()
    {
        if(rollBar)
        {
            RotateArrow(rollBar.value);
        }
    }

    private void RotateArrow(float amount)
    {
        if(smoothStep != 0)
        {
            smoothedAmount = Mathf.Lerp(smoothedAmount, amount, Time.fixedDeltaTime * smoothStep);
        }
        else
        {
            smoothedAmount = amount;
        }

        float result = smoothedAmount / rollBar.maxValue;
        resultRotation.z = result * step;

        pinContainer.localEulerAngles = resultRotation;
    }
}
