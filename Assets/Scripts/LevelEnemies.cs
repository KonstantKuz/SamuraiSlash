using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LevelEnemies : MonoBehaviour
{
    [SerializeField] private EnemyController[] currentLevelEnemies;

    private bool firstAttack = true;
    private int currentEnemyIndex = 0;

    private void OnEnable()
    {
        Observer.Instance.OnFightStarterTriggered += PushEnemyToAttack;
        Observer.Instance.OnEnemyDied += PushEnemyToAttack;
    }

    private void PushEnemyToAttack()
    {
        StartCoroutine(PushEnemyToAttack());

        IEnumerator PushEnemyToAttack()
        {
            float delay = 0;
            if (firstAttack)
            {
                firstAttack = false;
                delay = 1f;
            }
            yield return new WaitForSeconds(delay);

            if (currentEnemyIndex >= currentLevelEnemies.Length)
                yield break;

            Observer.Instance.OnNextEnemyPushed(currentLevelEnemies[currentEnemyIndex]);
            currentLevelEnemies[currentEnemyIndex].GoToPlayer();
            currentEnemyIndex++;
        }
    }
}
