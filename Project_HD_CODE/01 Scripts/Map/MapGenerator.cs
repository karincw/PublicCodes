using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("기본 제작관련 값")]
    [SerializeField] private int _mapWidth = 30;
    [SerializeField] private int _mapHeight = 30;
    [SerializeField] private float _tileSize = 1;

    [Header("랜덤맵 노이즈관련 값")]
    [SerializeField] private int _noiseSeed = -1;
    [SerializeField] private float _noiseFrequency = 10f;
    [SerializeField] private float _waterThreshold = 0.4f;
    [SerializeField] private float _islandThreshold = 0.1f;
    [SerializeField] private float _forestThreshold = 0.7f;
    [SerializeField] private float _hillThreshold = 0.8f;
    [SerializeField] private float _highHillThreshold = 0.88f;
    [SerializeField] private float _randomTileThreshold = 0.5f;
    [SerializeField] private int _woodRandomValue = 15;
    [SerializeField] private int _hillRandomValue = 5;
    [SerializeField] private int _waterTreasuteValue = 1;

    private void Awake()
    {
        MakeGridTile();
    }

    #region MapGenerte

    private Vector2 GetHexCoords(int x, int z)
    {
        float xPos = z * _tileSize + ((x % 2 == 1) ? _tileSize * 0.5f : 0);
        float zPos = x * _tileSize * 0.86f;

        return new Vector2(xPos, zPos);
    }

    private void MakeGridTile()
    {
        for (int x = 0; x < _mapWidth; x++)
        {
            for (int z = 0; z < _mapHeight; z++)
            {
                Vector2 hexCoords = GetHexCoords(x, z);
                Vector3 pos = new Vector3(hexCoords.x, 0, hexCoords.y);

                if (_noiseSeed == -1)
                {
                    _noiseSeed = Random.Range(0, 10000000);
                }

                float noiseValue = Mathf.PerlinNoise((hexCoords.x + _noiseSeed) / _noiseFrequency, (hexCoords.y + _noiseSeed) / _noiseFrequency);

                string mapID = "Grass";

                if (noiseValue < _waterThreshold) mapID = "Water";
                if (noiseValue < _islandThreshold) mapID = "Grass";
                if (noiseValue > _forestThreshold) mapID = "Forest";
                if (noiseValue > _hillThreshold) mapID = "Hill";
                if (noiseValue > _highHillThreshold) mapID = "Mountain";


                if (mapID == "Grass" && noiseValue > _randomTileThreshold)
                {
                    int randomValue = Random.Range(0, 100);
                    if (randomValue < _woodRandomValue)
                    {
                        mapID = ("Forest");
                    }
                    else if (randomValue < _woodRandomValue + _hillRandomValue)
                    {
                        mapID = ("Hill");
                    }
                }
                else if (mapID == "Water")
                {
                    int randomValue = Random.Range(0, 100);
                    if (randomValue < _waterTreasuteValue)
                    {
                        mapID = ("Treasure");
                    }
                }
                GameObject map = GameManager.Instance.MapManager.GetMapPrefab(mapID).mapPrefab;
                Instantiate(map, pos, Quaternion.identity, this.gameObject.transform);
            }
        }

        StartSetting();
    }

    #endregion

    /// <summary>
    /// 플레이어 의 개수를 기반으로 적당한위치에 주요거섬(성)을 생성하는 함수
    /// MakeGridTile이 전부 끝나고 시작
    /// </summary>
    private void StartSetting()
    {
        GameObject castlePrefab = GameManager.Instance.MapManager.GetMapPrefab("Castle").mapPrefab;



    }



}