using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISliceEffect : MonoBehaviour
{
    [SerializeField] private Animation sliceAnimation;
    [SerializeField] private RectTransform sliceEffect;
    [SerializeField] private int UpRotation;
    [SerializeField] private int DownRotation;
    [SerializeField] private int LeftRotation;
    [SerializeField] private int RightRotation;

    private void OnEnable()
    {
        SwipeDetector.OnSwipe += ActivateSliceEffect;
    }

    private void OnDisable()
    {
        SwipeDetector.OnSwipe -= ActivateSliceEffect;
    }

    private void ActivateSliceEffect(SwipeData obj)
    {
        Vector3 currentRotation = sliceEffect.transform.eulerAngles;
        
        switch (obj.Direction)
        {
            case SwipeDirection.Up:
                currentRotation.z = UpRotation;
                break;
            case SwipeDirection.Down:
                currentRotation.z = DownRotation;
                break;
            case SwipeDirection.Left:
                currentRotation.z = LeftRotation;
                break;
            case SwipeDirection.Right:
                currentRotation.z = RightRotation;
                break;
        }

        sliceEffect.transform.eulerAngles = currentRotation;
        sliceAnimation.Play();
    }
}
