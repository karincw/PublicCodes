using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnotherWorldBtn : MonoBehaviour
{
    [SerializeField] private Button _button;

    private void Awake()
    {
        _button.onClick.AddListener(MoveScene);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(MoveScene);
    }

    private void MoveScene() => GameManager.Instance.CombatScene();

}
