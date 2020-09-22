using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class SwipeAnimation : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector2 originPosition, targetPosition;
    private float animationDuration = 1.0f;
    private float xOffset = 400.0f;
    private bool toAnimate = true;
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        InitVectors();
    }

    private void OnEnable()
    {
        StartAnimate();
    }

    private void InitVectors()
    {
        originPosition = rectTransform.localPosition;
        targetPosition.x = originPosition.x + xOffset;
        targetPosition.y = originPosition.y;
    }

    private void StartAnimate()
    {
        StartCoroutine(AnimateCoroutine());
    }

    private IEnumerator AnimateCoroutine()
    {
        Debug.Log("AnimateCoroutine!");
        while (toAnimate)
        {
            transform.DOLocalMoveX(targetPosition.x, animationDuration);
            yield return new WaitForSeconds(animationDuration);
            rectTransform.localPosition = originPosition;
        }
    }
}
