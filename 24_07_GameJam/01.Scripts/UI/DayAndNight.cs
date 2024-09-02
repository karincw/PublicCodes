using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _DayText;
    [SerializeField] private TextMeshProUGUI _GoldText;

    public int gold = 0;

    private void Awake()
    {
        gold = GameManager.Instance.gold;
    }

    private void OnDisable()
    {
        GameManager.Instance.gold = gold;
    }

    private void Update()
    {
        _GoldText.text = gold.ToString();
        _DayText.text = (GameManager.Instance.npcCount / 4).ToString();
    }

}
