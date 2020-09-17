using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class BossFightCheckPoint : MonoBehaviour
{
    [Tooltip("задержка перед первым нападением на игрока")]
    [SerializeField] private float firstAttackDelay;
    
    [Tooltip("враги нападающие при входе игрока в этот триггер")]
    [SerializeField] private EnemyController[] pointEnemies;

    [Tooltip("враги которые должны убежать")]
    [SerializeField] private EnemyController[] escapeEnemies;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameConstants.TagPlayer))
        {
            DisableCollider();
            other.GetComponent<PlayerController>().SetAttackType(AttackType.Super);

            for (int i = 0; i < pointEnemies.Length; i++)
            {
                pointEnemies[i].GoToStartPoint();
            }
            for (int i = 0; i < escapeEnemies.Length; i++)
            {
                escapeEnemies[i].GoToStartPoint();
            }

            StartDelayedAttack();
        }
    }

    private void StartDelayedAttack()
    {
        StartCoroutine(DelayedAttack());
        IEnumerator DelayedAttack()
        {
            yield return new WaitForSeconds(firstAttackDelay);
            
            pointEnemies[0].GoToPlayer();
            Observer.Instance.OnNextEnemyPushed(pointEnemies[0]);
            for (int i = 1; i < pointEnemies.Length; i++)
            {
                pointEnemies[i].GoToPlayer();
            }
            
            pointEnemies[0].OnTakeDamage += Finish;
        }
    }

    private void Finish()
    {
        KillLeftEnemies();
        StartRunEscapeEnemies();
    }

    private void KillLeftEnemies()
    {
        for (int i = 1; i < pointEnemies.Length; i++)
        {
            pointEnemies[i].TakeDamage();
        }
    }

    private void StartRunEscapeEnemies()
    {
        for (int i = 0; i < escapeEnemies.Length; i++)
        {
            escapeEnemies[i].GoRunAway();
        }
    }

    private void DisableCollider()
    {
        GetComponent<Collider>().enabled = false;
    }
}
