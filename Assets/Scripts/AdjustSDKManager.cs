using System.Collections;
using System.Collections.Generic;
using com.adjust.sdk;
using UnityEngine;

public class AdjustSDKManager : MonoBehaviour
{
    #if UNITY_IOS
    private void Start()
    {
        PreventDuplicate();
        DontDestroyOnLoad(gameObject);
        
        InitAdjust("8afc7uh6962o");
    }

    private void InitAdjust(string adjustAppToken)
    {
        var adjustConfig = new AdjustConfig(
            adjustAppToken,
            AdjustEnvironment.Production, // AdjustEnvironment.Sandbox to test in dashboard
            true
        );
        adjustConfig.setLogLevel(AdjustLogLevel.Info); // AdjustLogLevel.Suppress to disable logs
        adjustConfig.setSendInBackground(true);
        new GameObject("Adjust").AddComponent<Adjust>(); // do not remove or rename

        // Adjust.addSessionCallbackParameter("foo", "bar"); // if requested to set session-level parameters

        //adjustConfig.setAttributionChangedDelegate((adjustAttribution) => {
        //  Debug.LogFormat("Adjust Attribution Callback: ", adjustAttribution.trackerName);
        //});
        Adjust.start(adjustConfig);
    }
    
    private void PreventDuplicate()
    {
        AdjustSDKManager[] objs = FindObjectsOfType<AdjustSDKManager>();

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }
    }
    #endif

}
