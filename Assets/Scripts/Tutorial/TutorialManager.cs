using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject text;
    [SerializeField] private GameObject icon;

    private void OnEnable()
    {
        Observer.Instance.OnEnemyStartsAttack += ActivateTutorial;
        Observer.Instance.OnEnemyDied += DeactivateTutorial;
    }

    private void ActivateTutorial()
    {
        Debug.Log("Activate Tutorial!");
        text.gameObject.SetActive(true);
        icon.gameObject.SetActive(true);
    }

    private void DeactivateTutorial()
    {
        text.gameObject.SetActive(false);
        icon.gameObject.SetActive(false);
    }
}
