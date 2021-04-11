using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_chara_arrange_notify_update_unlock : PacketResponse
    {
        public recv_chara_arrange_notify_update_unlock()
            : base((ushort) AreaPacketId.recv_chara_arrange_notify_update_unlock, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            for (int i = 0; i < 100; i++)
            { res.WriteInt64(i); } //This is a really complicated loop.  5 groups of int64 written 20 times each.
            return res;
        }
    }
}
