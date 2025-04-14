using Karin.AStar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Karin
{
    public enum FSMState
    {
        Chase,
        Attack
    }

    public class EnemyAgent : MonoBehaviour, IDetecterUser
    {
        [SerializeField] private float slowmultiple = 0;
        [SerializeField] private float slowTime = 0;
        [SerializeField] private float poisoncount = 0;
        [SerializeField] private float poisondamage = 0;
        [SerializeField] private EnemySO enemyData;
        private AgentMovement _agentMovement;
        private NavAgent _navAgent;
        private FSMState _state = FSMState.Chase;
        private Vector3Int attackPos;
        private float timer = 0;
        AgentAnimator _animator;
        bool cjdma = false;

        [SerializeField] private LayerMask enemyMask;

        private float currentHP = 1;
        public float Atk;
        public float CurrentHP
        {
            get
            {
                return currentHP;
            }
            set
            {
                currentHP = value;
                if (currentHP <= 0)
                {
                    ResourceManager.Instance.BoneCount += Random.Range(0, 3);
                    Debug.Log($"Did : {this.name}");
                    Destroy(this.gameObject);
                }
            }
        }

        public List<GameObject> DetectedObject { get => detectedObject; set { detectedObject = value; Chase(); } }
        public List<GameObject> detectedObject = new List<GameObject>();

        private void Awake()
        {
            _animator = GetComponentInChildren<AgentAnimator>();
            _agentMovement = GetComponent<AgentMovement>();
            _navAgent = GetComponent<NavAgent>();
        }

        private void OnEnable()
        {
            _navAgent.NotFoundAction += MidMove;
        }
        private void OnDisable()
        {
            _navAgent.NotFoundAction -= MidMove;
        }

        private void MidMove()
        {
            if (CanChase(MapManager.Instance.GetTilePos(new Vector3(Random.Range(-9, 9), Random.Range(-9, 9))), out var midPos) && _agentMovement.IsMove == false)
            {
                _navAgent.Destination = midPos;
            }
        }

        private void Start()
        {
            _agentMovement.Speed = enemyData.MoveSpeed;
            _state = FSMState.Chase;
            cjdma = false;
        }
        private void Update()
        {
            if (_state == FSMState.Chase)
            {
                Chase();
            }
            else if (_state == FSMState.Attack)
            {
                Attack();
            }
        }

        private void Chase()
        {
            if (CanAttack())
            {
                _state = FSMState.Attack;
            }
            else
            {
                try
                {
                    foreach (var t in DetectedObject.OrderBy(t => Vector3.Distance(t.transform.position, transform.position)))
                    {
                        if (t == null) continue;
                        if (CanChase(MapManager.Instance.GetTilePos(t.transform.position), out var movePos))
                        {
                            _navAgent.Destination = movePos;
                            break;
                        }
                    }

                    //if (cjdma == false)
                    //{
                    //    if (CanChase(MapManager.Instance.GetTilePos(new Vector3(Random.Range(-4, 4), Random.Range(-4, 4))), out var midPos))
                    //    {
                    //        _navAgent.Destination = midPos;
                    //        cjdma = true;
                    //    }
                    //}


                }
                catch (Exception ex)
                { Debug.Log(ex.Message); }
            }
        }

        private bool CanChase(Vector3Int AttackPos, out Vector3Int movePos)
        {
            foreach (var dir in Direction.Directions)
            {
                if (MapManager.Instance.collisionMap.GetTile(AttackPos + (Vector3Int)dir) == null)
                {
                    movePos = AttackPos + (Vector3Int)dir;
                    return true;
                }
            }
            movePos = Vector3Int.zero;
            return false;
        }

        private bool CanAttack()
        {
            Vector3Int pos = MapManager.Instance.GetTilePos(transform.position);
            var col = Physics2D.OverlapCircle(MapManager.Instance.GetWorldPos(pos), 1.3f, enemyMask);
            if (col != null)
            {
                return true;
            }
            return false;
        }
        private bool CanAttack(out GameObject attackObj)
        {
            Vector3Int pos = MapManager.Instance.GetTilePos(transform.position);
            foreach (var dir in Direction.Directions)
            {
                var col = Physics2D.OverlapCircle(MapManager.Instance.GetWorldPos(pos + (Vector3Int)dir), 0.3f, enemyMask);
                if (col != null)
                {
                    attackObj = col.gameObject;
                    return true;
                }

            }
            attackObj = null;
            return false;
        }

        private void Attack()
        {
            try
            {
                if (CanAttack(out GameObject obj))
                {
                    Debug.Log(obj);
                    if (obj == null) return;
                    var controlableCharacter = obj.GetComponentInParent<ControlableCharacter>();
                    if (controlableCharacter != null)
                    {
                        if (timer >= enemyData.atkspeed)
                        {
                            timer = 0;
                            var dir = controlableCharacter.transform.position - transform.position;
                            if (dir.x > 0)
                            {
                                _animator.LookRight();
                            }
                            else if (dir.x < 0)
                            {
                                _animator.LookLeft();
                            }
                            _animator.Attack();
                            controlableCharacter.CurrentHP -= Atk;
                            if(slowTime != 0)
                            {
                                StartCoroutine(SlowEffect(controlableCharacter));
                            }
                            if(poisoncount != 0)
                            {
                                StartCoroutine(PoisonEffect(controlableCharacter));
                            }
                        }
                        else
                        {
                            timer += Time.deltaTime;
                        }
                    }
                    else if (obj.TryGetComponent<IStructure>(out var structure))
                    {
                        if (timer >= enemyData.atkspeed)
                        {
                            timer = 0;
                            var dir = obj.transform.position - transform.position;
                            if (dir.x > 0)
                            {
                                _animator.LookRight();
                            }
                            else if (dir.x < 0)
                            {
                                _animator.LookLeft();
                            }
                            _animator.Attack();
                            structure.CurrentHp -= enemyData.atk;
                        }
                        else
                        {
                            timer += Time.deltaTime;
                        }
                    }
                    else
                    {
                        _state = FSMState.Chase;
                    }
                }
                else
                {
                    _state = FSMState.Chase;
                }
            }
            catch { }
        }


        private IEnumerator SlowEffect(ControlableCharacter controlableCharacter)
        {
            float originspeed = controlableCharacter._agentMovement.Speed;
            controlableCharacter._agentMovement.Speed *= slowmultiple;

            yield return new WaitForSeconds(slowTime);

            controlableCharacter._agentMovement.Speed = originspeed;
        }

        private IEnumerator PoisonEffect(ControlableCharacter controlableCharacter)
        {
            for (int i = 0; i < poisoncount; i++)
            {
                yield return new WaitForSeconds(1);

                controlableCharacter.CurrentHP -= poisondamage;
            }
        }
    }
}