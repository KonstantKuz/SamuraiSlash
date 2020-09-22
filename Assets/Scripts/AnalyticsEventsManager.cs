using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyticsEventsManager : MonoBehaviour
{
    private void OnEnable()
    {
        Observer.Instance.OnStartGame += LogStartLevel;
        Observer.Instance.OnLoseLevel += LogLevelFailed;
        Observer.Instance.OnWinLevel += LogLevelComplete;
    }

    private void OnDisable()
    {
        if (Observer.Instance)
        {
            Observer.Instance.OnStartGame -= LogStartLevel;
            Observer.Instance.OnLoseLevel -= LogLevelFailed;
            Observer.Instance.OnWinLevel -= LogLevelComplete;
        }
    }

    private void LogStartLevel()
    {
        FacebookSDKManager.Instance.LogStartLevel();
    }
    
    private void LogLevelFailed()
    {
        FacebookSDKManager.Instance.LogLevelFailed();
    }
    
    private void LogLevelComplete()
    {
        FacebookSDKManager.Instance.LogLevelComplete();
    }
}
