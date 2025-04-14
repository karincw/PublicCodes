using Karin.Network;
using System;

namespace Packets
{
    public class C_MoveEndedPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.C_MoveEndedPacket;

        public ushort PlayerID;
        public bool MoveEnded;

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);

            process += PacketUtility.ReadUShortData(buffer, process, out PlayerID);
            process += PacketUtility.ReadBoolData(buffer, process, out MoveEnded);
        }

        public override ArraySegment<byte> Serialize()
        {
            ArraySegment<byte> buffer = UniqueBuffer.Open(1024);
            ushort process = 0;

            process += sizeof(ushort);
            process += PacketUtility.AppendUShortData(this.ID, buffer, process);
            process += PacketUtility.AppendUShortData(this.PlayerID, buffer, process);
            process += PacketUtility.AppendBoolData(this.MoveEnded, buffer, process);
            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }
    }
}
