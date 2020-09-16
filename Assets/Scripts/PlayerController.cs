using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Sword sword;
    [SerializeField] private float slowMoStartTimePercentage;
    [SerializeField] private float slowMoDuration;
    
    private Animator animator;
    
    private void OnEnable()
    {
        SwipeDetector.OnSwipe += Attack;
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
                animator.SetTrigger(AnimatorHashes.AttackDT);
                break;
            case SwipeDirection.Down:
                animator.SetTrigger(AnimatorHashes.AttackTD);
                break;
            case SwipeDirection.Left:
                animator.SetTrigger(AnimatorHashes.AttackRL);
                break;
            case SwipeDirection.Right:
                animator.SetTrigger(AnimatorHashes.AttackLR);
                break;
        }
        
        sword.StartAttack();

        SlowMoData slowMoData = new SlowMoData();
        slowMoData.delay = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length * slowMoStartTimePercentage;
        slowMoData.duration = slowMoDuration;
        //slowMoData.animatorsSpeed = 0.1f;
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
}
