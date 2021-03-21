using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class send_raisescale_request_revive_event : ClientHandler
    {
        public send_raisescale_request_revive_event(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_raisescale_request_revive_event;

        public override void Handle(NecClient client, NecPacket packet)
        {

            IBuffer res22 = BufferProvider.Provide();
            res22.WriteInt32(1); // 0 = normal 1 = cinematic
            res22.WriteByte(0);
            Router.Send(client, (ushort)AreaPacketId.recv_event_start, res22, ServerType.Area);
            //if success
            RecvEventScriptPlay recvEventScriptPlay1 = new RecvEventScriptPlay("scale\revive_success", client.Character.InstanceId);
            Router.Send(recvEventScriptPlay1, client);
            //if fail
            RecvEventScriptPlay recvEventScriptPlay2 = new RecvEventScriptPlay("scale\revive_fail", client.Character.InstanceId);
            //Router.Send(recvEventScriptPlay2, client);
            //if fail again. you're lost
            RecvEventScriptPlay recvEventScriptPlay3 = new RecvEventScriptPlay("scale\revive_lost", client.Character.InstanceId);
            //Router.Send(recvEventScriptPlay3, client);
        }
    }
}
