using DG.Tweening;
using karin;
using Karin;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoSingleton<NPC>
{
    [SerializeField] private Image _timer;
    [SerializeField] private Image _characterImage;
    [SerializeField] private float _settingTime;
    private float _currentTime = 0, _time;
    public bool existNpc = false;

    [SerializeField] private float _NPCStartPos;
    [SerializeField] NPCDataListSO _data;
    NPCdataSO _currentData;
    [SerializeField] NPCdataSO _lastData;
    [SerializeField] DayAndNight _topBar;
    [SerializeField] Button _anotherWorldBtn;

    private RectTransform _rectTransform => (transform as RectTransform);

    private float _resultWeight = 0;

    public void ResultGold(int foodPrice)
    {
        _topBar.gold += Mathf.FloorToInt(foodPrice * _condition * (_resultWeight / 7 < 0.1f ? 0.1f : _resultWeight / 7));
        ExitNPC();
    }
    private float _goodCondition = 0.75f, _sosoCondition = .25f, _badCondition = 0f;
    private float _condition;

    private int _npcCount = 0;

    private void Start()
    {
        existNpc = true;
        UIOpenManager.Instance.DefenceAll();
        if (GameManager.Instance.npcdata != null)
        {
            _currentData = GameManager.Instance.npcdata;
            _currentTime = GameManager.Instance.time;
            _time = GameManager.Instance.maxTime;

            _rectTransform.anchoredPosition = new Vector2(0, _rectTransform.anchoredPosition.y);
            _characterImage.sprite = _currentData.characterImage;
            existNpc = true;
            DialogSystem.Instance.SetText(_currentData.text.Split('>')[1]);
        }
        else
        {
            _rectTransform.anchoredPosition = new Vector2(2000, _rectTransform.anchoredPosition.y);
        }
    }

    private void OnDisable()
    {
        GameManager.Instance.npcdata = _currentData;
        GameManager.Instance.npcCount = _npcCount;
        GameManager.Instance.maxTime = _settingTime;
        GameManager.Instance.time = _currentTime;
        GameManager.Instance.text = _currentData.text;
    }

    private void Update()
    {
        _anotherWorldBtn.interactable = existNpc;
        if (existNpc)
        {
            _currentTime -= Time.deltaTime;

            if (_currentTime > 0)
            {
                _timer.fillAmount = _currentTime / _time;
                if (_timer.fillAmount > _goodCondition)
                {
                    _timer.color = new Color(31, 237, 106, 255);
                    _condition = 1.3f;
                }
                else if (_timer.fillAmount > _sosoCondition)
                {
                    _timer.color = new Color(247, 202, 40, 255);
                    _condition = 1f;
                }
                else if (_timer.fillAmount > _badCondition)
                {
                    _timer.color = new Color(245, 67, 54, 255);
                    _condition = 0.7f;
                }
            }
            else
            {
                _timer.fillAmount = 0;
                _condition = 0;
                ExitNPC();
            }

        }
    }

    [ContextMenu("Enter")]
    public void NextNPC()
    {
        if (existNpc) return;
        _timer.fillAmount = 1;
        DialogSystem.Instance.SetText("");
        UIOpenManager.Instance.DefenceAll();
        ++_npcCount;

        if (GameManager.Instance.npcCount == 13)
        {
            _currentData = _lastData;
        }
        else
        {
            _currentData = _data.GetRandomData();
        }
        _rectTransform.anchoredPosition = new Vector2(_NPCStartPos, _rectTransform.anchoredPosition.y);
        _characterImage.sprite = _currentData.characterImage;

        _rectTransform.DOAnchorPosX(0, 2f);
        Invoke("StartSpeak", 2f);
    }

    private void StartSpeak()
    {
        DialogSystem.Instance.TextInit(_currentData.text);
        DialogSystem.Instance.TextSpawn(() =>
        {
            existNpc = true;
            StartTimer();
        });
    }

    public void FoodEvaluation(FoodSO food)
    {
        _resultWeight = 0;
        _resultWeight += _currentData.weight.meet * food.weight.meet;
        _resultWeight += _currentData.weight.vegetable * food.weight.vegetable;
        _resultWeight += _currentData.weight.hot * food.weight.hot;
        _resultWeight += _currentData.weight.ice * food.weight.ice;
        _resultWeight += _currentData.weight.soft * food.weight.soft;
        _resultWeight += _currentData.weight.hard * food.weight.hard;
        _resultWeight += _currentData.weight.spice * food.weight.spice;
    }

    [ContextMenu("Exit")]
    public void ExitNPC()
    {
        if (!existNpc) return;
        _rectTransform.DOAnchorPosX(-_NPCStartPos, 2f);
        existNpc = false;
        _currentData = null;
        _npcCount = GameManager.Instance.npcCount;
    }

    private void StartTimer()
    {
        _currentTime = _time = _settingTime;

        UIOpenManager.Instance.DefenceOpenAll();
    }
}
