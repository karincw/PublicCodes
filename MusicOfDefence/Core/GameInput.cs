using DG.Tweening;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public UnityEvent MouseLClick;

    [SerializeField] private LayerMask findObjLayer;

    public bool ToneToogle = false;
    public bool SpeedToogle = false;
    public bool ReturnToogle = false;

    [SerializeField] private GameObject moveObject;

    bool isHold;
    Vector3 startVector;
    Vector3 changeVector;
    Vector3 moveobjPos;

    Camera main;

    public PlayerInputAction playerInputAction { get; private set; }

    private void Awake()
    {
        ReturnToogle = false;
        main = Camera.main;
        moveobjPos = moveObject.transform.position;
        playerInputAction = new PlayerInputAction();
        GameInputEnable();
        playerInputAction.PlayerAction.MouseLeft.performed += OnMouseLeft;

        playerInputAction.PlayerAction.MapScroll.performed += MapScroll_performed;
        playerInputAction.PlayerAction.MapScroll.canceled += MapScroll_canceled;
    }

    private void Update()
    {
        if (isHold == true)
        {
            changeVector = startVector - main.ScreenToWorldPoint(Input.mousePosition);

            moveObject.transform.position += changeVector;
            moveObject.transform.position = new Vector3(moveObject.transform.position.x, 0, -10);
        }
    }

    private void OnDestroy()
    {
        playerInputAction.PlayerAction.MouseLeft.performed -= OnMouseLeft;

        playerInputAction.PlayerAction.MapScroll.started += MapScroll_performed;
        playerInputAction.PlayerAction.MapScroll.canceled += MapScroll_canceled;
    }

    public void MapScrollEvent(bool value)
    {
        if (value == true)
            playerInputAction.PlayerAction.MapScroll.Enable();
        else if (value == false)
            playerInputAction.PlayerAction.MapScroll.Disable();
    }

    private void MapScroll_performed(InputAction.CallbackContext obj)
    {
        startVector = main.ScreenToWorldPoint(Input.mousePosition);
        isHold = true;
    }

    private void MapScroll_canceled(InputAction.CallbackContext obj)
    {
        isHold = false;
    }

    public void GameInputEnable()
    {
        playerInputAction.PlayerAction.Enable();
        playerInputAction.UI.Disable();
        playerInputAction.Tutorial.Disable();
    }

    public void UIInputEnable()
    {
        playerInputAction.UI.Enable();
        playerInputAction.PlayerAction.Disable();
        playerInputAction.Tutorial.Disable();
    }
    public void TutorialInputEnable()
    {
        playerInputAction.Tutorial.Enable();
        playerInputAction.PlayerAction.Disable();
        playerInputAction.UI.Disable();
    }

    public void OnMouseLeft(InputAction.CallbackContext callback)
    {
        if (ReturnToogle == true)
        {
            if (FindObject()?.TryGetComponent<Btn>(out Btn btn) == true)
            {
                btn.ReturnButton();
            }
        }
        else if (ToneToogle == true)
        {
            GameObject obj = FindObject();
            if (obj?.TryGetComponent<ChangeBtn>(out ChangeBtn changeBtn) == true)
            {
                changeBtn.OnClick();
                return;
            }
            Opener opener = obj?.GetComponentInChildren<Opener>();
            if (opener != null)
            {
                var btn = obj.GetComponent<Btn>();
                if (btn == null)
                {
                    var insBtn = obj.GetComponent<InstrumentBtn>();
                    opener.ControlChell(insBtn, OpenType.Tone);
                }
                else if (btn.myState == BtnState.BasicActive || btn.myState == BtnState.Basic)
                {
                    return;
                }
                opener.ControlChell(btn, OpenType.Tone);
            }
        }
        else if (SpeedToogle == true)
        {
            GameObject obj = FindObject();
            if (obj?.TryGetComponent<ChangeBtn>(out ChangeBtn changeBtn) == true)
            {
                changeBtn.OnClick();
                return;
            }
            Opener opener = obj?.GetComponentInChildren<Opener>();
            if (opener != null)
            {
                var btn = obj.GetComponent<Btn>();
                if (btn == null)
                {
                    var insBtn = obj.GetComponent<InstrumentBtn>();
                    opener.ControlChell(insBtn, OpenType.Speed);
                }
                else if (btn.myState == BtnState.BasicActive || btn.myState == BtnState.Basic)
                {
                    return;
                }
                opener.ControlChell(btn, OpenType.Speed);
            }
        }
        else
        {
            MouseLClick?.Invoke();
        }
    }

    private GameObject FindObject()
    {
        var pos = main.ScreenToWorldPoint(Input.mousePosition);
        //RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f, findObjLayer);
        Collider2D hitcol = Physics2D.OverlapPoint(pos);
        if (hitcol == null)
        {
            return null;
        }
        GameObject obj = hitcol.transform.gameObject;

        return obj;
    }

    public void BtnClick()
    {
        // AddBtn >> ChangeBtn >> InstrumentBtn >> Btn
        if (FindObject()?.TryGetComponent<AddonBtns>(out AddonBtns addbtn) == true)
        {
            addbtn.OnClick();
        }
        else if (FindObject()?.TryGetComponent<ChangeBtn>(out ChangeBtn changeBtn) == true)
        {
            changeBtn.OnClick();
        }
        else if (FindObject()?.TryGetComponent<InstrumentBtn>(out InstrumentBtn insbtn) == true)
        {
            insbtn.Zoom();
        }
        else if (FindObject()?.TryGetComponent<Btn>(out Btn btn) == true && (ToneToogle != true || SpeedToogle != true))
        {
            btn.ClickLeftBtn();
        }
    }
}
