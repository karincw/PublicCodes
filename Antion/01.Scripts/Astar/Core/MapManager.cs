using UnityEngine;
using UnityEngine.Tilemaps;

namespace Karin.AStar
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private Tilemap _mainMap;
        public Tilemap collisionMap;
        public Tilemap collisionMap2;


        public static MapManager Instance;

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
                Destroy(Instance);

            _mainMap.CompressBounds();
            collisionMap.CompressBounds();
            collisionMap2.CompressBounds();
        }

        public Vector3Int GetTilePos(Vector3 worldPos)
        {
            if (_mainMap == null) return new Vector3Int();
            //worldPos = new Vector3(worldPos.x - 0.5f, worldPos.y - 0.5f);
            return _mainMap.WorldToCell(worldPos); //원드좌표를 넣으면 해당좌표의 타일맵좌표로 리턴해줌
        }
        public Vector3 GetWorldPos(Vector3Int CellPos)
        {
            if (_mainMap == null) return new Vector3Int();
            return _mainMap.GetCellCenterWorld(CellPos);
        }

        public bool CanMove(Vector3Int tilePos, Vector3Int StartRoutingPos)
        {
            BoundsInt mapBound = _mainMap.cellBounds; //Compress시킨 바운드가 나옴

            if (tilePos.x < mapBound.xMin || tilePos.x > mapBound.xMax || tilePos.y < mapBound.yMin || tilePos.y > mapBound.yMax)
            {
                return false;
            }

            if (StartRoutingPos == tilePos) return true;

            return collisionMap.GetTile(tilePos) is null && collisionMap2.GetTile(tilePos) is null;
            //CollisionMap에 해당타일에 충돌체가 있다면 갈수없다 없다면 갈수있다
        }
    }
}