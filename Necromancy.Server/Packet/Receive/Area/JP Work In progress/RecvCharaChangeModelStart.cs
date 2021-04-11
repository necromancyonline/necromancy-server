using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_chara_change_model_start : PacketResponse
    {
        public recv_chara_change_model_start()
            : base((ushort) AreaPacketId.recv_chara_change_model_start, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //neat!
            res.WriteInt16(100);
            return res;
        }
    }
}
