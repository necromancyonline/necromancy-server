using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_sv_conf_option_change : ClientHandler
    {
        public send_sv_conf_option_change(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_sv_conf_option_change;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);//Success?
            Router.Send(client, (ushort)AreaPacketId.recv_sv_conf_option_change_r, res, ServerType.Area);


        }


    }
}
