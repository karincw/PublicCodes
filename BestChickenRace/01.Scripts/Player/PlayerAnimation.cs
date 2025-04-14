using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : Player
{
    SpriteRenderer _spriterenderer;
    Animator _animator;

    private void Awake()
    {
        _spriterenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }


    public void SetAniSpeedValue(float value)
    {
        GameModePlay(() => _animator.SetFloat("Speed", value));
    }

    public void SetAniJumpValue(bool value)
    {
        GameModePlay(() => _animator.SetBool("Jump", value));
    }

    public void SetAniWallJumpValue(bool value)
    {
        GameModePlay(() => _animator.SetBool("Landing", value));
    }

    public void SetAniFalling(bool value)
    {
        GameModePlay(() => _animator.SetBool("Falling", value));
    }

    public void SetPlayerFlip(bool value)
    {
        GameModePlay(() => _spriterenderer.flipX = value);
    }

    public void SetWinAnimation()
    {
        GameModePlay(() => { Debug.Log(gameObject.name + "통과완료"); });
    }

}
