﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public struct SlowMoData
{
    public float delay;
    public float duration;
    //public float animatorsSpeed;
}

public class SlowMoController : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Animator enemyAnimator;
    
    [SerializeField] private float playerAnimatorSpeedOnSlowMo;
    [SerializeField] private float enemyAnimatorSpeedOnSlowMo;
    
    private void OnEnable()
    {
        Observer.Instance.OnEnableSlowMo += EnableSlowMo;
    }

    private void EnableSlowMo(SlowMoData data)
    {
        StartCoroutine(HandleSlowMo());
        IEnumerator HandleSlowMo()
        {
            yield return new WaitForSeconds(data.delay);
            playerAnimator.speed = playerAnimatorSpeedOnSlowMo;
            enemyAnimator.speed = enemyAnimatorSpeedOnSlowMo;
            yield return new WaitForSeconds(data.duration);
            DisableSlowMo();
        }
    }

    private void DisableSlowMo()
    {
        playerAnimator.speed = 1;
        enemyAnimator.speed = 1;
        Observer.Instance.OnSlowMoDisabled();
    }
}
