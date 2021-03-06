﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.AccessControl;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour, IDamageable
{
    [SerializeField] private float finishDelay = 0.5f;
    [SerializeField] private SlowMoSettings slowMoSettings;
    [SerializeField] private Sword sword;
    [SerializeField] private Transform[] checkPoints;

    private Animator animator;
    private EnemyController currentEnemy;

    private Transform currentAimTarget;
    private int currentCheckPointIndex;
    private AttackType attackType;

    private float playerLookAtSpeed = 3f;

    private bool canAttack = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        Observer.Instance.OnStartGame += GoToNextCheckPoint;
        Observer.Instance.OnNextEnemyPushed += SetCurrentTarget;
        Observer.Instance.OnCheckPointPassed += GoToNextCheckPoint;
        Observer.Instance.OnCheckPointPassed += ClearCurrentEnemy;
        
        Observer.Instance.OnEnemyStartsAttack += delegate { canAttack = true; };
        Observer.Instance.OnEnemyDied += delegate { canAttack = false; };
        
        SwipeDetector.OnSwipe += TryAttack;
    }

    private void SetCurrentTarget(EnemyController enemy)
    {
        currentEnemy = enemy;
        SetAimTarget(enemy.transform);
    }

    private void SetAimTarget(Transform target)
    {
        currentAimTarget = target;
    }

    private void ClearCurrentEnemy()
    {
        currentEnemy = null;
    }

    private void TryAttack(SwipeData swipe)
    {
        if (!currentEnemy || !canAttack)
        {
            return;
        }

        if (attackType == AttackType.Simple)
        {
            NormalAttack(swipe);
        }
        else
        {
            SuperAttack();
        }

        currentEnemy.SlowDown();
        EnableSlowMo();
        sword.StartAttack();
    }

    private void NormalAttack(SwipeData swipe)
    {
        switch (swipe.Direction)
        {
            case SwipeDirection.Up:
                animator.SetTrigger(AnimatorHashes.AttackUp);
                break;
            case SwipeDirection.Down:
                animator.SetTrigger(AnimatorHashes.AttackDown);
                break;
            case SwipeDirection.Left:
                animator.SetTrigger(AnimatorHashes.AttackLeft);
                break;
            case SwipeDirection.Right:
                animator.SetTrigger(AnimatorHashes.AttackRight);
                break;
        }
    }

    private void SuperAttack()
    {
        ObjectPooler.Instance.SpawnObject("SuperAttack", transform.position).transform.parent = transform;
        animator.SetTrigger(AnimatorHashes.SuperAttack);
    }


    private void EnableSlowMo()
    {
        SlowMoData slowMoData = new SlowMoData();
        slowMoData.delay = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length * slowMoSettings.startTimePercentage;
        slowMoData.duration = slowMoSettings.duration;
        Observer.Instance.OnEnableSlowMo(slowMoData);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameConstants.TagFightCheckPoint))
        {
            Stop();

            SetAimTarget(other.GetComponent<IFightCheckPoint>().PlayerAimTarget);
            UpdateCheckPointIndex();
        }

        if (other.CompareTag(GameConstants.TagPathPoint) && checkPoints.Contains(other.transform))
        {
            UpdateCheckPointIndex();
            SetAimTarget(checkPoints[currentCheckPointIndex]);
        }
    }

    private void UpdateCheckPointIndex()
    {
        currentCheckPointIndex++;
    }

    private void Stop()
    {
        animator.SetTrigger(AnimatorHashes.Idle);
    }

    private void GoToNextCheckPoint()
    {
        if (currentCheckPointIndex >= checkPoints.Length)
        {
            StartCoroutine(DelayedFinish());
            IEnumerator DelayedFinish()
            {
                yield return new WaitForSeconds(finishDelay);
                Observer.Instance.CallOnWinLevel();
            }
            return;
        }

        SetAimTarget(checkPoints[currentCheckPointIndex]);
        animator.SetTrigger(AnimatorHashes.Run);
    }

    private void Update()
    {
        AimToCurrentTarget();
    }

    private void AimToCurrentTarget()
    {
        if (!currentAimTarget)
        {
            return;
        }

        Vector3 targetDirection = currentAimTarget.position - transform.position;
        Vector3 targetDirectionXZ = Vector3.ProjectOnPlane(targetDirection, transform.up);
        Quaternion lookRotationAboutYAxis = Quaternion.LookRotation(targetDirectionXZ);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotationAboutYAxis,
                                             Time.deltaTime * playerLookAtSpeed);
    }

    public void SetAttackType(AttackType attackType)
    {
        this.attackType = attackType;
    }

    public Action OnTakeDamage { get; set; }
    public void TakeDamage()
    {
        if (!animator.GetBool(AnimatorHashes.Death))
        {
            sword.StopAttack();
            OnTakeDamage?.Invoke();
            DisableSlowMo();
            PlayDeathAnimation();
            SpawnBlood();
            Observer.Instance.CallOnLoseLevel();
        }
    }

    private static void DisableSlowMo()
    {
        SlowMoData slowMoData;
        slowMoData.delay = 0;
        slowMoData.duration = 0;
        Observer.Instance.OnEnableSlowMo(slowMoData);
    }

    private void PlayDeathAnimation()
    {
        animator.applyRootMotion = true;
        animator.SetFloat(AnimatorHashes.DeathType, Random.Range(0, 2));
        animator.SetBool(AnimatorHashes.Death, true);
    }

    private void SpawnBlood()
    {
        ObjectPooler.Instance.SpawnObject("Blood", 
                                          transform.position - transform.forward,
                                          Quaternion.LookRotation(-transform.forward));
    }
}
