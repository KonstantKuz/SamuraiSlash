using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private AttackType attackType;
    [SerializeField] private Sword sword;
    [SerializeField] private float slowMoStartTimePercentage;
    [SerializeField] private float slowMoDuration;
    
    private Animator animator;
    private EnemyController currentEnemy;
    
    private void OnEnable()
    {
        Observer.Instance.OnNextEnemyPushed += SetCurrentEnemy;
        SwipeDetector.OnSwipe += Attack;
    }

    private void SetCurrentEnemy(EnemyController enemy)
    {
        currentEnemy = enemy;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
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
        if (other.CompareTag(GameConstants.TagFightStarter))
        {
            animator.SetTrigger(AnimatorHashes.Idle);
            Observer.Instance.CallOnFightStarterTriggered();
        }
    }

    private void Update()
    {
        AimToCurrentEnemy();
    }
    
    private void AimToCurrentEnemy()
    {
        if (currentEnemy)
        {
            Vector3 targetDirection = currentEnemy.transform.position - transform.position;
            Vector3 targetDirectionXZ = Vector3.ProjectOnPlane(targetDirection, transform.up);
            Quaternion lookRotationAboutYAxis = Quaternion.LookRotation(targetDirectionXZ);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotationAboutYAxis, Time.deltaTime * 5f);
        }
    }
}
