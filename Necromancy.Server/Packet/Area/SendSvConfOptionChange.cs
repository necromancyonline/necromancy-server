using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendSvConfOptionChange : ClientHandler
    {
        public SendSvConfOptionChange(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_sv_conf_option_change;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //Success?
            router.Send(client, (ushort)AreaPacketId.recv_sv_conf_option_change_r, res, ServerType.Area);
        }
    }
}
