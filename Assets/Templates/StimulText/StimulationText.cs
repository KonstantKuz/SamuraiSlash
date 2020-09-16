using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum StimulType
{
    None,
    Good,
    Perfect,
    Insane
}
[System.Serializable]
public class StimulAnimations
{
    [Tooltip("Важно : проигрываемая анимация должна находиться в одном из состояний аниматора. Имя состояния и анимации должны совпадать.")]
    public AnimationClip GoodAnimStateClip;
    public AnimationClip PerfectAnimStateClip;
    public AnimationClip InsaneAnimStateClip;

    public Dictionary<StimulType, string> stateDictionary;

    public void InitStateDictionary()
    {
        stateDictionary = new Dictionary<StimulType, string>();
        stateDictionary.Add(StimulType.Good, GoodAnimStateClip.name);
        stateDictionary.Add(StimulType.Perfect, PerfectAnimStateClip.name);
        stateDictionary.Add(StimulType.Insane, InsaneAnimStateClip.name);
    }
}

public class StimulationText : MonoBehaviour
{
    [Tooltip("Здесь можно изменить проигрываемое состояние при выдаче того или иного стимула")]
    public StimulAnimations animations;

    private Animator UIanimator;
    private TextMeshProUGUI text;

    private StimulType nextStimul;
    public bool isInsanePlaying { get; set; }

    private void Awake()
    {
        UIanimator = GetComponent<Animator>();
        text = GetComponent<TextMeshProUGUI>();
        animations.InitStateDictionary();
    }

    public void SetTextAndPlay(StimulType textTypeToSet)
    {
        if (isInsanePlaying)
        {
            return;
        }
        text.text = textTypeToSet.ToString();
        UIanimator.CrossFadeInFixedTime(animations.stateDictionary[textTypeToSet], 0f, 0, 0f,0f);
    }
}
