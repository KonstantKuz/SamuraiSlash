using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHashes : MonoBehaviour
{
    public static int Idle;
    public static int AttackRight;
    public static int AttackLeft;
    public static int AttackDown;
    public static int AttackUp;
    public static int SuperAttack;

    public static int Motion;
    public static int Attack;
    public static int AttackType;
    public static int Death;
    public static int DeathType;

    private void OnEnable()
    {
        Idle = Animator.StringToHash("Idle");
        AttackRight = Animator.StringToHash("AttackRight");
        AttackLeft = Animator.StringToHash("AttackLeft");
        AttackDown = Animator.StringToHash("AttackDown");
        AttackUp = Animator.StringToHash("AttackUp");
        SuperAttack = Animator.StringToHash("SuperAttack");

        Motion = Animator.StringToHash("Motion");
        Attack = Animator.StringToHash("Attack");
        AttackType = Animator.StringToHash("AttackType");
        Death = Animator.StringToHash("Death");
        DeathType = Animator.StringToHash("DeathType");
    }
}
