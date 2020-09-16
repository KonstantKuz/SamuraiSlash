using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class EndPanelLevelText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;

    private void OnEnable()
    {
        SubscribeToNecessaryEvents();
    }

    private void SubscribeToNecessaryEvents()
    {
        Observer.Instance.OnLevelManagerLoaded += SetLevelText;
    }

    private void SetLevelText(int currentLevel)
    {
        levelText.SetText($"level {currentLevel.ToString()}");
    }
}
