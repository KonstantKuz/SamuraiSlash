using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int currentLevel;
    private int maxLevelCount;

    public int CurrentLevel => currentLevel;

    private void OnEnable()
    {
        maxLevelCount = SceneManager.sceneCountInBuildSettings;
        SubscribeToNecessaryEvets();
    }
    
    public void SubscribeToNecessaryEvets()
    {
        Observer.Instance.OnLoadNextLevel += LoadNextLevel;
        Observer.Instance.OnRestartGame += RestartLevel;
    }

    private void OnDestroy()
    {
        Observer.Instance.OnLoadNextLevel -= LoadNextLevel;
    }

    private void Start()
    {
        currentLevel = PlayerPrefs.HasKey(GameConstants.PrefsCurrentLevel)
            ? PlayerPrefs.GetInt(GameConstants.PrefsCurrentLevel)
            : 0;

        Observer.Instance.OnLevelManagerLoaded(currentLevel);
    }

    private void LoadNextLevel()
    {
        UpdateCurrentLevel();
        SceneManager.LoadScene(currentLevel);
    }

    private void UpdateCurrentLevel()
    {
        Debug.Log("UpdateActiveLevel");
        Debug.Log($"<color=red> Setted active level prefs to currentlevel = {currentLevel} </color>");
        currentLevel++;
        if(currentLevel >= maxLevelCount)
        {
            currentLevel = 0;
        }
        PlayerPrefs.SetInt(GameConstants.PrefsCurrentLevel, currentLevel);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    [ContextMenu("Clean prefs")]
    public void CleanPrefs()
    {
        PlayerPrefs.DeleteKey(GameConstants.PrefsCurrentLevel);
    }
}