using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class SendRaisescaleRequestReviveEvent : ClientHandler
    {
        public SendRaisescaleRequestReviveEvent(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_raisescale_request_revive_event;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res22 = BufferProvider.Provide();
            res22.WriteInt32(1); // 0 = normal 1 = cinematic
            res22.WriteByte(0);
            router.Send(client, (ushort)AreaPacketId.recv_event_start, res22, ServerType.Area);
            //if success
            RecvEventScriptPlay recvEventScriptPlay1 = new RecvEventScriptPlay("scale\revive_success", client.character.instanceId);
            router.Send(recvEventScriptPlay1, client);
            //if fail
            RecvEventScriptPlay recvEventScriptPlay2 = new RecvEventScriptPlay("scale\revive_fail", client.character.instanceId);
            //Router.Send(recvEventScriptPlay2, client);
            //if fail again. you're lost
            RecvEventScriptPlay recvEventScriptPlay3 = new RecvEventScriptPlay("scale\revive_lost", client.character.instanceId);
            //Router.Send(recvEventScriptPlay3, client);
        }
    }
}
