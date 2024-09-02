using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialItem : MonoBehaviour, IPointerClickHandler
{

    RectTransform rectTrm;

    private void Awake()
    {
        rectTrm = transform as RectTransform;    
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var center = Camera.main.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));

        rectTrm.DOAnchorPos(center, 2f);
    }
}
