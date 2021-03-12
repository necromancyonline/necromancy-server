using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_login_news_url_notify : PacketResponse
    {
        public recv_login_news_url_notify()
            : base((ushort) AreaPacketId.recv_login_news_url_notify, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            res.WriteCString("What");
            return res;
        }
    }
}
