using Cinemachine;
using Karin.Network;
using System.Collections;
using UnityEngine;
using Packets;
using UnityEngine.Events;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : Player
{
    [Header("속도가 변할때")]
    public UnityEvent<float> OnVelocityChanged;
    public UnityEvent<bool> OnJumped;
    public UnityEvent<bool> OnWallLanding;
    public UnityEvent<bool> OnViewLeft;
    public UnityEvent<bool> OnFalling;

    private Collider2D _col;
    private Rigidbody2D _rig2d;
    private PlayerStateManager _playerStateManager;

    private Vector2 _moveDirection;
    [SerializeField] private float _landingDirX;

    private float ocha = 0.05f;
    private Vector2 ochavec = Vector2.zero;
    private float delay = 0.01f;

    [Header("Movement")]
    [SerializeField] private float _currentSpeed = 0f;
    [SerializeField] private float _accelSpeed = 5f;
    [SerializeField] private float _maxSpeed = 4f;
    [SerializeField] private float _AddSpeed = 3f;

    [Header("Jump")]
    [SerializeField] private float _JumpPower = 0f;
    [SerializeField] private bool _canJumping = true;
    [SerializeField] private LayerMask mapLayer;

    [Header("WallJump")]
    [SerializeField] private bool _landing = false;
    [SerializeField] private float _wallDownSpeed;

    public Vector2 Velocity
    {
        get
        {
            if (_currentSpeed <= 0.3f)
            {
                return _rig2d.velocity;
            }
            else
            {
                return new Vector2(_moveDirection.x * _currentSpeed, _rig2d.velocity.y);
            }
        }
        set { }
    }

    private void Awake()
    {
        _rig2d = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();
        _playerStateManager = GetComponent<PlayerStateManager>();

    }
    private void Start()
    {
        StartCoroutine(MovePacketSender());
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        _rig2d.velocity = Velocity;
        OnVelocityChanged?.Invoke(_currentSpeed);
        OnJumped?.Invoke(!_canJumping);
        OnWallLanding?.Invoke(_landing);
        if (_rig2d.velocity.y < 0)
        {
            OnFalling?.Invoke(true);
        }
        else
        {
            OnFalling?.Invoke(false);
        }

        MapCheck();

    }

    public void SendPos(Packet packet)
    {
        NetworkManager.Instance.Send(packet);
    }

    public void Movement(Vector2 direction)
    {
        if (_landing == true)
        {
            return;
        }
        if (direction.sqrMagnitude > 0)
        {
            if (Vector2.Dot(_moveDirection, direction) < 0)
            {
                _currentSpeed = 0;
            }
            _moveDirection = direction;
        }
        if (direction.x < 0)
        {
            OnViewLeft?.Invoke(true);
        }
        else if (direction.x > 0)
        {
            OnViewLeft?.Invoke(false);

        }
        _currentSpeed = CalculateSpeed(direction, true);

        GameModePlay(() =>
        {
            ocha = Vector2.Distance(transform.position, ochavec);
            if(ocha >= 0.05f)
            {
                C_MovePacket movePacket = new C_MovePacket();
                movePacket.playerData = new PlayerPacket();
                movePacket.playerData.PlayerID = (ushort)GameManager.Instance.playerID;
                movePacket.playerData.X = transform.position.x;
                movePacket.playerData.Y = transform.position.y;
                NetworkManager.Instance.Send(movePacket);
                ochavec = transform.position;
            }
        });
    }

    private IEnumerator MovePacketSender()
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);

            C_MovePacket movePacket = new C_MovePacket();
            movePacket.playerData = new PlayerPacket();
            movePacket.playerData.PlayerID = (ushort)GameManager.Instance.playerID;
            movePacket.playerData.X = transform.position.x;
            movePacket.playerData.Y = transform.position.y;
            NetworkManager.Instance.Send(movePacket);
            ochavec = transform.position;

        }
    }

    private float CalculateSpeed(Vector2 direction, bool useDecreaseSpeed = false, bool useIncreaseSpeed = false)
    {
        if (direction.sqrMagnitude > 0)
        {
            if (useIncreaseSpeed)
            {
                _currentSpeed += _accelSpeed * Time.fixedDeltaTime * _AddSpeed;
            }
            else
            {
                _currentSpeed += _accelSpeed * Time.fixedDeltaTime;
            }
        }
        else
        {
            if (useDecreaseSpeed)
            {
                _currentSpeed -= _accelSpeed * Time.fixedDeltaTime * _AddSpeed;
            }
            else
            {
                _currentSpeed -= _accelSpeed * Time.fixedDeltaTime;
            }
        }

        return Mathf.Clamp(_currentSpeed, 0, _maxSpeed);
    }

    public void Jump()
    {
        if (_landing)
        {
            _rig2d.velocity = Vector2.zero;
            _rig2d.AddForce(new Vector2(_landingDirX * _JumpPower / 2, _JumpPower), ForceMode2D.Impulse);
            _canJumping = false;
            _landing = false;
        }
        else if (_canJumping)
        {
            _rig2d.velocity = Vector2.zero;
            _rig2d.AddForce(new Vector2(0, _JumpPower), ForceMode2D.Impulse);
            _canJumping = false;
            _landing = false;
        }
    }

    public void WallLanding(bool value)
    {
        if (value == true && _landing == true)
        {
            _rig2d.MovePosition(new Vector2(_rig2d.position.x, _rig2d.position.y - (_wallDownSpeed * Time.fixedDeltaTime)));
            _currentSpeed = 0;
        }
    }

    private void MapCheck()
    {
        RaycastHit2D floorRay = Physics2D.BoxCast(_col.bounds.center, _col.bounds.size, 0f, Vector3.down, 0.1f, mapLayer);
        if (floorRay.collider != null)
        {
            _landing = false;
            _canJumping = true;
            return;
        }
        RaycastHit2D rightWallRay = Physics2D.BoxCast(_col.bounds.center, _col.bounds.size, 0f, Vector3.right, 0.05f, mapLayer);
        RaycastHit2D leftWallRay = Physics2D.BoxCast(_col.bounds.center, _col.bounds.size, 0f, Vector3.left, 0.05f, mapLayer);
        if (rightWallRay.collider != null)
        {
            _landingDirX = -1;
            _landing = true;
            _canJumping = false;
            return;
        }
        else if (leftWallRay.collider != null)
        {
            _landingDirX = 1;
            _landing = true;
            _canJumping = false;
            return;
        }
        _landing = false;
        _canJumping = false;
        return;
    }


}



