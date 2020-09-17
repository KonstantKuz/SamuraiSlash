using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public struct SlowMoData
{
    public float delay;
    public float duration;
}

public class SlowMoController : MonoBehaviour
{
    private void OnEnable()
    {
        Observer.Instance.OnEnableSlowMo += EnableSlowMo;
    }

    private void EnableSlowMo(SlowMoData data)
    {
        DisableSlowMo();
        StartCoroutine(HandleSlowMo());
        IEnumerator HandleSlowMo()
        {
            yield return new WaitForSeconds(data.delay);
            Time.timeScale = 0.05f;
            yield return new WaitForSeconds(data.duration);
            DisableSlowMo();
        }
    }

    private void DisableSlowMo()
    {
        Time.timeScale = 1f;
        //Observer.Instance.OnSlowMoDisabled();
    }
}
