using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    [SerializeField] private float rayDistance;
    
    private Animator animator;
    private bool attacked = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        if (attacked)
            return;
        
        Ray ray = new Ray(transform.position + transform.up, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.CompareTag(GameConstants.TagPlayer))
            {
                attacked = true;
                Attack();
            }
        }
    }

    private void Attack()
    {
        animator.SetTrigger(AnimatorHashes.Attack);
        EnableSlowMo();
    }

    private void EnableSlowMo()
    {
        SlowMoData slowMoData = new SlowMoData();
        slowMoData.delay = GameConstants.SlowMoDelayOnEnemyAttack;
        slowMoData.duration = GameConstants.SlowMoDurationOnEnemyAttack;
        //slowMoData.animatorsSpeed = GameConstants.SlowMoAnimatorsSpeedOnEnemyAttack;
        Observer.Instance.OnEnableSlowMo(slowMoData);
    }


    public void TakeDamage()
    {
        if (!animator.GetBool(AnimatorHashes.Die))
        {
            animator.SetBool(AnimatorHashes.Die, true);
        }
    }
}
