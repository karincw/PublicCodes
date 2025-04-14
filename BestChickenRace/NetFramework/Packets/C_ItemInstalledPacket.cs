using Karin.Network;
using System;

namespace Packets
{
    public class C_ItemInstalledPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.C_ItemInstalledPacket;

        public bool Installed;
        public ObjectPacket ObjectData;
        public ushort playerID;

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);

            process += PacketUtility.ReadUShortData(buffer, process, out playerID);
            process += PacketUtility.ReadBoolData(buffer, process, out Installed);
            process += PacketUtility.ReadDataPacket<ObjectPacket>(buffer, process, out ObjectData);
        }

        public override ArraySegment<byte> Serialize()
        {
            ArraySegment<byte> buffer = UniqueBuffer.Open(1024);
            ushort process = 0;

            process += sizeof(ushort);
            process += PacketUtility.AppendUShortData(this.ID, buffer, process);

            process += PacketUtility.AppendUShortData(this.playerID, buffer, process);
            process += PacketUtility.AppendBoolData(Installed, buffer, process);
            process += PacketUtility.AppendDataPacket<ObjectPacket>(ObjectData, buffer, process);
            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }
    }
}
