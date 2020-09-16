using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    /// <summary>
    /// Вместо isGameStarted в каждом классе
    /// используй GameStarter.IsGameStarted
    /// </summary>
    private void OnEnable()
    {
        SubscribeToNecessaryEvets();
    }

    public void SubscribeToNecessaryEvets()
    {
    }
}
