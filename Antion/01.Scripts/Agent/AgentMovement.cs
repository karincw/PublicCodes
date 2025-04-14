using System;
using UnityEngine;

namespace Karin.AStar
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class AgentMovement : MonoBehaviour
    {
        private Rigidbody2D _rig2d;

        private Vector2 _movementDirection;
        AgentAnimator _animator;

        public bool IsMove { get { return _movementDirection == Vector2Int.zero ? false : true; } }

        [Header("Attribute")]
        public float Speed = 1.0f;

        private void Awake()
        {
            _animator = GetComponentInChildren<AgentAnimator>();
            _rig2d = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            _rig2d.velocity = _movementDirection * Speed * Time.fixedDeltaTime;
        }

        public void Movement(Vector2 dir)
        {
            if (dir.x > 0)
            {
                _animator.LookRight();
            }
            else if (dir.x < 0)
            {
                _animator.LookLeft();
            }

            if (dir != Vector2.zero)
            {
                _movementDirection = dir.normalized;
                _animator.Move(true);
            }
            else
            {
                _animator.Move(false);
                _movementDirection = Vector2.zero;
            }
        }



    }
}
