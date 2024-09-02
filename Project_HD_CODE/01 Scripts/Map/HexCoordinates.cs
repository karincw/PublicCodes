using UnityEngine;

public class HexCoordinates : MonoBehaviour
{
    public static float xOffset = 1, yOffset = 100f, zOffset = 0.86f;

    [Header("Offset Coordinates")]
    [SerializeField]
    private Vector3Int offsetCoordinates;

    private void Awake()
    {
        offsetCoordinates = ConvertPositionToOffset(transform.position);
    }

    public Vector3Int GetHexCoords() => ConvertPositionToOffset(transform.position);

    public static Vector3Int ConvertPositionToOffset(Vector3 position)
    {
        int x = Mathf.CeilToInt(position.x / xOffset);
        int y = Mathf.RoundToInt(position.y / yOffset);
        int z = Mathf.RoundToInt(position.z / zOffset);
        return new Vector3Int(x, y, z);
    }

    public static Vector3 ConvertOffsetToPosition(Vector3Int position)
    {
        float x = position.x * xOffset;
        float y = position.y * yOffset;
        float z = position.z * zOffset;
        return new Vector3(x, y, z);
    }
}
