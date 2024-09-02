using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Tile : MonoBehaviour
{
    [SerializeField] private GameObject originMap;
    [SerializeField] private GameObject cloudMap;
    [HideInInspector] public bool iscloud = true;
    [HideInInspector] public Vector3Int hexCoord;

    [ContextMenu("CloudOff")]
    public void DisableCloud()
    {
        cloudMap.SetActive(false);
        originMap.SetActive(true);
        iscloud = false;
    }

    [ContextMenu("CloudON")]
    public void EnableCloud()
    {
        originMap.SetActive(false);
        cloudMap.SetActive(true);
        iscloud = true;
    }

    [ContextMenu("CastleChange")]
    private void CastleChange()
    {
        GameObject castle = Instantiate(GameManager.Instance.MapManager.GetMapPrefab("Castle").mapPrefab, transform.position, Quaternion.identity, transform.parent);
        HexGrid.Instance.hexTileDic[hexCoord] = castle.GetComponentInChildren<Hex>();
        Destroy(gameObject);
        castle.GetComponent<Tile>().DisableCloud();
    }
}
