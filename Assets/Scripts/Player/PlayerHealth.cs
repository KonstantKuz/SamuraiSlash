using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public Action OnTakeDamage { get; set; }

    public void TakeDamage()
    {
        if (!animator.GetBool(AnimatorHashes.Death))
        {
            animator.speed = 1f;
            //controller.enabled = false;
            animator.applyRootMotion = true;
            animator.SetFloat(AnimatorHashes.DeathType, Random.Range(0,2));
            animator.SetBool(AnimatorHashes.Death, true);
            //Observer.Instance.OnEnemyDied();
            //OnTakeDamage?.Invoke();
            SpawnBlood();
        }
    }

    private void SpawnBlood()
    {
        ObjectPooler.Instance.SpawnObject("Blood", 
                                          transform.position - transform.forward,
                                          Quaternion.LookRotation(-transform.forward));
    }
}
