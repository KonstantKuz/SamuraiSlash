using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private FightCheckPoint[] checkPoints;
    
    [SerializeField] private AttackType attackType;
    [SerializeField] private Sword sword;
    [Tooltip("это ПРОЦЕНТНОЕ соотношение точки старта слоумо относительно анимации удара" +
             "то есть если значение равно 0.2 то слоумо включится почти сразу в начале анимации удара" +
             "если равно 0.8 - слоумо включится почти в конце анимации удара" +
             "сделано так потому что одна анимация удара может длится дольше чем другая" +
             "и тогда слоумо будет включаться либо уже после фактического удара по врагу" +
             "либо перед ударом")]
    [SerializeField] private float slowMoStartTimePercentage;
    [SerializeField] private float slowMoDuration;
    
    private Animator animator;
    private EnemyController currentEnemy;

    private Transform currentAimTarget;
    private int currentCheckPointIndex;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    private void OnEnable()
    {
        Observer.Instance.OnNextEnemyPushed += SetCurrentTarget;
        Observer.Instance.OnCheckPointPassed += Go;
        Observer.Instance.OnCheckPointPassed += ClearCurrentEnemy;
        SwipeDetector.OnSwipe += Attack;
    }

    private void SetCurrentTarget(EnemyController enemy)
    {
        currentEnemy = enemy;
        currentAimTarget = enemy.transform;
    }

    private void ClearCurrentEnemy()
    {
        currentEnemy = null;
    }
    
    private void Attack(SwipeData swipe)
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
        
        EnableSlowMo();
        
        sword.StartAttack(attackType);
        currentEnemy.SlowDown();
    }

    private void EnableSlowMo()
    {
        SlowMoData slowMoData = new SlowMoData();
        slowMoData.delay = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length * slowMoStartTimePercentage;
        slowMoData.duration = slowMoDuration;
        Observer.Instance.OnEnableSlowMo(slowMoData);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameConstants.TagFightCheckPoint))
        {
            UpdateCheckPointIndex();
            Stop();
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

    private void Go()
    {
        if (currentCheckPointIndex >= checkPoints.Length)
        {
            Debug.Log("all check points passed");
            return;
        }
        currentAimTarget = checkPoints[currentCheckPointIndex].transform;
        animator.SetTrigger(AnimatorHashes.Run);
    }

    private void Update()
    {
        AimToCurrentTarget();
    }
    
    private void AimToCurrentTarget()
    {
        if (currentAimTarget)
        {
            Vector3 targetDirection = currentAimTarget.position - transform.position;
            Vector3 targetDirectionXZ = Vector3.ProjectOnPlane(targetDirection, transform.up);
            Quaternion lookRotationAboutYAxis = Quaternion.LookRotation(targetDirectionXZ);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotationAboutYAxis, Time.deltaTime * 5f);
        }
    }
}
