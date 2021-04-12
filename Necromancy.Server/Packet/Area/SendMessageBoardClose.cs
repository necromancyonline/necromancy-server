using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendMessageBoardClose : ClientHandler
    {
        public SendMessageBoardClose(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort)AreaPacketId.send_message_board_close;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res1 = BufferProvider.Provide();
            //recv_message_board_close_r = 0x6001,
            res1.WriteUInt32(client.character.instanceId);
            router.Send(client, (ushort)AreaPacketId.recv_message_board_close_r, res1, ServerType.Area);

            IBuffer res = BufferProvider.Provide();
            //recv_message_board_notify_close
            //No Structure
            router.Send(client.map, (ushort)AreaPacketId.recv_message_board_notify_close, res, ServerType.Area);

            IBuffer res2 = BufferProvider.Provide();
            //recv_event_sync = 0x462E,
            //No Structure
            router.Send(client, (ushort)AreaPacketId.recv_event_sync, res2, ServerType.Area);
        }
    }
}
