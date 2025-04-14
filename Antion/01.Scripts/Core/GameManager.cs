using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

namespace Karin
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public TileBase EnemyTile;
        public TileBase PlayerTile;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
                Destroy(this);
        }


    }
}
