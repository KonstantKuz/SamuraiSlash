using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Sword sword;
    [SerializeField] private float slowMoStartTimePercentage;
    [SerializeField] private float slowMoDurationPercentage;
    
    private Animator animator;
    
    private void OnEnable()
    {
        SwipeDetector.OnSwipe += Attack;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        animator.SetTrigger(AnimatorHashes.Idle);
    }
    
    private void Attack(SwipeData swipe)
    {
        animator.SetTrigger(AnimatorHashes.Idle);
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

        SlowMoData slowMoData = new SlowMoData();
        slowMoData.delay = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length * slowMoStartTimePercentage;
        slowMoData.duration = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length * slowMoDurationPercentage;
        //slowMoData.animatorsSpeed = 0.1f;
        Observer.Instance.OnEnableSlowMo(slowMoData);
        
        sword.Attack();
    }
    
    // [SerializeField] private Transform sword;
    //
    // private void Update()
    // {
    //     Ray ray = new Ray(sword.position, sword.up);
    //     if (Physics.Raycast(ray, out RaycastHit hit, 1f))
    //     {
    //         EnemyController enemyController = hit.transform.GetComponent<EnemyController>();
    //         if (enemyController != null)
    //         {
    //             enemyController.Die();
    //         }
    //     }
    // }
}
