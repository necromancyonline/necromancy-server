using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaUpdateCon : PacketResponse
    {
        private byte _con;
        public RecvCharaUpdateCon(byte con)
            : base((ushort) AreaPacketId.recv_chara_update_con, ServerType.Area)
        {
            _con = con;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(_con);
            return res;
        }
    }
}
