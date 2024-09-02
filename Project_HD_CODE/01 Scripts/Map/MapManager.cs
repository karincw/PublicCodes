using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    public List<Map> mapPrefabs;

    public Map GetMapPrefab(string mapName)
    {
        return mapPrefabs.Find(t => t.mapID == mapName);
    }
}

[System.Serializable]
public struct Map
{
    public string mapID;
    public GameObject mapPrefab;
    public bool CanMove;
}
