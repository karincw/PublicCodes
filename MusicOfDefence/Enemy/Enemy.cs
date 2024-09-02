using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : Poolable
{
    [SerializeField] private float _moveSpeed = 1f;

    private Vector2 _attackPos;
    private Rigidbody2D _rig2d;

    private void Start()
    {
        _attackPos = GameManager.Instance.TowerTrm.position;
        _rig2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float targetDirX = _attackPos.x - _rig2d.position.x;
        Vector2 targetDir = new Vector2(targetDirX, 0);
        _rig2d.MovePosition(_rig2d.position + targetDir.normalized * Time.fixedDeltaTime * _moveSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Castle"))
        {
            GameManager.Instance.CastleHealth.DecreaseHealth(10);
            Release();
        }
    }

    public void Hit()
    {
        Debug.Log($"Hit {gameObject.name}"); 
        Release();
    }

}
