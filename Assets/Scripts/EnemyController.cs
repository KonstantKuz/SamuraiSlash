using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Security.AccessControl;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour, IDamageable
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform attackRaycaster;
    [SerializeField] private float attackRayLength;

    private Animator animator;
    private CharacterController controller;
    private Transform currentAimTarget;
    private PlayerController playerController;

    public Action OnTakeDamage { get; set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        playerController = FindObjectOfType<PlayerController>();
    }

    public void GoToStartPoint()
    {
        animator.SetFloat(AnimatorHashes.Motion, 1f);
        currentAimTarget = startPoint;
        
        StartCoroutine(AimToPlayerAfterFinishStartPoint());

        IEnumerator AimToPlayerAfterFinishStartPoint()
        {
            while ((transform.position - currentAimTarget.position).magnitude > 0.5f)
            {
                yield return null;
            }
            
            animator.SetFloat(AnimatorHashes.Motion, 0f);
            currentAimTarget = playerController.transform;
        }
    }

    public void GoToPlayer()
    {
        animator.SetFloat(AnimatorHashes.Motion, 1f);
    }

    public void GoRunAway()
    {
        Vector3 targetDirection = playerController.transform.forward;
        Vector3 targetDirectionXZ = Vector3.ProjectOnPlane(targetDirection, transform.up);
        Quaternion lookRotationAboutYAxis = Quaternion.LookRotation(targetDirectionXZ);
        transform.rotation = lookRotationAboutYAxis;
        currentAimTarget = attackRaycaster;
        animator.SetFloat(AnimatorHashes.Motion,1f);
    }
    
    private void Update()
    {
        if (animator.GetBool(AnimatorHashes.Attack) || animator.GetBool(AnimatorHashes.Death))
            return;
        
        AimToCurrentTarget();

        FindPlayerToAttack();
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

    private void FindPlayerToAttack()
    {
        Ray ray = new Ray(attackRaycaster.position, attackRaycaster.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, attackRayLength))
        {
            if (hit.collider.CompareTag(GameConstants.TagPlayer))
            {
                Attack();
            }
        }
    }

    private void Attack()
    {
        //animator.applyRootMotion = false;
        animator.SetFloat(AnimatorHashes.AttackType, Random.Range(0,3));
        animator.SetBool(AnimatorHashes.Attack, true);
        EnableSlowMo();
    }

    private void EnableSlowMo()
    {
        SlowMoData slowMoData = new SlowMoData();
        slowMoData.delay = GameConstants.SlowMoDelayOnEnemyAttack;
        slowMoData.duration = GameConstants.SlowMoDurationOnEnemyAttack;
        Observer.Instance.OnEnableSlowMo(slowMoData);
    }

    public void SlowDown()
    {
        animator.speed = 0.3f;
    }
    
    public void TakeDamage()
    {
        if (!animator.GetBool(AnimatorHashes.Death))
        {
            animator.speed = 1f;
            controller.enabled = false;
            //animator.applyRootMotion = true;
            animator.SetFloat(AnimatorHashes.DeathType, Random.Range(0,2));
            animator.SetBool(AnimatorHashes.Death, true);
            Observer.Instance.OnEnemyDied();
            OnTakeDamage?.Invoke();
            SpawnBlood();
        }
    }

    private void SpawnBlood()
    {
        ObjectPooler.Instance.SpawnObject("Blood", attackRaycaster.position, attackRaycaster.rotation);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 attackRayStart = attackRaycaster.position;
        Vector3 attackRayEnd = attackRaycaster.position + attackRaycaster.forward * attackRayLength;
        Gizmos.DrawLine(attackRayStart, attackRayEnd);
    }
}
