using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_union_request_set_info : ClientHandler
    {
        public send_union_request_set_info(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_union_request_set_info;


        public override void Handle(NecClient client, NecPacket packet)
        {
            uint newsUpdatedByInstanceId = packet.Data.ReadUInt32();
            string unionNews = packet.Data.ReadCString(); //Max size 0x196?

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //error check.  0 for success.  See sys_msg.csv for all error check messages.
            /*
                    0	Union news changed
                GENERIC	Failed to change the union news
                -1709	You do not have permission to change the news
                -1715	The news contained banned words
            */

            Router.Send(client, (ushort) MsgPacketId.recv_union_request_set_info_r, res, ServerType.Msg);

            //ToDo
            //L"network::proto_msg_implement_client::recv_union_notify_info()\n"
            //Permissions check for news updating based on 'newsUpdatedByInstanceId'. Returns 0 or 1709
            //Naughty word scrubbing for 'banned words' returns 1715


            IBuffer res2 = BufferProvider.Provide();

            res2.WriteInt32(0); //Error check probably.  0 means success?
            res2.WriteCString($"{unionNews}"); //max size 0x196

            Router.Send(client.Union.UnionMembers, (ushort) MsgPacketId.recv_union_notify_info, res2,
                ServerType.Msg);
        }
    }
}
