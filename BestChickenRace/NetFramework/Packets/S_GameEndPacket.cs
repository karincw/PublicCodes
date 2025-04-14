using Karin.Network;
using Packets;
using System;

public class S_GameEndPacket : Packet
{
    public override ushort ID => (ushort)PacketID.S_GameEndPacket;

    public override void Deserialize(ArraySegment<byte> buffer)
    {

    }

    public override ArraySegment<byte> Serialize()
    {
        ArraySegment<byte> buffer = UniqueBuffer.Open(1024);
        ushort process = 0;

        process += sizeof(ushort);
        process += PacketUtility.AppendUShortData(this.ID, buffer, process);

        PacketUtility.AppendUShortData(process, buffer, 0);

        return UniqueBuffer.Close(process);
    }
}
