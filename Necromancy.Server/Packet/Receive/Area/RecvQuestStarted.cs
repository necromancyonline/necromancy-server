using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvQuestStarted : PacketResponse
    {
        public RecvQuestStarted()
            : base((ushort)AreaPacketId.recv_quest_started, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteByte(0);

            res.WriteFixedString(" ", 0x61);

            res.WriteInt32(0);
            res.WriteInt32(0);

            res.WriteFixedString(" ", 0x61);

            res.WriteByte(1); // bool
            res.WriteByte(1); // bool

            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);

            int numEntries = 0xA;
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(0);
                res.WriteFixedString(" ", 0x10);
                res.WriteInt16(0);
                res.WriteInt32(0);
            }

            res.WriteByte(0);

            int numEntries2 = 0xC;
            for (int i = 0; i < numEntries2; i++)
            {
                res.WriteInt32(0);
                res.WriteFixedString(" ", 0x10);
                res.WriteInt16(0);
                res.WriteInt32(0);
            }

            res.WriteByte(0);

            res.WriteFixedString(" ", 0x181);

            res.WriteInt64(0);

            res.WriteByte(0);

            res.WriteFixedString(" ", 0x181);

            int numEntries3 = 0x5;
            for (int i = 0; i < numEntries3; i++)
            {
                res.WriteByte(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
            }

            res.WriteByte(0);

            res.WriteInt32(0);

            res.WriteFloat(0);
            return res;
        }
    }
}
