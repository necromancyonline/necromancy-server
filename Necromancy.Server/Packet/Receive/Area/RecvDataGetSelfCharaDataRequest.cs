using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvDataGetSelfCharaDataRequest : PacketResponse
    {
        public RecvDataGetSelfCharaDataRequest()
            : base((ushort) AreaPacketId.recv_data_get_self_chara_data_request_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            //Text file says no structure, is this the long struct and we just skipped??//  i blame bank.  Copy from send_data_get_self...
            return res;
        }
    }
}
