using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightCheckPoint : MonoBehaviour, IFightCheckPoint
{
    [SerializeField] private Transform playerAimTarget;

    public Transform PlayerAimTarget
    {
        get { return playerAimTarget; }
    }

    [Tooltip("задержка перед первым нападением на игрока")]
    [SerializeField] private float firstAttackDelay;

    [Tooltip("задержка перед следующими нападениями")]
    [SerializeField] private float regularAttackDelay;
    
    [Tooltip("враги нападающие при входе игрока в этот триггер")]
    [SerializeField] private EnemyController[] pointEnemies;

    private int currentEnemyIndex = 0;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameConstants.TagPlayer))
        {
            other.GetComponent<PlayerController>().SetAttackType(AttackType.Simple);
            
            foreach (EnemyController enemy in pointEnemies)
            {
                enemy.GoToStartPoint();
            }

            DisableCollider();
            StartDelayedFirstAttack();
            Observer.Instance.OnEnemyDied += PushNextEnemy;
        }
    }

    private void StartDelayedFirstAttack()
    {
        StartCoroutine(DelayedFirstAttack());
        IEnumerator DelayedFirstAttack()
        {
            yield return new WaitForSeconds(firstAttackDelay);
            PushNextEnemy();
            Observer.Instance.OnFightStarted();
        }
    }

    private void PushNextEnemy()
    {
        if (currentEnemyIndex >= pointEnemies.Length)
        {
            StartCoroutine(DelayedPass());
            IEnumerator DelayedPass()
            {
                yield return new WaitForSeconds(1f);
                Observer.Instance.OnCheckPointPassed();
            }
            //Observer.Instance.OnCheckPointPassed();
            return;
        }

        StartCoroutine(DelayedPushNextEnemy());
        IEnumerator DelayedPushNextEnemy()
        {
            yield return new WaitForSeconds(regularAttackDelay);
            pointEnemies[currentEnemyIndex].GoToPlayer();
            Observer.Instance.OnNextEnemyPushed(pointEnemies[currentEnemyIndex]);
            currentEnemyIndex++;
        }
    }

    private void DisableCollider()
    {
        GetComponent<Collider>().enabled = false;
    }
}

public interface IFightCheckPoint
{
    Transform PlayerAimTarget { get; }
}