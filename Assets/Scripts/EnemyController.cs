using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform attackRaycaster;
    [SerializeField] private float attackRayLength;
    //[SerializeField] private float attackRayHelpSphereRadius;

    private Transform currentAimTarget;
    private PlayerController playerController;
    private Animator animator;

    private void OnEnable()
    {
        Observer.Instance.OnFightStarterTriggered += GoToStartPoint;
    }

    private void GoToStartPoint()
    {
        animator.SetFloat(AnimatorHashes.Motion, 1f);
        currentAimTarget = startPoint;
        
        StartCoroutine(AimToPlayerAfterFinishStartPoint());

        IEnumerator AimToPlayerAfterFinishStartPoint()
        {
            while ((transform.position - currentAimTarget.position).magnitude > 1f)
            {
                yield return null;
            }
            
            animator.SetFloat(AnimatorHashes.Motion, 0f);
            currentAimTarget = playerController.transform;
        }
    }

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        if (animator.GetBool(AnimatorHashes.Attack) || animator.GetBool(AnimatorHashes.Die))
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
        animator.applyRootMotion = false;
        animator.SetBool(AnimatorHashes.Attack, true);
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
            animator.applyRootMotion = true;
            animator.SetBool(AnimatorHashes.Die, true);
            Observer.Instance.OnEnemyDied();
        }
    }

    public void GoToPlayer()
    {
        animator.SetFloat(AnimatorHashes.Motion, 1f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 attackRayStart = attackRaycaster.position;
        Vector3 attackRayEnd = attackRaycaster.position + attackRaycaster.forward * attackRayLength;
        Gizmos.DrawLine(attackRayStart, attackRayEnd);
        //Gizmos.DrawWireSphere(attackRayEnd, attackRayHelpSphereRadius);
    }
}
