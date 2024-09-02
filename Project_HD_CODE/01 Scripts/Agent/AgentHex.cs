using UnityEngine;

[SelectionBase]
public class AgentHex : Hex
{
    // Grid, Coordinate, HighLight 가지고 있음

    [SerializeField] AgentDataSO _agentData;

    public string characterName => _agentData.characterName;
    public int cost => _agentData.Cost;
    public int sight => _agentData.Sight;
    public int moveRange => _agentData.MoveRange;
    public int atk => _agentData.Atk;
    public int atkRange => _agentData.AtkRange;
    public int _currentHp { get; private set; }
    public int defence => _agentData.Defence;

    Material material = null;
    private Rigidbody _rigidbody;
    private Vector3 direction = Vector3.zero;

    SpriteRenderer _spriteRenderer;
    AgentAttack _agentAttack;
    [HideInInspector] public Team team;

    [SerializeField] private float movementSpeed;
    public bool DidItMove = false;
    public bool DidItAttack = false;

    private void Awake()
    {
        _agentAttack = GetComponentInChildren<AgentAttack>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _hexCoordinates = GetComponent<HexCoordinates>();
        team = GetComponent<Team>();
        _rigidbody = GetComponent<Rigidbody>();
        material = _spriteRenderer.material;
    }

    private void Start()
    {
        _spriteRenderer.sprite = _agentData.Sptires[2];
        _currentHp = _agentData.Hp;
        _spriteRenderer.flipX = true;
        SightDefine();
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = direction;
    }

    public void GlowEnable()
    {
        material.SetInt("_OutLine", 1);

    }
    public void GlowDisable()
    {
        material.SetInt("_OutLine", 0);
    }

    [ContextMenu("SightDefine")]
    public void SightDefine()
    {
        HexGrid.Instance.GetTileAt(HexCoords).DefineTile(sight);
    }

    public void Movement(Vector3 dir)
    {
        direction = dir * movementSpeed;
    }

    public void ChangeSprite(Vector3 dir)
    {
        var z = Mathf.RoundToInt(dir.z);
        if (z > 0)
        {
            _spriteRenderer.sprite = _agentData.Sptires[0];
        }
        else if (z < 0)
        {
            _spriteRenderer.sprite = _agentData.Sptires[2];
        }
        else
        {
            _spriteRenderer.sprite = _agentData.Sptires[1];
        }

        if (dir.x > 0)
        {
            _spriteRenderer.flipX = true;
        }
        else if (dir.x < 0)
        {
            _spriteRenderer.flipX = false;
        }
    }

    public void Attack(Vector3Int attackPosition, AgentHex hitagent)
    {
        var attackPos = HexCoordinates.ConvertOffsetToPosition(attackPosition);
        _agentAttack.AttackFeedback(attackPos, hitagent);
        ChangeSprite(attackPos - transform.position);
    }

    public void Hit(int atk)
    {
        _currentHp -= (atk - defence);
        if (IsDead() == false)
        {
            _agentAttack.HitFeedBack();
        }
        else
        {
            Die();
        }
    }

    private bool IsDead()
    {
        if (_currentHp <= 0)
        {
            return true;
        }
        return false;
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public void TurnReset()
    {
        DidItMove = false;
        DidItAttack = false;
    }
}
