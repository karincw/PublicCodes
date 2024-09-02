using DG.Tweening;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ZoomObj : MonoBehaviour
{
    private Sequence _sequence;
    SpriteRenderer _spriteRenderer;
    InstrumentManager _instrumentManager;
    private List<AddonBtns> _zoomUnderObj = new List<AddonBtns>();
    private AddonBtns _addBtn;
    private AddonBtns _upgradeBtn;
    private Vector3 _originPos;

    private void Start()
    {
        _instrumentManager = GetComponentInParent<InstrumentManager>();
        _spriteRenderer = transform.Find("BG").GetComponent<SpriteRenderer>();
        _zoomUnderObj.AddRange(_spriteRenderer.gameObject.GetComponentsInChildren<AddonBtns>());
        _addBtn = _zoomUnderObj[0];
        _upgradeBtn = _zoomUnderObj[1];
        _originPos = _instrumentManager.transform.position;

        if (_addBtn != null && _upgradeBtn != null)
        {
            _addBtn.gameObject.SetActive(false);
            _upgradeBtn.gameObject.SetActive(false);
            _spriteRenderer.gameObject.SetActive(false);
        }
    }

    public void ZoomIn()
    {
        Transform trm = _instrumentManager.transform;
        _sequence = DOTween.Sequence()
            .Append(trm.DOMove(new Vector3(transform.position.x, 3.5f), 0.5f).SetEase(Ease.Linear))
            //.Join(trm.DOScale(Vector3.one * 1.03f, 0.5f).SetEase(Ease.Linear))
            .AppendCallback(() =>
            {
                _spriteRenderer.gameObject.SetActive(true);
                _addBtn.gameObject.SetActive(true);
                _upgradeBtn.gameObject.SetActive(true);
            })
            .Append(_spriteRenderer.DOFade(0.7f, 0.5f))
            .Join(_addBtn.GetComponent<SpriteRenderer>().DOFade(0.7f, 0.5f))
            .Join(_upgradeBtn.GetComponent<SpriteRenderer>().DOFade(0.7f, 0.5f));
    }
    public void ZoomOut()
    {
        Transform trm = _instrumentManager.transform;
        _sequence = DOTween.Sequence()
            .Append(trm.DOMove(_originPos, 0.5f).SetEase(Ease.Linear))
            //.Join(trm.DOScale(Vector3.one, 0.5f).SetEase(Ease.Linear))
            .Join(_spriteRenderer.DOFade(0f, 0.3f))
            .Join(_addBtn.GetComponent<SpriteRenderer>().DOFade(0f, 0.3f))
            .Join(_upgradeBtn.GetComponent<SpriteRenderer>().DOFade(0f, 0.3f))
            .AppendCallback(() =>
            {
                _spriteRenderer.gameObject.SetActive(false);
                _addBtn.gameObject.SetActive(false);
                _upgradeBtn.gameObject.SetActive(false);
            });
    }
}
