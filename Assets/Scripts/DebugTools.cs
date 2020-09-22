using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTools : MonoBehaviour
{
    public int levelNumberToSet;
    
    [ContextMenu("Reset ALL!")]
    private void ResetAll()
    {
        ResetLevelData();
    }

    [ContextMenu("Reset level data")]
    private void ResetLevelData()
    {
        PlayerPrefs.DeleteKey(GameConstants.PrefsCurrentLevel);
    }
    
    [ContextMenu("Set level data")]
    private void SetLevelData()
    {
        PlayerPrefs.SetInt(GameConstants.PrefsCurrentLevel, levelNumberToSet);
    }

}
