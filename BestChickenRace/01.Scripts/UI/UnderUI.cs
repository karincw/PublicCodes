using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UnderUI : MonoBehaviour
{
    [SerializeField] Color originColor;
    Image image;
    public Action FadeInCallBack;

    private void Awake()
    {
        image = GetComponent<Image>();
        originColor = image.color;
        image.color = new Color32(0, 0, 0, 0);
    }

    public void FadeIn(float time)
    {
        image.DOColor(originColor, time);
        Invoke("CallBack", time);
    }

    private void CallBack()
    {
        FadeInCallBack?.Invoke();
    }

}
