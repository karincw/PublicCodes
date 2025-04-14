using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class AgentAnimator : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Move(bool state)
    {
        animator.SetBool("Move", state);
    }
    public void Attack()
    {
        animator.SetTrigger("Attack");
    }


    public void LookRight()
    {
        spriteRenderer.flipX = true;
    }
    public void LookLeft()
    {
        spriteRenderer.flipX = false;
    }
}
