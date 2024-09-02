using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [HideInInspector] public MapManager MapManager;
    [HideInInspector] public TurnManager TurnManager;

    public int PlayerCount = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        MapManager = GetComponent<MapManager>();
        TurnManager = GetComponent<TurnManager>();

        Application.targetFrameRate = 60;
    }
}
