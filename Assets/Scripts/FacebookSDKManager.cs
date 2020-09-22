using System;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using Singleton;
using UnityEngine;

public class FacebookSDKManager : Singleton<FacebookSDKManager>
{
    private void Awake ()
    {
        PreventDuplicate();
        DontDestroyOnLoad(gameObject);
        
        if (!FB.IsInitialized) {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        } else {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }
    
    private void PreventDuplicate()
    {
        FacebookSDKManager[] objs = FindObjectsOfType<FacebookSDKManager>();

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }
    }

    private void InitCallback ()
    {
        if (FB.IsInitialized) {
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        } else {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity (bool isGameShown)
    {
        if (!isGameShown) {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        } else {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

    public void LogStartLevel()
    {
        var tutParams = new Dictionary<string, object>();
        tutParams["LevelNumber"] = LevelManager.Instance.CurrentLevel;
        Debug.Log("<color=red>Start Level </color>" + LevelManager.Instance.CurrentLevel);

        FB.LogAppEvent (
            "Level Started",
            parameters: tutParams
        );    
    }
    
    public void LogLevelFailed()
    {
        var tutParams = new Dictionary<string, object>();
        tutParams["LevelNumber"] = LevelManager.Instance.CurrentLevel;

        FB.LogAppEvent (
            "Level Failed",
            parameters: tutParams
        );
        Debug.Log("<color=red>Failed Level </color>" + LevelManager.Instance.CurrentLevel);
    }
    
    public void LogLevelComplete()
    {
        var tutParams = new Dictionary<string, object>();
        tutParams["LevelNumber"] = LevelManager.Instance.CurrentLevel;
        Debug.Log("<color=red>LevelComplete = </color>" + LevelManager.Instance.CurrentLevel);

        FB.LogAppEvent (
            "Level Complete",
            parameters: tutParams
        );    
    }
}
