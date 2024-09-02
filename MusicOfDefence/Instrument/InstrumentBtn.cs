using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

public class InstrumentBtn : MonoBehaviour, IButton
{
    InstrumentManager _instrumentManager;
    SortingGroup _sortingGroup;
    List<InstrumentBtn> _instrumetBtns = new List<InstrumentBtn>();
    public bool zoomState = false;
    ZoomObj _zoomObj;
    Sorting _sorting;
    List<Btn> btns = new List<Btn>();

    private void Awake()
    {
        _instrumentManager = GetComponentInParent<InstrumentManager>();
        _sortingGroup = _instrumentManager.GetComponent<SortingGroup>();
        _zoomObj = _instrumentManager.GetComponentInChildren<ZoomObj>();
        _sorting = GetComponentInParent<Sorting>();
        ReFindBtns();
    }
    private void Start()
    {
        _sortingGroup.sortingOrder = 5;
    }
    public void ResetTick()
    {
        _instrumentManager.ResetTick();
    }

    private void ZoomIn()
    {
        if (_sorting == null) _sorting = GetComponentInParent<Sorting>();
        _sortingGroup.sortingOrder = 10;
        zoomState = true;
        _zoomObj.ZoomIn();
        _sorting.Dimension2Sort();
    }
    private void ZoomOut()
    {
        if (_sorting == null) _sorting = GetComponentInParent<Sorting>();
        _sortingGroup.sortingOrder = 5;
        zoomState = false;
        _zoomObj.ZoomOut();
        _sorting.HorizontalSort();
    }
    public void Zoom()
    {
        if (OtherZoomIn() == false)
        {
            ZoomIn();
        }
        else
        {
            ZoomOut();
            ReFindBtns();
        }
    }
    private bool OtherZoomIn()
    {
        //다른 얘들중 줌된얘가 있다면 True를 없다면 False를 반환
        if (_instrumetBtns.Count == 0)
        {
            _instrumetBtns.AddRange(FindObjectsOfType<InstrumentBtn>());
        }
        for (int i = 0; i < _instrumetBtns.Count; i++)
        {
            if (_instrumetBtns[i].zoomState == true)
            {
                return true;
            }
        }
        return false;
    }

    public void ReFindBtns()
    {
        btns.Clear();
        btns.AddRange(gameObject.GetComponentsInChildren<Btn>());
    }

    public void ToneUp()
    {
        foreach (var btn in btns)
        {
            btn.ToneUp();
        }
    }
    public void ToneDown()
    {
        foreach (var btn in btns)
        {
            btn.ToneDown();
        }
    }
    public void SpeedUP()
    {
        foreach (var btn in btns)
        {
            btn.SpeedUP();
        }
    }
    public void SpeedDown()
    {
        foreach (var btn in btns)
        {
            btn.SpeedDown();
        }
    }
    public void ViewTone(bool value)
    {
        foreach (var btn in btns)
        {
            btn.ViewTone(value);
            if (btn._opener.IsOpen)
            {
                btn.ViewTone(true);
            }
        }
    }
    public void ViewSpeed(bool value)
    {
        foreach (var btn in btns)
        {
            btn.ViewSpeed(value);
            if (btn._opener.IsOpen)
            {
                btn.ViewTone(true);
            }
        }
    }

}
