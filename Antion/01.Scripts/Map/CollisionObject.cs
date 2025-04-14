using Karin.AStar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Karin
{
    public class CollisionObject : MonoBehaviour
    {
        [SerializeField] private TileBase tile;

        [SerializeField] private bool Hitable = false;

        void Start()
        {
            SummonTile();
        }

        public void SummonTile()
        {
            var currentPos = MapManager.Instance.GetTilePos(transform.position);
            transform.position = MapManager.Instance.GetWorldPos(currentPos);

            MapManager.Instance.collisionMap2.SetTile(currentPos, tile);
            if (Hitable) { MapManager.Instance.collisionMap.SetTile(currentPos, GameManager.Instance.PlayerTile); }
        }

        private void OnDestroy()
        {
            try
            {

                var currentPos = MapManager.Instance.GetTilePos(transform.position);
                transform.position = MapManager.Instance.GetWorldPos(currentPos);

                if (Hitable) { MapManager.Instance.collisionMap.SetTile(currentPos, null); }
                MapManager.Instance.collisionMap2.SetTile(currentPos, null);
            }
            catch { }
        }
    }
}