using Karin.AStar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace Karin
{
    [RequireComponent(typeof(NavAgent))]
    [RequireComponent(typeof(AgentMovement))]

    public class ControlableCharacter : MonoBehaviour
    {
        NavAgent _navAgent;
        public AgentMovement _agentMovement;
        [SerializeField] InputReaderSO _inputReader;
        public CharacterSO _characterData;
        private float attackTimer = 0;
        AgentAnimator _animator;

        [SerializeField] CircleCollider2D _collider;

        public float atk => _characterData.atk;
        public float atkspeed => _characterData.atkspeed;
        public float range => _characterData.range;
        private float currentHP = 0;
        public float CurrentHP
        {
            get { return currentHP; }
            set
            {
                currentHP = value;
                if (currentHP <= 0) { Debug.Log($"Did : {this.name}"); Destroy(this.gameObject); }
            }
        }
        public bool IsMove => _agentMovement.IsMove;
        public float AttackBonus = 1;
        public AntType type => _characterData.TYPE;

        private float timer = 0;

        [SerializeField] private string findObjectTag;
        private List<EnemyAgent> attackableList = new List<EnemyAgent>();

        private void Awake()
        {
            _animator = GetComponentInChildren<AgentAnimator>();
            _navAgent = GetComponent<NavAgent>();
            _agentMovement = GetComponent<AgentMovement>();
        }

        private void Start()
        {
            currentHP = _characterData.maxHealth;
            _agentMovement.Speed = _characterData.MoveSpeed;
            _collider.radius = range;
        }

        private void Update()
        {
            currentHP = Mathf.Clamp(currentHP, 0, _characterData.maxHealth);

            if (attackableList != null)
                Attack();
        }

        private void Attack()
        {
            if (IsMove == true) return;
            if (timer >= atkspeed)
            {
                if (attackableList.Count == 0 || attackableList.First() == null) { return; }
                var attackAgent = attackableList.First();

                Debug.Log($"Attack {atk * AttackBonus}");
                attackAgent.CurrentHP -= atk * AttackBonus;
                var dir = attackAgent.transform.position - transform.position;
                if (dir.x > 0)
                {
                    _animator.LookRight();
                }
                else if (dir.x < 0)
                {
                    _animator.LookLeft();
                }
                _animator.Attack();

                timer = 0;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }


        public void Select()
        {
            _inputReader.InterectionEvent += Move;
        }
        public void DeSelect()
        {
            _inputReader.InterectionEvent -= Move;
        }

        private void Move(Vector2 World)
        {
            Vector3Int cellPos = MapManager.Instance.GetTilePos(World);
            _navAgent.Destination = cellPos;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == findObjectTag)
            {
                attackableList.Add(collision.gameObject.GetComponent<EnemyAgent>());
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag == findObjectTag)
            {
                attackableList.Remove(collision.gameObject.GetComponent<EnemyAgent>());
            }
        }

    }
}