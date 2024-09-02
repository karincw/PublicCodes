using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowHighlight : MonoBehaviour
{
    public Material MovementGlowMat;
    public Material AttackGlowMat;

    private Material _movementGlowMat;
    private Material _attackGlowMat;
    private Material _originMat;
    private Renderer _renderer;

    private Tile _tile;

    private bool isGlowing = false;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _tile = GetComponentInParent<Tile>();
        _movementGlowMat = new Material(MovementGlowMat);
        _attackGlowMat = new Material(AttackGlowMat);
    }

    private void Start()
    {
        _originMat = _renderer.material;

        _movementGlowMat.color = _originMat.color;
        _attackGlowMat.color = _originMat.color;
    }

    public void ToggleMovementGlow(bool state)
    {
        if (_tile.iscloud) return;
        switch (state)
        {
            case true:
                _renderer.material = _movementGlowMat;
                isGlowing = true;
                break;

            case false:
                _renderer.material = _originMat;
                isGlowing = !isGlowing;
                break;
        }
    }
    public void ToggleAttackGlow(bool state)
    {
        if (_tile.iscloud) return;
        switch (state)
        {
            case true:
                _renderer.material = _attackGlowMat;
                isGlowing = true;
                break;

            case false:
                _renderer.material = _originMat;
                isGlowing = !isGlowing;
                break;
        }
    }
}
