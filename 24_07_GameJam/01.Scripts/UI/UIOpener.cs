using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Karin
{

    public class UIOpener : MonoBehaviour
    {
        [SerializeField] private Position AnimPos;
        [SerializeField] private Button _openButton;
        [SerializeField] private RectTransform _uI;
        [SerializeField] private AnimationCurve _animCurve;
        [SerializeField] private TableScroller _tableScroller;

        private Vector2 _startPos, _movePos;
        private CanvasGroup _canvasGroup;
        bool _isOpen = false;


        private void Awake()
        {
            _canvasGroup = _uI.GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _startPos = _uI.anchoredPosition;
            switch (AnimPos)
            {
                case Position.Left:
                    _movePos = new Vector2(-1920, 0);
                    break;
                case Position.Right:
                    _movePos = new Vector2(1920, 0);
                    break;
                case Position.Top:
                    _movePos = new Vector2(0, 1080);
                    break;
                case Position.Bottom:
                    _movePos = new Vector2(0, -1080);
                    break;
            }

            _uI.anchoredPosition += _movePos;

            _openButton.onClick.AddListener(ActiveSwitch);
        }

        public void ActiveSwitch()
        {
            if (_isOpen)
            {
                CloseUI();
                _tableScroller?.TableScroll(_movePos);
                _isOpen = false;
            }
            else
            {
                OpenUI();
                _tableScroller?.TableScroll(-_movePos);
                _isOpen = true;
            }
        }
        public void CloseAll()
        {
            if (_isOpen == false) return;

            CloseUI();
            _tableScroller?.TableScroll(_movePos);
            _isOpen = false;
        }

        private void OpenUI()
        {
            UIOpenManager.Instance.OpenDefencer(this, AnimPos);

            Sequence seq = DOTween.Sequence()
                .Append(_uI.DOAnchorPos(_startPos, 0.25f).SetEase(_animCurve))
                .Join(_canvasGroup.DOFade(1f, .25f))
                .AppendCallback(() =>
                {
                    _canvasGroup.interactable = true;
                });
        }

        private void CloseUI()
        {
            UIOpenManager.Instance.ListSetup(AnimPos);
            Sequence seq = DOTween.Sequence()
                .Append(_uI.DOAnchorPos(_movePos, 0.25f))
                .Join(_canvasGroup.DOFade(0f, 0.25f))
                .AppendCallback(() =>
                {
                    _canvasGroup.interactable = false;
                });
        }

        public void BtnInteract(bool state)
        {
            _openButton.interactable = state;
        }

    }

    public enum Position
    {
        Left, Right, Top, Bottom
    }
}