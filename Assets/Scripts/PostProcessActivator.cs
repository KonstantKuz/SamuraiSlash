using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessActivator : MonoBehaviour
{
    [SerializeField] private Volume postProcess;

    private void OnEnable()
    {
        Observer.Instance.OnEnemyDied += ActivatePostProcess;
        Observer.Instance.OnWinLevel += DisablePostProcess;
    }

    private void ActivatePostProcess()
    {
        postProcess.enabled = true;
    }

    private void DisablePostProcess()
    {
        postProcess.enabled = false;
    }
}
