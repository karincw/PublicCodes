using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstallItemUis : MonoBehaviour
{
    RectTransform rect;
    UnderUI parent;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        parent = GetComponentInParent<UnderUI>();
        parent.FadeInCallBack += Enable;
    }
    private void OnDestroy()
    {
        parent.FadeInCallBack -= Enable;
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }
    private void OnEnable()
    {
        rect.localPosition = new Vector3(Random.Range(-345, 345), Random.Range(-195, 195), 0);
    }
}
