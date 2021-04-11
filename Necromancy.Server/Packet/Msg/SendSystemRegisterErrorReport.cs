using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_system_register_error_report : ClientHandler
    {
        public send_system_register_error_report(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_system_register_error_report;


        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);
            Router.Send(client, (ushort) MsgPacketId.recv_system_register_error_report_r, res, ServerType.Msg);
        }
    }
}
