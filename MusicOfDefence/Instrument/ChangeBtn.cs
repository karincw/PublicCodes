using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBtn : MonoBehaviour
{
    public enum UpDown
    {
        Up,
        Down
    }

    public OpenType openType;
    public UpDown type;
    IButton btn;

    private void Awake()
    {
        btn = GetComponentInParent<Btn>();
    }
    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void OnClick(IButton btn = null)
    {
        if (btn == null)
        {
            btn = this.btn;
            if (btn == null)
            {
                btn = GetComponentInParent<InstrumentBtn>();
            }
        }
        if (type == UpDown.Up)
        {
            if (openType == OpenType.Tone)
            {
                btn.ToneUp();
            }
            else if (openType == OpenType.Speed)
            {
                btn.SpeedUP();
            }
        }
        else
        {
            if (openType == OpenType.Tone)
            {
                btn.ToneDown();
            }
            else
            {
                btn.SpeedDown();
            }
        }
    }

}
