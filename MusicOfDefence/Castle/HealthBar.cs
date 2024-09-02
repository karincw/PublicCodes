using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    Transform _barTrm;

    private void Awake()
    {
        _barTrm = transform.Find("Bar");
    }
    public void SetBarScale(float normalizedScale)
    {
        Vector3 scale = _barTrm.transform.localScale;
        scale.x = Mathf.Clamp(normalizedScale, 0.0f, 1.0f);
        _barTrm.transform.localScale = scale;
    }
}
