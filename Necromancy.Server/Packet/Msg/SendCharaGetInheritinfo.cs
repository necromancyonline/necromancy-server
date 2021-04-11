using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class SendCharaGetInheritinfo : ClientHandler
    {
        public SendCharaGetInheritinfo(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) MsgPacketId.send_chara_get_inheritinfo;


        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();


            router.Send(client, (ushort) MsgPacketId.recv_base_login_r, res, ServerType.Msg);
        }
    }
}