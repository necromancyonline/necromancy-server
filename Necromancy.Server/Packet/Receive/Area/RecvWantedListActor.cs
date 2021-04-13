using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvWantedListActor : PacketResponse
    {
        public RecvWantedListActor()
            : base((ushort)AreaPacketId.recv_wanted_list_actor, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            int numEntries = 0x1E;
            res.WriteInt32(numEntries); //less than or equal to 0x1E
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(0);
                res.WriteFixedString("", 0x31);
                res.WriteInt32(0);
                res.WriteFixedString("", 0x5B);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
            }

            res.WriteInt32(0);
            return res;
        }
    }
}
