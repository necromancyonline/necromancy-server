using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvDbgCharaUnequipped : PacketResponse
    {
        public RecvDbgCharaUnequipped()
            : base((ushort)AreaPacketId.recv_dbg_chara_unequipped, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);

            res.WriteInt32(0);
            return res;
        }
    }
}
