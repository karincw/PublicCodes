using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum BtnState
{
    Basic,
    Clicked,
    BasicActive,
    ClickedActive,
    Return
}

public class Btn : Poolable, IButton
{
    [SerializeField] Sprite[] btnTypes;
    [SerializeField] InstrumentManager _instrumentManager;
    SpriteRenderer _spriteRenderer;
    private SpriteRenderer _noteRenderer;
    private TextMeshPro _tickSpeedView;
    public Opener _opener;

    public BtnState myState;
    public bool _viewToneState = false;
    public bool _viewSpeedState = false;
    public int tone;
    private int particleLen = 7;
    private Vector3 _originSize;
    private float _tickSpeed = 0.2f;

    public float TickSpeed { get { return _tickSpeed; } set { _tickSpeed = value; } }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _instrumentManager = transform.GetComponentInParent<InstrumentManager>();
        _noteRenderer = transform.Find("note").GetComponent<SpriteRenderer>();
        _tickSpeedView = transform.Find("ToneSpeedText")?.GetComponent<TextMeshPro>();
        _originSize = gameObject.transform.localScale;
    }
    private void OnEnable()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _instrumentManager = transform.GetComponentInParent<InstrumentManager>();
        _noteRenderer = transform.Find("note").GetComponent<SpriteRenderer>();
        _tickSpeedView = transform.Find("ToneSpeedText")?.GetComponent<TextMeshPro>();
        _originSize = gameObject.transform.localScale;
        _noteRenderer.gameObject.SetActive(false);
        _tickSpeedView.gameObject.SetActive(false);
        _opener = GetComponentInChildren<Opener>();
    }
    private void Update()
    {
        if (_instrumentManager == null)
            _instrumentManager = transform.GetComponentInParent<InstrumentManager>();

        _tickSpeed = Mathf.Clamp(_tickSpeed, 0.05f, 5);
        _tickSpeed = MathF.Round(_tickSpeed, 3);
        _tickSpeedView.SetText(TickSpeed.ToString());


    }
    private void OnDestroy()
    {
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.GameInput == null) return;
    }

    public void ClickLeftBtn()
    {
        if (myState == BtnState.Basic)
        {
            myState = BtnState.Clicked;
            _spriteRenderer.sprite = btnTypes[1];
        }
        else if (myState == BtnState.Clicked || myState == BtnState.Return)
        {
            myState = BtnState.Basic;
            _spriteRenderer.sprite = btnTypes[0];
            //tone = 0;
            _opener.CloseShell(this);
        }

        particleLen = _instrumentManager.instrument.SoundLength;
    }

    public void ReturnButton()
    {
        if (myState == BtnState.BasicActive && myState == BtnState.ClickedActive)
        {
            return;
        }
        else if (myState == BtnState.Return)
        {
            myState = BtnState.Basic;
            _spriteRenderer.sprite = btnTypes[0];
        }
        else if (myState == BtnState.Basic || myState == BtnState.Clicked)
        {
            myState = BtnState.Return;
            _spriteRenderer.sprite = btnTypes[4];
        }
    }

    public void ImageReRender()
    {
        switch (myState)
        {
            case BtnState.Basic:
                _spriteRenderer.sprite = btnTypes[0];

                break;
            case BtnState.Clicked:
                _spriteRenderer.sprite = btnTypes[1];

                break;
            case BtnState.BasicActive:
                _spriteRenderer.sprite = btnTypes[2];

                break;
            case BtnState.ClickedActive:
                _spriteRenderer.sprite = btnTypes[3];

                break;
            case BtnState.Return:
                _spriteRenderer.sprite = btnTypes[4];

                break;
        }
    }

    public IEnumerator MyTick()
    {
        if (_instrumentManager == null)
            _instrumentManager = transform.GetComponentInParent<InstrumentManager>();
        if (myState == BtnState.Basic)
        {
            myState = BtnState.BasicActive;
            _spriteRenderer.sprite = btnTypes[2];
        }
        else if (myState == BtnState.Clicked)
        {
            myState = BtnState.ClickedActive;
            _spriteRenderer.sprite = btnTypes[3];
            _instrumentManager.attack?.Invoke(tone);

            particleLen = _instrumentManager.instrument.SoundLength;
            if (particleLen == 10)
            {
                GameObject summonNote = GameManager.Instance.PoolManager.Spawn($"DrumNote{(tone + 1)}");
                summonNote.transform.position = transform.position + new Vector3(0, 0.75f);
            }
            else
            {
                GameObject summonNote = GameManager.Instance.PoolManager.Spawn($"BasicNote{(tone + 1)}");
                summonNote.transform.position = transform.position + new Vector3(0, 0.75f);
            }

            transform.DOScale(_originSize * 0.95f, 0.15f).SetEase(Ease.Linear);

        }
        else if (myState == BtnState.Return)
        {
            _instrumentManager.attack?.Invoke(tone);

            particleLen = _instrumentManager.instrument.SoundLength;
            if (particleLen == 10)
            {
                GameObject summonNote = GameManager.Instance.PoolManager.Spawn("DrumNote" + (tone + 1).ToString());
                summonNote.transform.position = transform.position + new Vector3(0, 0.75f);
            }
            else
            {
                GameObject summonNote = GameManager.Instance.PoolManager.Spawn("BasicNote" + (tone + 1).ToString());
                summonNote.transform.position = transform.position + new Vector3(0, 0.75f);
            }


            _instrumentManager.StopCoroutine("StartTick");
            _instrumentManager.StartCoroutine("StartTick");
        }


        yield return new WaitForSeconds(_tickSpeed);


        if (myState == BtnState.BasicActive)
        {
            myState = BtnState.Basic;
            _spriteRenderer.sprite = btnTypes[0];
        }
        else if (myState == BtnState.ClickedActive)
        {
            myState = BtnState.Clicked;
            _spriteRenderer.sprite = btnTypes[1];
        }

        transform.DOScale(_originSize, 0.15f).SetEase(Ease.Linear);
    }

    public void ToneUp()
    {
        if (tone == _instrumentManager.instrument.SoundLength)
        {
            tone = 0;
        }
        else
            tone++;
    }
    public void ToneDown()
    {
        if (tone == 0)
        {
            tone = _instrumentManager.instrument.SoundLength;
        }
        else
            tone--;
    }
    public void SpeedUP()
    {
        _tickSpeed += 0.01f;
    }
    public void SpeedDown()
    {
        _tickSpeed -= 0.01f;
    }

    #region ViewSpeed
    public void ViewSpeed(bool value)
    {
        if (value == true)
        {
            StartCoroutine("Holdviewspeed");
            _viewSpeedState = false;
        }
        else if (value == false)
        {
            StopCoroutine("Holdviewspeed");
            _viewSpeedState = true;
        }
    }

    private IEnumerator Holdviewspeed()
    {
        while (true)
        {
            StopCoroutine("ViewSpeedCorutine");
            StartCoroutine("ViewSpeedCorutine");
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator ViewSpeedCorutine()
    {
        _tickSpeedView.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        _tickSpeedView.gameObject.SetActive(false);
    }
    #endregion

    #region ViewTone
    public void ViewTone(bool value)
    {
        if (value == true)
        {
            StartCoroutine("HoldViewTone");
            _viewToneState = false;
        }
        else if (value == false)
        {
            StopCoroutine("HoldViewTone");
            _viewToneState = true;
        }
    }

    private IEnumerator HoldViewTone()
    {
        while (true)
        {
            StopCoroutine("ViewToneCorutine");
            StartCoroutine("ViewToneCorutine");
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator ViewToneCorutine()
    {
        _noteRenderer.gameObject.SetActive(myState == BtnState.Clicked || myState == BtnState.ClickedActive || myState == BtnState.Return);

        if (_instrumentManager.instrument.SoundLength == 10)
            _noteRenderer.sprite = GameManager.Instance.drumNoteImage[tone];
        else
            _noteRenderer.sprite = GameManager.Instance.noteImage[tone];

        yield return new WaitForSeconds(0.1f);

        _noteRenderer.gameObject.SetActive(false);
    }
    #endregion 

    public void ResetOrigin()
    {
        _originSize = transform.localScale;
        transform.localScale = _originSize;
    }

    public void SettingData(WriteBtnData data)
    {
        this.tone = data.tone;
        this._tickSpeed = data.tickSpeed;
        this.myState = data.state;
    }
}

[System.Serializable]
public class WriteBtnData
{
    public float tickSpeed;
    public int tone;
    public BtnState state;
    public string InstrumentName = "";
    public string name;

    public WriteBtnData(float tickSpeed, int tone, BtnState state, string instrumentName = "", string name = "")
    {
        this.tickSpeed = tickSpeed;
        this.tone = tone;
        this.state = state;
        this.InstrumentName = instrumentName;
        this.name = name;
    }

    public WriteBtnData() { }

}

[System.Serializable]
public class WriteBtnListClass
{
    public List<WriteBtnData> buttons = new List<WriteBtnData>();
}
