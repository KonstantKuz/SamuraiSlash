using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using DG.Tweening;
using UnityEngine;

public class PlayerCustomCameraController : MonoBehaviour
{
    //[SerializeField] private CameraSettings cameraSettings;

    private Transform camera;

    [SerializeField] private Transform cinematicCamTarget;
    [SerializeField] private Transform generalCamTarget;
    [SerializeField] private Transform bossFightCamTarget;
    
    private Transform currentFollowTarget;
    private Transform currentRotationTarget;
    private Transform currentLookAtTarget;
    
    private void OnEnable()
    {
        camera = Camera.main.transform;
        
        currentFollowTarget = cinematicCamTarget;
        currentRotationTarget = cinematicCamTarget;
        
        Observer.Instance.OnFightStarted += delegate
        {
            currentFollowTarget = generalCamTarget;
            currentRotationTarget = generalCamTarget;
            currentLookAtTarget = null;
        };
        
        Observer.Instance.OnNextEnemyPushed += delegate(EnemyController enemy)
        {
            currentFollowTarget = generalCamTarget;
            currentRotationTarget = null;
            currentLookAtTarget = enemy.transform;
        };
        
        Observer.Instance.OnBossEnemyPushed += delegate(EnemyController boss)
        {
            currentFollowTarget = bossFightCamTarget;
            currentRotationTarget = bossFightCamTarget;
            currentLookAtTarget = null;
        };
        
        Observer.Instance.OnCheckPointPassed += delegate
        {
            currentFollowTarget = generalCamTarget;
            currentRotationTarget = generalCamTarget;
            currentLookAtTarget = null;
        };;
    }

    private void Update()
    {
        MoveCam();
        RotateCam();
        LookAtCam();
    }

    private void MoveCam()
    {
        if (currentFollowTarget == null)
            return;
        
        camera.transform.position = Vector3.Lerp(camera.transform.position,
                                                 currentFollowTarget.position,
                                                 Time.deltaTime * 5f);
    }

    private void RotateCam()
    {
        if (currentRotationTarget == null)
            return;
        
        camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation,
                                                    currentRotationTarget.rotation, 
                                                    Time.deltaTime * 5f);
    }

    private void LookAtCam()
    {
        if (currentLookAtTarget == null)
            return;
        
        Quaternion lookRotation = Quaternion.LookRotation(currentLookAtTarget.position - camera.transform.position);
        camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, 
                                                    lookRotation,
                                                    Time.deltaTime * 5f);
    }
    
    

    // private void UpdateCamera()
    // {
    //     if (currentEnemy)
    //     {
    //         FightCamUpdate();
    //     }
    //     else
    //     {
    //         NormalCamUpdate();
    //     }
    // }
    //
    // private void NormalCamUpdate()
    // {
    //     camera.transform.position = Vector3.Lerp(camera.transform.position,
    //                                              cameraPosition.position,
    //                                              Time.deltaTime * cameraSettings.normalFollowSpeed);
    //     camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation,
    //                                                 cameraPosition.rotation,
    //                                                 Time.deltaTime * cameraSettings.normalRotationSpeed);
    // }
    //
    // private void FightCamUpdate()
    // {
    //     camera.transform.position = Vector3.Lerp(camera.transform.position,
    //                                              cameraPosition.position,
    //                                              Time.deltaTime * cameraSettings.fightFollowSpeed);
    //
    //     Vector3 targetDirection = currentEnemy.transform.position - camera.transform.position;
    //     Quaternion fullLookRotation = Quaternion.LookRotation(targetDirection);
    //     camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation,
    //                                                 fullLookRotation,
    //                                                 Time.deltaTime * cameraSettings.fightRotationSpeed);
    // }
}
