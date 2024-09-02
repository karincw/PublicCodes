using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Karin
{

    public class TableScroller : MonoBehaviour
    {
        [SerializeField] RectTransform _tableRect;
        [SerializeField] RectTransform _CharacterRect;

        [SerializeField] private float _characterScroll;
        [SerializeField] private float _tableScroll;

        private Vector2 _tableOriginPos, _characterOriginPos;

        private void Awake()
        {
            _characterOriginPos = _CharacterRect.anchoredPosition;
            _tableOriginPos = _tableRect.anchoredPosition;
        }

        public void TableScroll(Vector2 Scroll)
        {
            DOTween.Kill(false, 2);

            _tableRect.DOAnchorPos(_tableRect.anchoredPosition + (Scroll.normalized * _tableScroll), 0.25f).SetId(2);
            _CharacterRect.DOAnchorPos(_CharacterRect.anchoredPosition + (Scroll.normalized * _characterScroll), 0.25f).SetId(2);
        }

        public void SetOriginPos()
        {
            _tableRect.DOAnchorPos(_tableOriginPos, 0.25f);
            _CharacterRect.DOAnchorPos(_characterOriginPos, 0.25f);
        }

    }
}