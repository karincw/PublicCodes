using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Karin
{
    public class Gauge : MonoBehaviour
    {
        [SerializeField] GameObject Bar;
        [SerializeField] private float StartGauge;
        [SerializeField] private float currentGuage;

        public event Action GaugeEnd;

        private void Awake()
        {
            currentGuage = StartGauge;
        }

        private void OnEnable()
        {
            GaugeEnd += GaugeReset;
        }

        private void OnDisable()
        {
            GaugeEnd -= GaugeReset;
        }

        private void Update()
        {
            Bar.transform.localScale = new Vector3(currentGuage / StartGauge, 0.9f, 1);
        }

        public void GaugeDecrease(float value)
        {
            currentGuage -= value;
            currentGuage = Mathf.Clamp(currentGuage, 0, StartGauge);
            if(currentGuage <= 0)
            {
                GaugeEnd?.Invoke();
            }
        }
        public void GaugeIncrease(float value)
        {
            currentGuage += value;
            currentGuage = Mathf.Clamp(currentGuage, 0, StartGauge);
        }

        private void GaugeReset()
        {
            currentGuage = StartGauge;
        }

    }
}