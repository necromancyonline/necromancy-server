using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_chara_update_identical_attach_time : PacketResponse
    {
        public recv_chara_update_identical_attach_time()
            : base((ushort) AreaPacketId.recv_chara_update_identical_attach_time, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //identicalId
            res.WriteInt16(0); //diff
            res.WriteInt16(0); //percent
            return res;
        }
    }
}
