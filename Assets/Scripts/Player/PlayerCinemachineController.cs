using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerCinemachineController : MonoBehaviour
{
    [SerializeField] private Transform generalCamTarget;
    [SerializeField] private Transform bossFightCamTarget;

    [SerializeField] private CinemachineVirtualCamera fullFollowCamera;
    [SerializeField] private CinemachineVirtualCamera aimCamera;

    private void OnEnable()
    {
        Observer.Instance.OnFightStarted += FollowGeneralTarget;
        Observer.Instance.OnNextEnemyPushed += delegate(EnemyController enemy)
        {
            AimCustomTarget(generalCamTarget, enemy.transform);
        };
        
        Observer.Instance.OnBossEnemyPushed += delegate(EnemyController boss)
        {
            FollowCustomTarget(bossFightCamTarget);
            //AimCustomTarget(bossFightCamTarget, boss.transform);
        };
        
        Observer.Instance.OnCheckPointPassed += delegate
        {
            FollowGeneralTarget();
        };;
    }

    private void FollowGeneralTarget()
    {
        fullFollowCamera.Follow = generalCamTarget;
        //fullFollowCamera.LookAt = generalCamTarget;
     
        fullFollowCamera.Priority = 10;
        aimCamera.Priority = 5;
    }

    private void FollowCustomTarget(Transform target)
    {
        fullFollowCamera.Follow = target;
        //fullFollowCamera.LookAt = target;
     
        fullFollowCamera.Priority = 10;
        aimCamera.Priority = 5;
    }

    private void AimCustomTarget(Transform followTarget, Transform aimTarget)
    {
        aimCamera.Follow = followTarget;
        aimCamera.LookAt = aimTarget;
        
        aimCamera.Priority = 10;
        fullFollowCamera.Priority = 5;
    }
}
