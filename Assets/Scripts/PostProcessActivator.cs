using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessActivator : MonoBehaviour
{
    [SerializeField] private bool useVignette;
    [SerializeField] private Volume postProcess;

    private Vignette vignette;
    
    private void OnEnable()
    {
        Observer.Instance.OnEnemyDied += ActivatePostProcess;
        Observer.Instance.OnWinLevel += DisablePostProcess;
    }

    private void OnValidate()
    {
        postProcess.profile.TryGet(out vignette);
        if (vignette != null)
        {
            vignette.active = useVignette;
        }
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