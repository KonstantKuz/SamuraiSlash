using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using Dreamteck.Splines;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Tooltip("это ПРОЦЕНТНОЕ соотношение точки старта слоумо относительно анимации удара" +
             "то есть если значение равно 0.2 то слоумо включится почти сразу в начале анимации удара" +
             "если равно 0.8 - слоумо включится почти в конце анимации удара" +
             "сделано так потому что одна анимация удара может длится дольше чем другая" +
             "и тогда слоумо будет включаться либо уже после фактического удара по врагу" +
             "либо перед ударом")]
    [SerializeField] private float slowMoStartTimePercentage;
    [SerializeField] private float slowMoDuration;
    
    [SerializeField] private Transform[] checkPoints;
    [SerializeField] private Transform cameraPosition;
    [SerializeField] private Sword sword;
    [SerializeField] private GameObject superAttackVFX;
    
    private Camera camera;
    private Animator animator;
    private EnemyController currentEnemy;

    private Transform currentAimTarget;
    private int currentCheckPointIndex;
    private AttackType attackType;

    private float playerLookAtSpeed = 3f;
    private float cameraSpeed = 5f;

    private void Awake()
    {
        camera = Camera.main;
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
        if (!currentEnemy)
        {
            return;
        }

        if (attackType == AttackType.Simple)
        {
            NormalAttack(swipe);
        }
        else
        {
            animator.SetTrigger(AnimatorHashes.SuperAttack);
        }

        EnableSlowMo();

        sword.StartAttack();
        currentEnemy.SlowDown();
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
        UpdateCamera();
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

    private void UpdateCamera()
    {
        camera.transform.position = Vector3.Lerp(camera.transform.position,
                                                 cameraPosition.position,
                                                 Time.deltaTime * cameraSpeed);
        camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, 
                                                    cameraPosition.rotation,
                                                    Time.deltaTime * cameraSpeed);
        
        if (!currentEnemy)
        {
            return;
        }

        Vector3 targetDirection = currentEnemy.transform.position - camera.transform.position;
        Quaternion fullLookRotation = Quaternion.LookRotation(targetDirection);
        camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, 
                                                    fullLookRotation, 
                                                    Time.deltaTime * cameraSpeed);
    }

    public void SetAttackType(AttackType attackType)
    {
        this.attackType = attackType;
    }
}
