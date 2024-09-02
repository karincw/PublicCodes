using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(HexCoordinates))]
public class Hex : MonoBehaviour
{
    internal GlowHighlight _highLight;
    [SerializeField] internal HexCoordinates _hexCoordinates;

    [SerializeField] private HexType _hexType;

    public Vector3Int HexCoords => HexCoordinates.ConvertPositionToOffset(transform.position);
    public bool IsGlow = false;
    public bool CanAttack = false;

    private void Awake()
    {
        _hexCoordinates = GetComponent<HexCoordinates>();
        _highLight = GetComponent<GlowHighlight>();
    }

    public virtual void EnableMovementHighlight()
    {
        _highLight.ToggleMovementGlow(true);
        IsGlow = true;
    }
    public virtual void DisableMovementHighlight()
    {
        _highLight.ToggleMovementGlow(false);
        IsGlow = false;
    }

    public virtual void EnableAttackHighlight()
    {
        _highLight.ToggleAttackGlow(true);
        CanAttack = true;
    }
    public virtual void DisableAttackHighlight()
    {
        _highLight.ToggleAttackGlow(false);
        CanAttack = false;
    }

    public bool CanMove()
    {
        List<Vector3Int> agentpos = HexGrid.Instance.GetAgentsCoordinate();
        
        if (_hexType == HexType.Obstacle
            || IsGlow == false)
        {
            return false;
        }
        return true;
    }

    public int GetCost()
    {
        switch (_hexType)
        {
            case HexType.Default:
                return 1;
            case HexType.Water:
                return 1;
            default:
                Debug.LogError("해당 HexType은 호환되지 않습니다.");
                return -1;
        }
    }

    public void DefineTile(int sight)
    {
        HashSet<Vector3Int> neighbours = HexGrid.Instance.GetNeighboursFor(HexCoords).ToHashSet();

        for (int i = 1; i < sight; ++i)
        {
            HashSet<Vector3Int> nei = new HashSet<Vector3Int>();
            foreach (var item in neighbours)
            {
                var addItem = HexGrid.Instance.GetNeighboursFor(HexGrid.Instance.GetTileAt(item).HexCoords).ToHashSet();

                nei.UnionWith(addItem);
            }
            neighbours.UnionWith(nei);
        }

        foreach (var item in neighbours)
        {
            Tile currentTile = HexGrid.Instance.GetTileAt(item).GetComponentInParent<Tile>();
            if (currentTile != null)
            {
                currentTile.DisableCloud();
            }
        }
    }

}

public enum HexType : ushort
{
    None,
    Default,
    Water,
    Obstacle
}