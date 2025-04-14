using Karin.Network;
using System;
namespace Packets
{
    public class ObjectPacket : DataPacket
    {
        public string ObjectName;
        public int X;
        public int Y;
        public float Rotation;


        public ObjectPacket() { }

        public ObjectPacket(int x, int y, float rotation, string objectName)
        {
            ObjectName = objectName;
            X = x;
            Y = y;
            Rotation = rotation;
        }

        public override ushort Deserialize(ArraySegment<byte> buffer, int offset)
        {
            ushort process = 0;

            process += PacketUtility.ReadStringData(buffer, offset + process, out this.ObjectName);
            process += PacketUtility.ReadIntData(buffer, offset + process, out this.X);
            process += PacketUtility.ReadIntData(buffer, offset + process, out this.Y);
            process += PacketUtility.ReadFloatData(buffer, offset + process, out this.Rotation);

            return process;
        }

        public override ushort Serialize(ArraySegment<byte> buffer, int offset)
        {
            ushort process = 0;

            process += PacketUtility.AppendStringData(this.ObjectName, buffer, offset + process);
            process += PacketUtility.AppendIntData(this.X, buffer, offset + process);
            process += PacketUtility.AppendIntData(this.Y, buffer, offset + process);
            process += PacketUtility.AppendFloatData(this.Rotation, buffer, offset + process);

            return process;
        }
    }
}
