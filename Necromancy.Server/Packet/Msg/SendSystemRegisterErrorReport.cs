using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class SendSystemRegisterErrorReport : ClientHandler
    {
        public SendSystemRegisterErrorReport(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) MsgPacketId.send_system_register_error_report;


        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);
            router.Send(client, (ushort) MsgPacketId.recv_system_register_error_report_r, res, ServerType.Msg);
        }
    }
}
