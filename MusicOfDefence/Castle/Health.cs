using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int currentHealth { private set; get; }
    [SerializeField] private int _maxHealth;

    public UnityEvent<float> OnChangeHealth;
    private void Start()
    {
        currentHealth = _maxHealth;
        OnChangeHealth?.Invoke((float)currentHealth / _maxHealth);
    }

    private void Update()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, _maxHealth);
        if (currentHealth < 0.01f)
        {
            Debug.Log("GameOver");
        }
    }

    public void DecreaseHealth(int amount)
    {
        if (GameManager.Instance.PlayMode == false) return;
        currentHealth -= amount;
        OnChangeHealth?.Invoke((float)currentHealth / _maxHealth);
    }

    public void IncreaseHealth(int amount)
    {
        if (GameManager.Instance.PlayMode == false) return;
        currentHealth += amount;
        OnChangeHealth?.Invoke((float)currentHealth / _maxHealth);
    }
}
