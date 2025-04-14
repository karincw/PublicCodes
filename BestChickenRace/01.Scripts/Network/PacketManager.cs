using Karin.Network;
using Packets;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PacketManager
{
    private static PacketManager instance;
    public static PacketManager Instance
    {
        get
        {
            if(instance == null)
                instance = new PacketManager();
            return instance;
        }
    }

    private Dictionary<ushort, Func<ArraySegment<byte>, Packet>> packetFactories = new();
    private Dictionary<ushort, Action<Session, Packet>> packetHandlers = new();

    public PacketManager()
    {
        packetFactories.Clear();
        packetHandlers.Clear();

        RegisterHandler();
    }

    private void RegisterHandler()
    {
        packetFactories.Add((ushort)PacketID.S_RoomEnterPacket, PacketUtility.CreatePacket<S_RoomEnterPacket>);
        packetHandlers.Add((ushort)PacketID.S_RoomEnterPacket, PacketHandler.S_RoomEnterPacket);

        packetFactories.Add((ushort)PacketID.S_PlayerJoinPacket, PacketUtility.CreatePacket<S_PlayerJoinPacket>);
        packetHandlers.Add((ushort)PacketID.S_PlayerJoinPacket, PacketHandler.S_PlayerJoinPacket);

        packetFactories.Add((ushort)PacketID.S_MovePacket, PacketUtility.CreatePacket<S_MovePacket>);
        packetHandlers.Add((ushort)PacketID.S_MovePacket, PacketHandler.S_MovePacket);

        packetFactories.Add((ushort)PacketID.S_SelectEndPacket, PacketUtility.CreatePacket<S_SelectEndPacket>);
        packetHandlers.Add((ushort)PacketID.S_SelectEndPacket, PacketHandler.S_SelectEndPacket);

        packetFactories.Add((ushort)PacketID.S_ItemInstalledPacket, PacketUtility.CreatePacket<S_ItemInstalledPacket>);
        packetHandlers.Add((ushort)PacketID.S_ItemInstalledPacket, PacketHandler.S_ItemInstalledPacket);

        packetFactories.Add((ushort)PacketID.S_InstallEndPacket, PacketUtility.CreatePacket<S_InstallEndPacket>);
        packetHandlers.Add((ushort)PacketID.S_InstallEndPacket, PacketHandler.S_InstallEndPacket);

        packetFactories.Add((ushort)PacketID.S_MoveEndedPacket, PacketUtility.CreatePacket<S_MoveEndedPacket>);
        packetHandlers.Add((ushort)PacketID.S_MoveEndedPacket, PacketHandler.S_MoveEndedPacket);

        packetFactories.Add((ushort)PacketID.S_GameEndPacket, PacketUtility.CreatePacket<S_GameEndPacket>);
        packetHandlers.Add((ushort)PacketID.S_GameEndPacket, PacketHandler.S_GameEndPacket);

    }

    public Packet CreatePacket(ArraySegment<byte> buffer)
    {
        ushort packetID = PacketUtility.ReadPacketID(buffer);

        if (packetFactories.ContainsKey(packetID))
            return packetFactories[packetID]?.Invoke(buffer);
        else
            return null;
    }

    public void HandlePacket(Session session, Packet packet)
    {
        if (packet != null)
            if (packetHandlers.ContainsKey(packet.ID)) // 핸들러가 존재하면
                packetHandlers[packet.ID]?.Invoke(session, packet); // 패킷 핸들링
    }
}

