using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvSoulPartnerExpeditionResultR : PacketResponse
    {
        public RecvSoulPartnerExpeditionResultR()
            : base((ushort) AreaPacketId.recv_soul_partner_expedition_result_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(1);
            //sub_496130
            res.WriteInt32(1);
            res.WriteInt32(1);

            for (int i = 0; i < 0xA; i++)
            {
                res.WriteInt64(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteFixedString("Xeno", 0x10);
            }
            //end sub
            return res;
        }
    }
}
