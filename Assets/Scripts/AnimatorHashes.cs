using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHashes : MonoBehaviour
{
    public static int Idle;
    public static int AttackLR;
    public static int AttackRL;
    public static int AttackTD;
    public static int AttackDT;

    public static int Motion;
    public static int Attack;
    public static int Die;

    private void OnEnable()
    {
        Idle = Animator.StringToHash("Idle");
        AttackLR = Animator.StringToHash("AttackLR");
        AttackRL = Animator.StringToHash("AttackRL");
        AttackTD = Animator.StringToHash("AttackTD");
        AttackDT = Animator.StringToHash("AttackDT");

        Motion = Animator.StringToHash("Motion");
        Attack = Animator.StringToHash("Attack");
        Die = Animator.StringToHash("Die");
    }
}
