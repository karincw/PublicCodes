using Karin.Network;
using System;


namespace Packets
{
    public class PlayerPacket : DataPacket
    {
        public ushort PlayerID;
        public float X;
        public float Y;
        public bool ItemSelected;
        public bool ItemInstalled;
        public bool MoveEnded;

        public PlayerPacket()
        {

        }

        public PlayerPacket(ushort playerID, float x, float y)
        {
            PlayerID = playerID;
            X = x;
            Y = y;
        }

        public PlayerPacket(ushort playerID, bool itemSelected, bool itemInstalled, bool moveEnded)
        {
            PlayerID = playerID;
            ItemSelected = (bool)itemSelected;
            ItemInstalled = (bool)itemInstalled;
            MoveEnded = (bool)moveEnded;
        }

        public PlayerPacket(ushort playerID, float x, float y, bool itemSelected, bool itemInstalled, bool moveEnded)
        {
            PlayerID = playerID;
            X = x;
            Y = y;
            ItemSelected = itemSelected;
            ItemInstalled = itemInstalled;
            MoveEnded = moveEnded;
        }


        public override ushort Deserialize(ArraySegment<byte> buffer, int offset)
        {
            ushort process = 0;
            process += PacketUtility.ReadUShortData(buffer, offset + process, out this.PlayerID);
            process += PacketUtility.ReadFloatData(buffer, offset + process, out this.X);
            process += PacketUtility.ReadFloatData(buffer, offset + process, out this.Y);
            process += PacketUtility.ReadBoolData(buffer, offset + process, out this.ItemSelected);
            process += PacketUtility.ReadBoolData(buffer, offset + process, out this.ItemInstalled);
            process += PacketUtility.ReadBoolData(buffer, offset + process, out this.MoveEnded);

            return process;
        }

        public override ushort Serialize(ArraySegment<byte> buffer, int offset)
        {
            ushort process = 0;
            process += PacketUtility.AppendUShortData(this.PlayerID, buffer, offset + process);
            process += PacketUtility.AppendFloatData(this.X, buffer, offset + process);
            process += PacketUtility.AppendFloatData(this.Y, buffer, offset + process);
            process += PacketUtility.AppendBoolData(this.ItemSelected, buffer, offset + process);
            process += PacketUtility.AppendBoolData(this.ItemInstalled, buffer, offset + process);
            process += PacketUtility.AppendBoolData(this.MoveEnded, buffer, offset + process);

            return process;

        }
    }
}
