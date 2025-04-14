using UnityEngine;
using DG.Tweening;
using Sequence = DG.Tweening.Sequence;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class InstallationUI : MonoBehaviour
{
    [SerializeField] private Vector3 originPos = new Vector3(1920, 0, 0);
    private Sequence sequence;
    [SerializeField] private List<UnderUI> selects;
    private RectTransform _rectTr;

    private void Awake()
    {
        _rectTr = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        sequence = DOTween.Sequence();
        sequence.
            Append(_rectTr.DOLocalMove(Vector3.zero, 1f)).
            AppendCallback(() =>
            {
                selects.ForEach(t => { t.FadeIn(1f); });
            });
    }

    public void Exit()
    {
        sequence = DOTween.Sequence();
        sequence.
            Append(_rectTr.DOLocalMove(originPos, 1f)).
        AppendCallback(() => { gameObject.SetActive(false); });
    }
}
