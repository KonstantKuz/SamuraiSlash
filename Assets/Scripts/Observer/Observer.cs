using System;
using UnityEngine;
using Singleton;
public class Observer : Singleton<Observer>
{
    public Action OnLoseLevel = delegate { Debug.Log("OnLoseLevel trigerred"); };
    public Action OnWinLevel = delegate { Debug.Log("OnWinLevel trigerred"); };

    public Action OnStartGame = delegate { Debug.Log("OnStartGame trigerred"); };
    public Action OnRestartGame = delegate { Debug.Log("OnRestart trigerred"); };

    public Action OnLoadMainMenu = delegate { Debug.Log("OnLoadMainMenu trigerred"); };
    public Action<int> OnLevelManagerLoaded = delegate { Debug.Log("OnLevelManagerLoaded trigerred"); };

    public Action OnLoadNextLevel = delegate { Debug.Log("OnLoadNextLevel trigerred"); };
    public Action<int> OnAddingScore = delegate { Debug.Log("OnAddingScore trigerred"); };
    public Action<StimulType> OnGetStimulationText = delegate { Debug.Log("OnGetStimulationText triggered"); };
    public Action OnLeftMouseButtonDown;

    private bool isLevelCompleted = false;
    public void CallOnWinLevel()
    {
        if (!isLevelCompleted)
        {
            isLevelCompleted = true;
            OnWinLevel();
        }
    }
    public void CallOnLoseLevel()
    {
        if(!isLevelCompleted)
        {
            isLevelCompleted = true;
            OnLoseLevel();
        }
    }
}