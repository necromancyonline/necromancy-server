using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class SendSoulSelect : ClientHandler
    {
        public SendSoulSelect(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)MsgPacketId.send_soul_select;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); // Error: 0 - Success, other values (maybe) erro code
            res.WriteByte(1); // 0 - Request password for enter, 1 - Set new password     (bool type)
            //Router.Send(client, (ushort)MsgPacketId.recv_soul_select_r, res, ServerType.Msg);

            // res.WriteInt32(1); // 0 = OK | 1 = Failed to return to soul selection
            // Router.Send(client, (ushort) MsgPacketId.recv_chara_select_back_soul_select_r, res, ServerType.Msg);
        }
    }
}
