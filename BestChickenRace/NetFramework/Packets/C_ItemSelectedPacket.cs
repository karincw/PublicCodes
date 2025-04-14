using Karin.Network;
using System;


namespace Packets
{
    public class C_ItemSelectedPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.C_ItemSelectedPacket;

        public bool Selected;
        public ushort playerID;

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);

            process += PacketUtility.ReadUShortData(buffer, process, out playerID);
            process += PacketUtility.ReadBoolData(buffer, process, out Selected);
        }

        public override ArraySegment<byte> Serialize()
        {
            ArraySegment<byte> buffer = UniqueBuffer.Open(1024);
            ushort process = 0;

            process += sizeof(ushort);
            process += PacketUtility.AppendUShortData(this.ID, buffer, process);
            process += PacketUtility.AppendUShortData(this.playerID, buffer, process);
            process += PacketUtility.AppendBoolData(this.Selected, buffer, process);
            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }
    }
}
