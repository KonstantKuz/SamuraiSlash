using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DamageText : MonoBehaviour
{
    [SerializeField] private Animator animator = null;
    [SerializeField] private TextMeshProUGUI damageText = null;

    public void SetUpDamageText(string damageText, Color color)
    {
        this.damageText.text = damageText;
        this.damageText.color = color;
        animator.CrossFadeInFixedTime("DamageAnim", 0,0,0);
    }
}
