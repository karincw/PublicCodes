using Packets;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    private Dictionary<ushort, OtherPlayer> otherplayers = new Dictionary<ushort, OtherPlayer>();

    [SerializeField] private OtherPlayer playerPrefab;
    public int playerID = -1;

    private void Awake()
    {
        if (GameManager.instance == null)
        {
            GameManager.instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        NetworkManager.Instance = gameObject.AddComponent<NetworkManager>();
        SceneLoader.Instance = gameObject.AddComponent<SceneLoader>();
    }

    public void AddPlayer(PlayerPacket p)
    {
        OtherPlayer player = Instantiate(playerPrefab, new Vector2(p.X, p.Y), Quaternion.identity);
        otherplayers.Add(p.PlayerID, player);
    }

    public OtherPlayer GetPlayer(ushort id)
    {
        if (otherplayers.ContainsKey(id))
        {
            return otherplayers[id];
        }
        else
        {
            return null;
        }
    }

    public void InstallBlock(ObjectPacket data)
    {
        GameObject obj = PoolManager.instance.Spawn(data.ObjectName);
        obj.transform.position = new Vector2(data.X, data.Y);
        obj.transform.localRotation = Quaternion.Euler(0, 0, data.Rotation);
    }
}
