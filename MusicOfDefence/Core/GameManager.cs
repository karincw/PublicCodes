using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public Sprite[] noteImage;
    public Sprite[] drumNoteImage;

    [HideInInspector] public GameInput GameInput;
    [HideInInspector] public PoolManager PoolManager;
    [HideInInspector] public FileManager FileManager;
    [HideInInspector] public Health CastleHealth;
    public Transform TowerTrm;

    public bool PlayMode = false;

    private List<InstrumentManager> _instrumentManagers = new();

    public float tickSpeed = 0.3f;

    private void Awake()
    {
        CastleHealth = FindObjectOfType<Health>();
        FileManager = FindObjectOfType<FileManager>();
        PoolManager = FindObjectOfType<PoolManager>();
        GameInput = FindObjectOfType<GameInput>();
        _instrumentManagers = FindObjectsOfType<InstrumentManager>().ToList();
        Application.targetFrameRate = 120;

    }



    public void IsTickEnd()
    {
        bool allEnd = true;
        foreach (var item in _instrumentManagers)
        {
            if (item.NoteEnd == false)
            {
                allEnd = false;
                break;
            }
        }

        if (allEnd == true)
        {
            foreach (var item in _instrumentManagers)
            {
                item.ResetTick();
                if (PlayMode == true)
                {
                    var stageUI = FindObjectOfType<StageUI>();
                    stageUI.Ending(CastleHealth.currentHealth < 0.01f ? "Fail.." : "Clear!!", CastleHealth.currentHealth < 0.01f ? false : true);
                }
                PlayMode = false;
            }
        }
    }


}
