using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum OpenType
{
    Tone,
    Speed,
    None
}
public class Opener : MonoBehaviour
{
    [SerializeField] ChangeBtn upShell;
    [SerializeField] ChangeBtn downShell;

    Collider2D upShellCol;
    Collider2D downShellCol;

    SortingGroup _sortingGroup;

    Sequence seq;

    public bool IsOpen { get; private set; }
    private bool moveEnd;

    private void Awake()
    {
        _sortingGroup = GetComponent<SortingGroup>();
        upShellCol = upShell.GetComponent<Collider2D>();
        downShellCol = downShell.GetComponent<Collider2D>();
    }
    private void Start()
    {
        moveEnd = true;
        IsOpen = false;
    }

    [ContextMenu("CtrlShell")]
    public void ControlChell(IButton btn, OpenType openType)
    {
        if (moveEnd == true)
        {
            if (IsOpen == true)
            {
                CloseShell(btn);
            }
            else
            {
                OpenShell(btn, openType);
            }
        }
    }

    private void OpenShell(IButton btn, OpenType openType)
    {
        seq = DOTween.Sequence();
        upShell.gameObject.SetActive(true);
        downShell.gameObject.SetActive(true);
        upShellCol.enabled = true;
        downShellCol.enabled = true;
        _sortingGroup.sortingOrder = 9;


        upShell.openType = openType;
        downShell.openType = openType;

        moveEnd = false;
        seq.Append(upShell.transform.DOLocalMove(new Vector3(0, 0.5f, 0), 0.7f))
           .Join(downShell.transform.DOLocalMove(new Vector3(0, -0.5f, 0), 0.7f))
           .AppendCallback(() =>
           {
               if (openType == OpenType.Tone)
               {
                   btn.ViewTone(true);
               }
               else if (openType == OpenType.Speed)
               {
                   btn.ViewSpeed(true);
               }
               moveEnd = true;
               IsOpen = true;
           });


    }

    public void CloseShell(IButton btn)
    {
        seq = DOTween.Sequence();

        _sortingGroup.sortingOrder = 0;
        moveEnd = false;

        upShellCol.enabled = false;
        downShellCol.enabled = false;

        seq.Append(upShell.transform.DOLocalMove(new Vector3(0, 0.22f, 0), 0.7f))
            .Join(downShell.transform.DOLocalMove(new Vector3(0, -0.22f, 0), 0.7f))
            .AppendCallback(() =>
            {
                upShell.gameObject.SetActive(false);
                downShell.gameObject.SetActive(false);
                moveEnd = true;
                IsOpen = false;

                btn?.ViewTone(false);
                btn?.ViewSpeed(false);

            });
    }
}
