using System;
using System.Collections;
using System.Collections.Generic;
using Tkfkadlsi;
using UnityEngine;

namespace Karin
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private PSpawner[] Spawner;

        [SerializeField] private GameObject[] SpawnEnemy;

        private float setHP = 0;

        public void EnemmySpawn(int Wave)
        {
            int summonValue = 0;
            if (Wave <= 20)
            {
                foreach (var item in Spawner)
                {
                    var sum = SpawnEnemy[0];
                    sum.GetComponent<EnemyAgent>().Atk = 7 + Wave;
                    setHP = 15 + (int)(Wave / 2);
                    item.SummonPrefabs = sum;
                }
                summonValue = (int)(Wave / 4) + 5;
            }
            else if (Wave <= 40)
            {
                foreach (var item in Spawner)
                {
                    var sum = SpawnEnemy[1];
                    sum.GetComponent<EnemyAgent>().Atk = 7 + Wave;
                    setHP = 15 + (int)(Wave / 2);
                    item.SummonPrefabs = sum;
                }
                summonValue = (int)((Wave - 20) / 2) + 5;
            }
            else if (Wave <= 50)
            {
                foreach (var item in Spawner)
                {
                    var sum = SpawnEnemy[2];
                    sum.GetComponent<EnemyAgent>().Atk = 20 + Wave;
                    setHP = 100 + (int)(Wave / 2);
                    item.SummonPrefabs = sum;
                }
                summonValue = (int)((Wave - 40) / 2) + 5;
            }
            else
            {
                Application.Quit();
            }

            foreach (var item in Spawner)
            {
                item.SetHP = this.setHP;
            }

            for (int i = summonValue; i >= 0; i -= 3)
            {
                Spawner[0].Count++;
                Spawner[1].Count++;
                Spawner[2].Count++;
            }

        }

    }
}
