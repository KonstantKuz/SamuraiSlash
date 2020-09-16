using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private int score;

    private void OnEnable()
    {
        SubscribeToNecessaryEvets();
    }
    public void SubscribeToNecessaryEvets()
    {
        Observer.Instance.OnAddingScore += AddScore;
    }

    private void OnDestroy()
    {
        Observer.Instance.OnAddingScore -= AddScore;
    }

    private void AddScore(int value)
    {
        score += value;
        UpdateScore();
    }

    private void UpdateScore()
    {
        scoreText.SetText(score.ToString());
    }
}
