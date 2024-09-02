using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public static HexGrid Instance;
    public Dictionary<Vector3Int, Hex> hexTileDic = new Dictionary<Vector3Int, Hex>();
    private Dictionary<Vector3Int, List<Vector3Int>> hexTileNeighboursDic = new Dictionary<Vector3Int, List<Vector3Int>>();

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void Start()
    {
        foreach (Hex hex in FindObjectsOfType<Hex>())
        {
            if (hex is AgentHex)
            {
                continue;
            }
            hexTileDic.Add(hex.HexCoords, hex);
            Tile tile = hex.GetComponentInParent<Tile>();
            tile.hexCoord = hex.HexCoords;
            tile.EnableCloud();
        }

        foreach (var item in FindObjectsOfType<AgentHex>())
        {
            if (hexTileDic.ContainsKey(item.HexCoords))
            {
                hexTileDic[item.HexCoords].GetComponentInParent<Tile>().DisableCloud();
            }
        }
    }

    public List<Vector3Int> GetAgentsCoordinate()
    {
        var agents = FindObjectsOfType<AgentHex>();
        return agents.Select(t => t.HexCoords).ToList();
    }

    public Hex GetTileAt(Vector3Int hexCoordinates)
    {
        Hex result = null;
        hexTileDic.TryGetValue(hexCoordinates, out result);
        return result;
    }

    public bool CanMove(Vector3Int coord)
    {
        hexTileDic.TryGetValue(coord, out var hex);
        if(hex == null) return false;

        if(hex.IsGlow == false) return false;

        return true;
    }

    public List<Vector3Int> GetNeighboursFor(Vector3Int hexCoordinates)
    {
        if (hexTileDic.ContainsKey(hexCoordinates) == false)
            return new List<Vector3Int>();


        if (hexTileNeighboursDic.ContainsKey(hexCoordinates) && hexTileNeighboursDic[hexCoordinates].Count != 0)
        {
            return hexTileNeighboursDic[hexCoordinates];
        }

        if (hexTileNeighboursDic.ContainsKey(hexCoordinates) == false)
        {
            hexTileNeighboursDic.Add(hexCoordinates, new List<Vector3Int>());
        }
        else
        {
            hexTileNeighboursDic[hexCoordinates].Clear();
        }

        foreach (Vector3Int direction in Direction.GetDirectionList(hexCoordinates.z))//여기가 문제
        {
            if (hexTileDic.ContainsKey(hexCoordinates + direction))
            {
                hexTileNeighboursDic[hexCoordinates].Add(hexCoordinates + direction);
            }
        }

        return hexTileNeighboursDic[hexCoordinates];

    }

}

public static class Direction
{
    public static List<Vector3Int> directionOffsetOdd = new List<Vector3Int>()
    {
        new Vector3Int(-1,0,1), // up1 1
        new Vector3Int(0,0,1), // up2 2
        new Vector3Int(1,0,0), // right 3
        new Vector3Int(0,0,-1), // down2 4
        new Vector3Int(-1,0,-1), // down1 5
        new Vector3Int(-1,0,0) // left 6
    };
    public static List<Vector3Int> directionOffsetEven = new List<Vector3Int>()
    {
        new Vector3Int(0,0,1), // up1
        new Vector3Int(1,0,1),// up2
        new Vector3Int(1,0,0),// right
        new Vector3Int(1,0,-1), // down2
        new Vector3Int(0,0,-1), // down1
        new Vector3Int(-1,0,0)// left
    };

    public static List<Vector3Int> GetDirectionList(int z) => z % 2 == 0 ? directionOffsetEven : directionOffsetOdd;
}