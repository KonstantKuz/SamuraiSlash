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
            other.GetComponent<PlayerController>().SetAttackType(AttackType.Super);
            
            foreach (EnemyController enemy in pointEnemies)
            {
                enemy.GoToStartPoint();
            }
            foreach (EnemyController enemy in escapeEnemies)
            {
                enemy.GoToStartPoint();
            }

            StartDelayedAttack();
            Observer.Instance.OnEnemyDied += KillLeftEnemies;
            Observer.Instance.OnEnemyDied += Escape;
        }
    }

    private void StartDelayedAttack()
    {
        StartCoroutine(DelayedAttack());
        IEnumerator DelayedAttack()
        {
            yield return new WaitForSeconds(firstAttackDelay);
            Observer.Instance.OnNextEnemyPushed(pointEnemies[0]);
            for (int i = 0; i < pointEnemies.Length; i++)
            {
                pointEnemies[i].GoToPlayer();
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    private void KillLeftEnemies()
    {
        Observer.Instance.OnEnemyDied -= KillLeftEnemies;
        for (int i = 1; i < pointEnemies.Length; i++)
        {
            pointEnemies[i].TakeDamage();
        }
    }

    private void Escape()
    {
        Observer.Instance.OnEnemyDied -= Escape;
        for (int i = 0; i < escapeEnemies.Length; i++)
        {
            escapeEnemies[i].GoRunAway();
        }
    }
}
