using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] public GameObject mainMenuPanel;
    [SerializeField] public GameObject losePanel;
    [SerializeField] public GameObject winPanel;
    
    private List<GameObject> allPanels = new List<GameObject>();
    
    private void OnEnable()
    {
        SubscribeToNecessaryEvets();
    }

    public void SubscribeToNecessaryEvets()
    {
        Observer.Instance.OnLoadMainMenu += delegate { ActivatePanel(mainMenuPanel); };
        Observer.Instance.OnLoseLevel += delegate { ActivatePanel(losePanel); };
        Observer.Instance.OnWinLevel += delegate { ActivatePanel(winPanel); };
    }

    private void Start()
    {
        CachePanels();
    }

    private void CachePanels()
    {
        allPanels.Add(mainMenuPanel);
        allPanels.Add(losePanel);
        allPanels.Add(winPanel);
    }
    
    
    private void ActivatePanel(GameObject panel)
    {
        DeactivateAllPanels();
        panel.SetActive(true);
    }
    
    private void DeactivateAllPanels()
    {
        for (int i = 0; i < allPanels.Count; i++)
        {
            allPanels[i].SetActive(false);
        }
    }

    /// <summary>
    /// Метод для вызова с кнопки UI
    /// </summary>
    public void StartGame()
    {
        Observer.Instance.OnStartGame();
    }
    /// <summary>
    /// Метод для вызова с кнопки UI (загрузить следующий уровень)
    /// </summary>
    public void LoadNextLevel()
    {
        Observer.Instance.OnLoadNextLevel();
    }
    /// <summary>
    /// Метод для вызова с кнопки UI (загрузить следующий уровень)
    /// </summary>
    public void RestartGame()
    {
        Observer.Instance.OnRestartGame();
    }
}
