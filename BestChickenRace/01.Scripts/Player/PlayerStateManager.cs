using Cinemachine;
using Packets;
using System;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public enum PlayerMode
{
    GAME,
    INSTALLATION,
    DEAD
}
public class PlayerStateManager : Player
{
    [SerializeField] private GameObject currentPlayer;
    [SerializeField] public CinemachineVirtualCamera PlayerCam;
    private Rigidbody2D rig2d;

    private PlayerInput _input;
    private PlayerAnimation _animation;
    private PlayerMovement _movement;
    private Map _map;

    public string clickedItemName;
    public bool clicked = false;

    public bool InstallEnd;

    public bool IsFinish = false;
    bool nnnn = false;

    private void Awake()
    {
        rig2d = GetComponent<Rigidbody2D>();
        currentPlayer = gameObject;
        _input = currentPlayer.GetComponent<PlayerInput>();
        _animation = currentPlayer.GetComponent<PlayerAnimation>();
        _movement = currentPlayer.GetComponent<PlayerMovement>();
        PlayerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
        _map = FindObjectOfType<Map>();
    }

    private void Update()
    {
        if (transform.position.y <= -17)
        {
            if (nnnn == false)
            {
                SetDeadMode(OnDead);
                nnnn = true;
            }
        }
    }

    public void OnDead()
    {
        PlayerCam.Priority = 5;
        Player.MODE = PlayerMode.DEAD;
        C_MoveEndedPacket packet = new C_MoveEndedPacket();
        packet.PlayerID = (ushort)GameManager.Instance.playerID;
        packet.MoveEnded = true;
        NetworkManager.Instance.Send(packet);
        rig2d.velocity = Vector2.zero;
    }

    public void StartPlayMode()
    {
        SetGameMode();
        nnnn = false;
        PlayerCam.Priority = 15;
        Player.MODE = PlayerMode.GAME;
        rig2d.velocity = Vector2.zero;
    }

    public void StartInstallMode()
    {
        SetInstallationMode();
        PlayerCam.Priority = 5;
        gameObject.SetActive(true);
        transform.position = _map.playerStartPos;
        Player.MODE = PlayerMode.INSTALLATION;
        rig2d.velocity = Vector2.zero;
    }

    public void Winning()
    {
        _animation.SetWinAnimation();
        IsFinish = true;
    }

}


public class Player : MonoBehaviour
{
    public static PlayerMode MODE = PlayerMode.GAME;

    public void SetGameMode(Action action = null)
    {
        MODE = PlayerMode.GAME;
        action?.Invoke();
    }

    public void SetInstallationMode(Action action = null)
    {
        MODE = PlayerMode.INSTALLATION;
        action?.Invoke();
    }

    public void SetDeadMode(Action action = null)
    {
        MODE = PlayerMode.DEAD;
        action?.Invoke();
    }

    public virtual void GameModePlay(Action action)
    {
        if (MODE == PlayerMode.GAME)
        {
            action.Invoke();
        }
    }

    public virtual void InstallModePlay(Action action)
    {
        if (MODE == PlayerMode.INSTALLATION)
        {
            action.Invoke();
        }
    }

    public virtual void DeadModePlay(Action action)
    {
        if (MODE == PlayerMode.DEAD)
        {
            action.Invoke();
        }
    }
}
