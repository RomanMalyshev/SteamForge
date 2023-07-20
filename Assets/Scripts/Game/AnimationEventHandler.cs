using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationEventHandler : MonoBehaviour
{
    public Action Attack;
    public Action AttackAnimationEnd;
    public Animator Animator;
    private static readonly int Attack1 = Animator.StringToHash("Attack");

    public void PlayMeleeAttackAnimation()
    {
        Animator.SetTrigger(Attack1);
    }

    public void OnAttack()
    {
        Attack?.Invoke();
    }
    
    public void OnAttackEnd()
    {
        AttackAnimationEnd?.Invoke();
    }
}
