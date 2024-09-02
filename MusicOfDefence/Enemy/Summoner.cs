using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Summoner : MonoBehaviour
{
    Transform[] spawnPoints;
    [SerializeField] private string[] enemysName;

    private void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
        // Index 0 번은 사용하지않음* 0은 자기자신을 가르킴
    }

    public GameObject Summon(int idx)
    {
        Vector2 spawnPoint = spawnPoints[idx].position;
        string summonEnemyName = enemysName[idx];

        GameObject enemy = GameManager.Instance.PoolManager.Spawn(summonEnemyName);
        enemy.transform.position = spawnPoint;

        return enemy;
    }

    private void Update()
    {

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Summon(1);
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            Summon(2);
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            Summon(3);
        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            Summon(4);
        }
        else if (Input.GetKeyDown(KeyCode.F5))
        {
            Summon(5);
        }
        else if (Input.GetKeyDown(KeyCode.F6))
        {
            Summon(6);
        }
        else if (Input.GetKeyDown(KeyCode.F7))
        {
            Summon(7);
        }
#endif

    }

    public async void Play(WriteBtnListClass data)
    {
        Debug.Log(data);
        if (data == null) return;
        List<WriteBtnData> btns = new List<WriteBtnData>();

        btns.AddRange(data.buttons);

        btns = btns.OrderBy(t => t.name).ToList();

        string current = "";
        foreach (var item in btns)
        {
            if (current != item.name)
            {
                current = item.name;
                await Task.Delay(200);
            }
            if (item.state == BtnState.Clicked)
            {
                int idx = Int32.Parse(item.InstrumentName[1].ToString());
                Summon(idx);
            }

        }

    }

}
